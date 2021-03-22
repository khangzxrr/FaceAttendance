using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpeedyAPI.Data;
using SpeedyAPI.Extensions;
using SpeedyAPI.Filters;
using SpeedyAPI.Models;
using SpeedyAPI.Models.WebcamAttendance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyAPI.Controllers
{

    public class WebcamAttendanceController : Controller
    {
        private string insightFacelURL = "http://localhost:18081";
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IWebHostEnvironment environment;

        private DBStudentContext dBStudentContext;

        public string STUDENT_FACE_VECTORS = "student_face_vectors";

        public WebcamAttendanceController(IHttpClientFactory clientFactory, DBStudentContext dBStudentContext, IWebHostEnvironment environment)
        {
            httpClientFactory = clientFactory;
            this.dBStudentContext = dBStudentContext;
            this.environment = environment;
        }

        private async Task<List<float[]>> RequestApiToIdentify(JArray base64Images)
        {
            List<float[]> identityFaces = new List<float[]>();

            var jsonObject = new JObject(
                    new JProperty("images",
                        new JObject(
                            new JProperty("data",
                                base64Images)
                        )
                    ),
                    new JProperty("extract_ga", false)
                );


            var content = new StringContent(jsonObject.ToString(Formatting.None), Encoding.ASCII, "application/json");

            var request = new HttpRequestMessage(
            HttpMethod.Post,
            insightFacelURL + "/extract");

            request.Content = content;

            var client = httpClientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var serializer = new JsonSerializer();

                using (var sr = new StreamReader(response.Content.ReadAsStream()))
                {
                    using (var jsonReader = new JsonTextReader(sr))
                    {
                        var message = serializer.Deserialize<JArray>(jsonReader);
                    
                        foreach(JArray person in message)
                        {
                            if (person.Count > 0)
                            {
                                var vector = person[0]["vec"].ToObject<float[]>();
                                identityFaces.Add(vector);
                            }
                        }
                        
                    }

                }


            }
          

            return identityFaces;
        }
        [SchoolManageFilter]
        public async Task<IActionResult> Index()
        {
            var school = HttpContext.Session.Get<SchoolAccount>(SchoolAccountsController.SCHOOL_SESSION_ACCOUNT_ID);

            //get students of this school
            var students = dBStudentContext.Students.Where(s => s.school_id == school.id).ToList();

            

            JArray studentBase64Images = new JArray();
            foreach(var student in students)
            {
                var imagePath = environment.WebRootPath + "/upload/" + student.image_url;
                byte[] imageArray = System.IO.File.ReadAllBytes(imagePath);
                string base64Encoded = Convert.ToBase64String(imageArray);

                studentBase64Images.Add(base64Encoded);

                Console.WriteLine($"Added {student.image_url} to base64 array");
            }


            var listOfIdentityVectors = await RequestApiToIdentify(studentBase64Images);


            var studentVecs = new List<StudentFaceVector>();
            
            for(int i = 0; i < listOfIdentityVectors.Count; i++)
            {
                studentVecs.Add(new StudentFaceVector(students[i], listOfIdentityVectors[i]));
            }

            var studentFaceVectors = new StudentFaceVectors();
            studentFaceVectors.studentFaceVectors = studentVecs;

            HttpContext.Session.Set<StudentFaceVectors>(STUDENT_FACE_VECTORS, studentFaceVectors);

            return View();
        }

        [HttpPost]
        public async Task<ActionResult<HttpResponseMessage>> Identity(string base64)
        {
            if (HttpContext.Session.Get<StudentFaceVectors>(STUDENT_FACE_VECTORS) == null)
            {
                return RedirectToAction("Index");
            }

            var faceVectors = HttpContext.Session.Get<StudentFaceVectors> (STUDENT_FACE_VECTORS);

            var base64Images = new JArray();
            base64Images.Add(base64);

            var currentVec = await RequestApiToIdentify(base64Images);

            if (currentVec.Count > 0)
            {
                var cloestStudent = faceVectors.GetNearestVectorDistance(currentVec[0]);
                if (faceVectors.minDistance >= 1)
                {
                    return Ok(null);
                }
                else
                {
                    return Ok(cloestStudent);
                }
                
            }
            
            return Ok(null);
        }
        [HttpGet]
        public async Task<ActionResult<HttpResponseMessage>> Test()
        {
            var request = new HttpRequestMessage(
            HttpMethod.Get,
            insightFacelURL + "/info");

            var client = httpClientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                return Ok(message);
            }
            else
            {
                return NotFound();
            }

        }
    }
}

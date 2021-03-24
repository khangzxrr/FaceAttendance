using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private DBTeacherContext dBTeacherContext;
        private DBSubjectContext dBSubjectContext;
        private DBMajorContext dBMajorContext;
        private readonly DBAttendanceContext attendanceContext;

        public static string STUDENT_FACE_VECTORS = "student_face_vectors";

        public static string TEACHER_LOGGED = "TEACHER_LOGGED";

        public string SELECTED_ROOM = "SELECTED_ROOM";

        public WebcamAttendanceController(IHttpClientFactory clientFactory,
                  DBStudentContext dBStudentContext,
                  DBTeacherContext dBTeacherContext,
                  DBAttendanceContext attendanceContext,
                  DBSubjectContext dBSubjectContext,
                  DBMajorContext dBMajorContext,
                  IWebHostEnvironment environment)
        {
            httpClientFactory = clientFactory;
            this.dBStudentContext = dBStudentContext;
            this.dBTeacherContext = dBTeacherContext;
            this.attendanceContext = attendanceContext;
            this.dBSubjectContext = dBSubjectContext;
            this.dBMajorContext = dBMajorContext;
            this.environment = environment;
        }

        private async Task<string> RequestLandmarkImage(string base64)
        {
            var jsonArrayImage = new JArray();
            jsonArrayImage.Add(base64);

            var jsonObject = new JObject(
                    new JProperty("images",
                        new JObject(
                            new JProperty("data",
                                jsonArrayImage)
                        )
                    )
                );

            var content = new StringContent(jsonObject.ToString(Formatting.None), Encoding.ASCII, "application/json");

            var request = new HttpRequestMessage(
           HttpMethod.Post,
           insightFacelURL + "/draw_detections");

            request.Content = content;

            var client = httpClientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var imageContent = await response.Content.ReadAsByteArrayAsync();
                return Convert.ToBase64String(imageContent);
            }

            return null;
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

                        foreach (JArray person in message)
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


        [TeacherFilter]
        [HttpPost]
        public async Task<IActionResult> ExtractFaceData(string base64)
        {
            var image = await RequestLandmarkImage(base64);
            var studentImage = new StudentImage();
            studentImage.base64Image = image;
            return Ok(studentImage);
        }


        [TeacherFilter]
        public async Task<IActionResult> Recognition()
        {
            if (HttpContext.Session.Get<Room>(SELECTED_ROOM) == null)
            {
                return RedirectToAction("Index");
            }

            var room = HttpContext.Session.Get<Room>(SELECTED_ROOM);
            //get students of this school
            var attendances = attendanceContext.Attendances
                                            .Where(a => a.id_subject == room.selectedSubjectId)
                                            .Include(a => a.student)
                                            .ToList();
            room.attendances = attendances;

            JArray studentBase64Images = new JArray();
            foreach (var attendance in attendances)
            {
                var imagePath = environment.WebRootPath + "/upload/" + attendance.student.image_url;
                byte[] imageArray = System.IO.File.ReadAllBytes(imagePath);
                string base64Encoded = Convert.ToBase64String(imageArray);

                studentBase64Images.Add(base64Encoded);

                Console.WriteLine($"Added {attendance.student.image_url} to base64 array");
            }

            var listOfIdentityVectors = await RequestApiToIdentify(studentBase64Images);


            var studentVecs = new List<StudentFaceVector>();

            for (int i = 0; i < listOfIdentityVectors.Count; i++)
            {
                studentVecs.Add(new StudentFaceVector(attendances[i].student, listOfIdentityVectors[i]));
            }

            var studentFaceVectors = new StudentFaceVectors();
            studentFaceVectors.studentFaceVectors = studentVecs;

            HttpContext.Session.Set(STUDENT_FACE_VECTORS, studentFaceVectors);

            return View();
        }


        [HttpGet]
        [TeacherFilter]
        public IActionResult ChooseSubject()
        {
            var teacher = HttpContext.Session.Get<TeacherAccount>(TEACHER_LOGGED);
            ViewBag.majors = dBMajorContext.Majors.Where(m => m.school_id == teacher.teach_in_school).ToList();

            return View();
        }

        [HttpPost]
        [TeacherFilter]
        public async Task<IActionResult> SaveAttendances()
        {
            if (HttpContext.Session.Get<Room>(SELECTED_ROOM) == null)
            {
                return RedirectToAction("Index");
            }

            var room = HttpContext.Session.Get<Room>(SELECTED_ROOM);
            try
            {
                foreach (var attendance in room.attendances)
                {
                    attendanceContext.Update(attendance);
                }
                await attendanceContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                ViewBag.error = "ERROR When saving attendances.. please contact developer";
            }

            ViewBag.success = "Saved attendances!";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [TeacherFilter]
        [ValidateAntiForgeryToken]
        public IActionResult ChooseSubject([Bind("selectedMajorId, selectedSubjectId")] Room room)
        {

            var teacher = HttpContext.Session.Get<TeacherAccount>(TEACHER_LOGGED);

            if (room.selectedSubjectId == -1 || room.selectedSubjectId == 0)
            {
                ViewBag.subjects = dBSubjectContext.Subjects.Where(s => s.major_id == room.selectedMajorId).ToList();
                ViewBag.majors = dBMajorContext.Majors.Where(m => m.school_id == teacher.teach_in_school).ToList();
                return View(room);
            }

            room.subject = dBSubjectContext.Subjects.Where(s => s.id == room.selectedSubjectId).FirstOrDefault();
            room.attendances = attendanceContext.Attendances
                                                .Where(a => a.id_subject == room.selectedSubjectId)
                                                .Include(a => a.student)
                                                .ToList();

            HttpContext.Session.Set<Room>(SELECTED_ROOM, room);

            return RedirectToAction("Recognition");
        }



        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TeacherLogin([Bind("email,password")] TeacherLogin teacher)
        {
            if (ModelState.IsValid)
            {
                var teachers = dBTeacherContext.TeacherAccounts
                                .Where(t => t.email == teacher.email && t.password == teacher.password)
                                .ToList();

                if (teachers.Count > 0)
                {
                    HttpContext.Session.Set(TEACHER_LOGGED, teachers.First());
                    return RedirectToAction("ChooseSubject");
                }
            }
            return View("Index");
        }

        [TeacherFilter]
        public IActionResult GetAttendances()
        {
            return Ok(HttpContext.Session.Get<Room>(SELECTED_ROOM));
        }

        [HttpPost]
        [TeacherFilter]
        public async Task<ActionResult<HttpResponseMessage>> Identity(string base64)
        {
            if (HttpContext.Session.Get<StudentFaceVectors>(STUDENT_FACE_VECTORS) == null)
            {
                return RedirectToAction("Index");
            }
            if (HttpContext.Session.Get<Room>(SELECTED_ROOM) == null)
            {
                return RedirectToAction("Index");
            }

            var faceVectors = HttpContext.Session.Get<StudentFaceVectors>(STUDENT_FACE_VECTORS);

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


                var room = HttpContext.Session.Get<Room>(SELECTED_ROOM);
                //2000 is default year that mean he/she s not recognized
                var attendance = room.attendances.Where(a => a.id_student == cloestStudent.id && a.checkin.Year == 2000)
                                .FirstOrDefault();
                if (attendance != null)
                {
                    attendance.checkin = DateTime.Now;
                    HttpContext.Session.Set<Room>(SELECTED_ROOM, room);
                }


                return Ok(cloestStudent);
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

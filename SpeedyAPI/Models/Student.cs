using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpeedyAPI.Models
{
    [Table("Students")]
    public class Student
    {
        
        public Student()
        {

        }
        public Student(Student student)
        {
            this.id = student.id;
            this.image_url = student.image_url;
            this.name = student.name;
        }

        [Required]
        public int id { get; set; }

        [Required]
        [MaxLength(100)]
        public string name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime date_of_birth { get; set; }

        [Required]
        public int school_id { get; set; }

        public string image_url { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        [Required]
        public IFormFile imageFile { get; set; }
    }
}

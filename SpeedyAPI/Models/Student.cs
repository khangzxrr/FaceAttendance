using System;
using System.ComponentModel.DataAnnotations;

namespace SpeedyAPI.Models
{
    public class Student
    {
        [Required]
        public int id { get; set; }

        [Required]
        [MaxLength(100)]
        public string name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime date_of_birth { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpeedyAPI.Models
{
    public class SchoolAccount
    {
        public int id { get; set; }
        [MaxLength(100)]
        public string name { get; set; }
        [Required]
        [MaxLength(30)]
        public string username { get; set; }
        [Required]
        [MaxLength(30)]
        public string password { get; set; }

        public List<Major> Majors { get; set;}
    }
}

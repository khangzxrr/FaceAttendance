using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpeedyAPI.Models
{
    public class Major
    {
        public int id { get; set; }

        [Required]
        public int school_id { get; set; }
        [Required]
        [MaxLength(100)]
        public string name { get; set; }
        [Required]
        public DateTime startDate { get; set; }

        public List<Student> Students;
    }
}

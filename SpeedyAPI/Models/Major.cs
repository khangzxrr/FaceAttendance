using System;
using System.ComponentModel.DataAnnotations;

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
    }
}

using SpeedyAPI.Validations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpeedyAPI.Models
{
    [Table("Majors")]
    public class Major
    {
        public int id { get; set; }

        [Required]
        public int school_id { get; set; }
        [Required]
        [MaxLength(100)]
        public string name { get; set; }
        [Required]
        [MustBeInPresent]
        public DateTime startDate { get; set; }
    }
}

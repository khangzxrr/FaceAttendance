using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpeedyAPI.Models
{
    [Table("Attendances")]
    public class Attendance
    {
        [Required]
        public int id { get; set; }
        [Required]
        public int id_subject { get; set; }
        [Required]
        public int id_student { get; set; }
        public DateTime checkin { get; set; }
        public DateTime checkout { get; set; }
    }
}

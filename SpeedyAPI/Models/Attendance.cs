﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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


        [ForeignKey("id_subject")]
        public Subject subject { get; set; }
        [ForeignKey("id_student")]
        public Student student { get; set; }

    }
}

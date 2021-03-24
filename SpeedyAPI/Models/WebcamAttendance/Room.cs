using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpeedyAPI.Models.WebcamAttendance
{
    public class Room
    {
        [Required]
        public int selectedMajorId { get; set; }
        public int selectedSubjectId { get; set; }

        public DateTime startTime { get; set; }
        public Subject subject { get; set; }
        public List<Attendance> attendances { get; set; }
    }
}

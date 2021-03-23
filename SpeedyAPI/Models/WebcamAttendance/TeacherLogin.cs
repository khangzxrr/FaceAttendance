using System.ComponentModel.DataAnnotations;

namespace SpeedyAPI.Models.WebcamAttendance
{
    public class TeacherLogin
    {
        [Required]
        [MaxLength(255)]
        public string email { get; set; }
        [Required]
        [MaxLength(255)]
        public string password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpeedyAPI.Models
{
    [Table("Subjects")]
    public class Subject
    {
        [Required]
        public int id {get;set;}
        [Required]
        [MaxLength(100)]
        public string name { get; set; }
        [Required]
        [MaxLength(100)]
        public string room { get; set; }
        [Required]
        public int major_id { get; set; }
        [Required]
        public int teacher_observer { get; set; }

        [ForeignKey("teacher_observer")]
        public TeacherAccount teacherAccount { get; set; }
    }
}

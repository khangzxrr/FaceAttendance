using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpeedyAPI.Models
{
    [Table("SchoolAccounts")]
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

        [ForeignKey("school_id")]
        public List<Major> Majors { get; set;}

        [ForeignKey("school_id")]
        public List<Student> Students { get; set; }

        [ForeignKey("teach_in_school")]
        public List<TeacherAccount> TeacherAccounts { get; set; }
    }
}

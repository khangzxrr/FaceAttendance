using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpeedyAPI.Models
{
    [Table("TeacherAccounts")]
    public class TeacherAccount
    {
        [Required]
        public int id { get; set; }
        [Required]
        [MaxLength(30)]
        public string email { get; set; }
        [Required]
        [MaxLength(30)]
        public string password { get; set; }
        [Required]
        public int teach_in_school { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace SpeedyAPI.Models
{
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
    }
}

using System.ComponentModel.DataAnnotations;

namespace SpeedyAPI.Models
{
    public class Admin
    {
        [Required]
        [StringLength(255)]
        public string username { get; set; }
        [Required]
        [StringLength(255)]
        public string password { get; set; }
    }
}

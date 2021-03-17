using System;
using System.ComponentModel.DataAnnotations;

namespace SpeedyAPI.Models
{
    public class Key
    {
        public int id { get; set; }

        [Required]
        [MaxLength(100)]
        public string keyText { get; set; }
        [Required]
        public bool isUsed { get; set; }
        [RegularExpression(@"school_key|api_key", 
            ErrorMessage = "Must be school_key or api_key")]
        public string key_type { get; set; }

        [DataType(DataType.Date)]
        public DateTime create_date { get; set; }
        [DataType(DataType.Date)]


        public DateTime expiry_date { get; set; }
    }
}

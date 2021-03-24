using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpeedyAPI.Models
{
    public class Key : IValidatableObject
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

        public Key()
        {
            create_date = DateTime.Now;
            expiry_date = DateTime.Now.AddDays(10);
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (expiry_date < create_date)
            {
                results.Add(new ValidationResult("expired date cannot smaller than create date"));
            }

            return results;
        }
    }
}

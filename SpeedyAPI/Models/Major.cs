using System;

namespace SpeedyAPI.Models
{
    public class Major
    {
        public int id { get; set; }
        public int school_id { get; set; }

        public string name { get; set; }
        public DateTime startDate { get; set; }
    }
}

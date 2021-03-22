using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeedyAPI.Models
{
    public class StudentFaceVector : Student
    {
        public StudentFaceVector()
        {

        }
        public StudentFaceVector(Student student, float[] vector)
            : base(student)
        {
            this.vector = vector;
        }

        public float[] vector { get; set; }


      
    }
}

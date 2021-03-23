using System;
using System.Collections.Generic;

namespace SpeedyAPI.Models.WebcamAttendance
{
    public class StudentFaceVectors
    {
        public double minDistance { get; set; }
        public List<StudentFaceVector> studentFaceVectors { get; set; }

        public StudentFaceVector GetNearestVectorDistance(float[] currentFaceVector)
        {
            StudentFaceVector closest = studentFaceVectors[0];
            double minDistance = double.MaxValue;
            foreach (StudentFaceVector studentVec in studentFaceVectors)
            {
                double total = 0;
                for (int i = 0; i < 512; i++)
                {
                    total += Math.Pow(studentVec.vector[i] - currentFaceVector[i], 2);
                }

                total = Math.Sqrt(total);
                if (total < minDistance)
                {
                    minDistance = total;
                    closest = studentVec;
                }
            }

            this.minDistance = minDistance;
            return closest;
        }
    }
}

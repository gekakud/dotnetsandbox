using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullObjectPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            //get student by name from repo
            //pass to renderer
        }
    }

    interface IStudent
    {
        int Id { get; set; }
        string Name { get; }
        int NumOfCourses { get; set; }
        float Gpa { get; set; }
    }

    class Student:IStudent
    {
        public int Id { get; set; }
        public string Name { get; }
        public int NumOfCourses { get; set; }
        public float Gpa { get; set; }
    }

    class NullStudent:IStudent
    {
        public int Id { get; set; } = -1;
        public string Name { get; } = "";
        public int NumOfCourses { get; set; } = -1;
        public float Gpa { get; set; } = 0;
    }

    static class StudentRenderer
    {
        static void PrintStudentData(IStudent student)
        {
            if (student != null)
            {
                if (student.Name != null)
                {
                    //... print student data
                }
            }
        }


    }
}

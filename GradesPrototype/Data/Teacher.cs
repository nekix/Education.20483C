using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradesPrototype.Data
{
    public class Teacher : User
    {
        public int TeacherID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Class { get; set; }

        public Teacher()
        {
            TeacherID = 0;
            UserName = string.Empty;
            Password = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Class = string.Empty;
        }

        public Teacher(int teacherID, string userName, string password, string firstName, string lastName, string className)
        {
            TeacherID = teacherID;
            UserName = userName;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Class = className;
        }

        public void EnrollInClass(Student student)
        {
            if (student.TeacherID == 0)
            {
                student.TeacherID = TeacherID;
            }
            else
            {
                throw new ArgumentException(nameof(EnrollInClass), "Student is already assigned to a class");
            }
        }

        public void RemoveFromClass(Student student)
        {
            if(student.TeacherID == TeacherID)
            {
                student.TeacherID = 0;
            }
            else
            {
                throw new ArgumentException(nameof(RemoveFromClass), "Student is not part of the class");
            }
        }
    }
}

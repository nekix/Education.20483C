using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public override bool SetPassword(string password)
        {
            if(password.Length >= 8 && Regex.Match(password, @".*[0-9]+.*[0-9]+.*").Success)
            {
                _password = password;
                return true;
            }

            return false;
        }

        public void EnrollInClass(Student student)
        {
            if (student.TeacherID == 0)
            {
                student.TeacherID = TeacherID;
            }
            else
            {
                throw new ArgumentException("Student is already assigned to a class", nameof(EnrollInClass));
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
                throw new ArgumentException("Student is not part of the class", nameof(RemoveFromClass));
            }
        }
    }
}

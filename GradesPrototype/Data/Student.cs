using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GradesPrototype.Data
{
    public class Student : User, IComparable<Student>
    {
        public int StudentID { get; set; }
        public int TeacherID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Student()
        {
            StudentID = 0;
            UserName = string.Empty;
            TeacherID = 0;
            FirstName = string.Empty;
            LastName = string.Empty;
        }

        public Student(int studentID, string userName, string password, int teacherID, string firstName, string lastName)
        {
            StudentID = studentID;
            UserName = userName;
            Password = password;
            TeacherID = teacherID;
            FirstName = firstName;
            LastName = lastName;
        }

        public override bool SetPassword(string password)
        {
            if (password.Length >= 6)
            {
                return true;
            }

            return false;
        }

        public int CompareTo(Student other)
        {
            return string.Compare(LastName + FirstName, other.LastName + other.FirstName);
        }

        public void AddGrade(Grade grade)
        {
            if(grade.StudentID == 0)
            {
                grade.StudentID = StudentID;
            }
            else
            {
                throw new ArgumentException("Grade belongs to a different student", nameof(AddGrade));
            }
        }
    }
}

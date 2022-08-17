using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GradesPrototype.Data
{
    public class Student : IComparable<Student>
    {
        public int StudentID { get; set; }
        public string UserName { get; set; }
        private string _password = Guid.NewGuid().ToString();
        public string Password
        {
            set { _password = value; }
        }
        public int TeacherID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Student()
        {
            StudentID = 0;
            UserName = string.Empty;
            Password = string.Empty;
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

        public bool VerifyPassword(string Password)
        {
            return string.Compare(Password, _password, StringComparison.Ordinal) == 0;
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
                throw new ArgumentException(nameof(AddGrade), "Grade belongs to a different student");
            }
        }
    }
}

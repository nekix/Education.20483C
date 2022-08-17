using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradesPrototype.Data
{
    public class Teacher
    {
        public int TeacherID { get; set; }
        public string UserName { get; set; }
        private string _password = Guid.NewGuid().ToString();
        public string Password
        {
            set { _password = value; }
        }
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

        public bool VerifyPassword(string Password)
        {
            return string.Compare(Password, _password, StringComparison.Ordinal) == 0;
        }
    }
}

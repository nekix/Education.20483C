﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GradesPrototype.Data
{
    public class Student
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
            _password = string.Empty;
            TeacherID = 0;
            FirstName = string.Empty;
            LastName = string.Empty;
        }

        public Student(int studentID, string userName, string password, int teacherID, string firstName, string lastName)
        {
            StudentID = studentID;
            UserName = userName;
            _password = password;
            TeacherID = teacherID;
            FirstName = firstName;
            LastName = lastName;
        }

        public bool VerifyPassword(string Password)
        {
            return string.Compare(Password, _password, StringComparison.Ordinal) == 0;
        }
    }
}

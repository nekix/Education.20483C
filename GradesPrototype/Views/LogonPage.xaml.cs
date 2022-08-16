﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GradesPrototype.Data;
using GradesPrototype.Services;

namespace GradesPrototype.Views
{
    /// <summary>
    /// Interaction logic for LogonPage.xaml
    /// </summary>
    public partial class LogonPage : UserControl
    {
        public LogonPage()
        {
            InitializeComponent();
        }

        #region Event Members
        public event EventHandler LogonSuccess;
        public event EventHandler LogonFailed; 

        #endregion

        #region Logon Validation

        //Init logon process
        public void Logon_Click(object sender, RoutedEventArgs e)
        {
            //Check teacher account
            var teacher = (from Teacher t in DataSource.Teachers
                          where t.UserName == username.Text
                          where t.Password == password.Password
                          select t).FirstOrDefault();

            if (!(teacher is default(Teacher)))
            {
                SessionContext.UserID = teacher.TeacherID;
                SessionContext.UserRole = Role.Teacher;
                SessionContext.UserName = teacher.UserName;
                SessionContext.CurrentTeacher = teacher;

                LogonSuccess?.Invoke(this, null);
                return;
            }

            //Check student account
            var student = (from Student t in DataSource.Students
                           where t.UserName == username.Text
                           where t.Password == password.Password
                           select t).FirstOrDefault();

            if (!(student is default(Student)))
            {
                SessionContext.UserID = student.StudentID;
                SessionContext.UserRole = Role.Student;
                SessionContext.UserName = student.UserName;
                SessionContext.CurrentStudent = student;

                LogonSuccess?.Invoke(this, null);
                return;
            }

            //Raise logon failed event
            LogonFailed?.Invoke(this, null);
        }

        #endregion
    }
}

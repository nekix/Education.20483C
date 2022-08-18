using System;
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
using Grades.DataModel;
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
        private void Logon_Click(object sender, RoutedEventArgs e)
        {
            //Check teacher account
            var teacher = (from Teacher t in SessionContext.DBContext.Teachers
                          where t.User.UserName == username.Text
                          where t.User.UserPassword == password.Password
                          select t).FirstOrDefault();

            //Set teacher to session context
            if (!(teacher is default(Teacher)) && !string.IsNullOrEmpty(teacher.User.UserName))
            {
                SessionContext.UserID = teacher.UserId;
                SessionContext.UserRole = Role.Teacher;
                SessionContext.UserName = teacher.User.UserName;
                SessionContext.CurrentTeacher = teacher;

                LogonSuccess?.Invoke(this, null);
                return;
            }

            //Check student account
            var student = (from Student s in SessionContext.DBContext.Students
                           where s.User.UserName == username.Text
                           where s.User.UserPassword == password.Password
                           select s).FirstOrDefault();

            //Set student to session context
            if (!(student is default(Student)) && !string.IsNullOrEmpty(teacher.User.UserName))
            {
                SessionContext.UserID = student.User.UserId;
                SessionContext.UserRole = Role.Student;
                SessionContext.UserName = student.User.UserName;
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

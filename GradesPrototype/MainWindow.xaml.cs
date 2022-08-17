using System;
using System.Collections;
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
using GradesPrototype.Views;

namespace GradesPrototype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataSource.CreateData();
            DataSource.Students.Sort();
            GotoLogon();
        }

        #region Navigation
        // Display the logon view and and hide the list of students and single student view
        public void GotoLogon()
        {
            logonPage.Visibility = Visibility.Visible;
            studentsPage.Visibility = Visibility.Collapsed;
            studentProfile.Visibility = Visibility.Collapsed;
        }

        // Display the list of students
        private void GotoStudentsPage()
        {
            studentProfile.Visibility = Visibility.Collapsed;
            studentsPage.Visibility = Visibility.Visible;
            studentsPage.Refresh();
        }

        // Display the details for a single student
        public void GotoStudentProfile()
        {
            studentsPage.Visibility = Visibility.Collapsed;
            studentProfile.Visibility = Visibility.Visible;
            studentProfile.Refresh();
        }
        #endregion

        #region Event Handlerson

        // Handle successful logon
        private void Logon_Success(object sender, EventArgs e)
        {
            // Update the display and show the data for the logged on user
            logonPage.Visibility = Visibility.Collapsed;
            gridLoggedIn.Visibility = Visibility.Visible;

            Refresh();
        }

        // Handle logoff
        private void Logoff_Click(object sender, RoutedEventArgs e)
        {
            // Hide all views apart from the logon page
            gridLoggedIn.Visibility = Visibility.Collapsed;
            studentsPage.Visibility = Visibility.Collapsed;
            studentProfile.Visibility = Visibility.Collapsed;
            logonPage.Visibility = Visibility.Visible;
        }

        //Handke failed logon 
        private void Logon_Failed(object sender, EventArgs e)
        {
            MessageBox.Show("Invalid Username or Password", "Logon Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Handle the Back button on the Student page
        private void studentPage_Back(object sender, EventArgs e)
        {
            GotoStudentsPage();
        }

        // Handle the StudentSelected event when the user clicks a student on the Students page
        // Set the global context to the name of the student and call the GotoStudentProfile method to display the details of the student
        private void studentsPage_StudentSelected(object sender, StudentEventArgs e)
        {
            SessionContext.CurrentStudent = e.Child;
            GotoStudentProfile();
        }
        #endregion

        #region Display Logic

        // Update the display for the logged on user (student or teacher)
        private void Refresh()
        {
            if (SessionContext.UserRole is Role.Student)
            {
                txtName.Text = $"Welcome {SessionContext.CurrentStudent.FirstName} {SessionContext.CurrentStudent.LastName}";
                GotoStudentProfile();
            }
            else
            {
                txtName.Text = $"Welcome {SessionContext.CurrentTeacher.FirstName} {SessionContext.CurrentTeacher.LastName}";
                GotoStudentsPage();
            }
        }
        #endregion
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
using GradesPrototype.Controls;
using GradesPrototype.Services;
using static System.Net.WebRequestMethods;

namespace GradesPrototype.Views
{
    /// <summary>
    /// Interaction logic for StudentsPage.xaml
    /// </summary>
    public partial class StudentsPage : UserControl
    {
        public StudentsPage()
        {
            InitializeComponent();
        }

        #region Display Logic

        // Display students for the current teacher
        public void Refresh()
        {
            list.Items.Clear();


            foreach (Student student in SessionContext.DBContext.Students)
            {
                if (student.TeacherUserId == SessionContext.CurrentTeacher.UserId)
                {
                    SessionContext.DBContext.LoadProperty(student, "User");
                    SessionContext.DBContext.LoadProperty(student, "Grades");

                    list.Items.Add(student);
                }
            }

            txtClass.Text = $"Class {SessionContext.CurrentTeacher.Class}";
        }
        #endregion

        #region Event Members
        public delegate void StudentSelectionHandler(object sender, StudentEventArgs e);
        public event StudentSelectionHandler StudentSelected;
        #endregion

        #region Event Handlers
        // Handle the click event for a student
        // Raise the StudentSelected event and indicate which student was selected
        // The MainWindow window subscribes to this event and displays the view for a single student
        private void Student_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                StudentSelected?.Invoke(this, new StudentEventArgs((Student)button.DataContext));
            }
        }

        // Create a new student with input from the user
        private void NewStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Use the StudentDialog to get the details of the student from the user
                StudentDialog sd = new StudentDialog();

                // Display the form and get the details of the new student
                if (sd.ShowDialog().Value)
                {
                    // When the user closes the form, retrieve the details of the student from the form
                    // and use them to create a new Student object
                    Student newStudent = new Student();
                    newStudent.FirstName = sd.firstName.Text;
                    newStudent.LastName = sd.lastName.Text;

                    //Checking the password for compliance with the requirements
                    if (!newStudent.User.SetPassword(Role.Student, sd.password.Text))
                    {
                        throw new Exception("Password must be at least 6 characters long. Student not created");
                    }

                    // Generate the UserName property - lastname with the initial letter of the first name all converted to lowercase
                    newStudent.User.UserName = (newStudent.LastName + newStudent.FirstName.Substring(0, 1)).ToLower();

                    // Generate a unique ID for the user
                    newStudent.UserId = Guid.NewGuid();


                    // Assign a value for the ImageName field
                    newStudent.ImageName = "No photo";

                    // Generate default values for remaining properties of user object
                    newStudent.User.ApplicationId = (from Grades.DataModel.User u in SessionContext.DBContext.Users select u.ApplicationId).FirstOrDefault();
                    newStudent.User.IsAnonymous = false;
                    newStudent.User.LastActivityDate = DateTime.Now;
                    newStudent.User.UserId = newStudent.UserId;

                    // Add the student to the Students collection
                    SessionContext.DBContext.AddToStudents(newStudent);
                    SessionContext.Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error creating new student", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Enroll a student in the teacher's class
        private void EnrollStudent_Click(object sender, RoutedEventArgs e)
        {
            AssignStudentDialog dialog = new AssignStudentDialog();
            dialog.ShowDialog();

            Refresh();
        }
        #endregion
    }

    // EventArgs class for passing Student information to an event
    public class StudentEventArgs : EventArgs
    {
        public Student Child { get; set; }

        public StudentEventArgs(Student s)
        {
            Child = s;
        }
    }

    public class ImageNameConverter : IValueConverter
    {
        const string webFolder = @"http://localhost:1650/Images/Portraits/";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                return $"{webFolder}{str}";
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

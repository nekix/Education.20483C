using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for StudentProfile.xaml
    /// </summary>
    public partial class StudentProfile : UserControl
    {
        public StudentProfile()
        {
            InitializeComponent();
        }

        #region Event Members
        public event EventHandler Back;
        #endregion

        #region Events
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // If the user is not a teacher, do nothing
            if (SessionContext.UserRole != Role.Teacher)
            {
                return;
            }

            // If the user is a teacher, raise the Back event
            // The MainWindow page has a handler that catches this event and returns to the Students page
            Back?.Invoke(sender, e);
        }

        // Enable a teacher to remove a student from a class
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (SessionContext.UserRole != Role.Teacher)
            {
                return;
            }

            try
            {
                if (MessageBox.Show($"Remove {SessionContext.CurrentStudent.FirstName} {SessionContext.CurrentStudent.LastName}",
                    "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    SessionContext.CurrentTeacher.RemoveFromClass(SessionContext.CurrentStudent);

                    Back?.Invoke(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error remove a student from a class", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // TODO: Exercise 4: Task 5a: Enable a teacher to add a grade to a student
        private void AddGrade_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        // Display the details for the current student including the grades for the student
        public void Refresh()
        {
            studentName.DataContext = SessionContext.CurrentStudent;

            //Hide button "Back" for student
            if (SessionContext.UserRole is Role.Student)
            {
                btnBack.Visibility = Visibility.Hidden;
            }
            else
            {
                btnBack.Visibility = Visibility.Visible;
            }

            // Find and display all the grades for the student in the studentGrades ItemsControl
            ArrayList grades = new ArrayList ((from Grade g in DataSource.Grades
                                               where g.StudentID == SessionContext.CurrentStudent.StudentID
                                               select g).ToList());

            studentGrades.ItemsSource = grades;
        }
    }
}

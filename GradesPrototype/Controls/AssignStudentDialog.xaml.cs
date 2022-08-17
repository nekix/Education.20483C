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
using System.Windows.Shapes;
using GradesPrototype.Data;
using GradesPrototype.Services;

namespace GradesPrototype.Controls
{
    /// <summary>
    /// Interaction logic for AssignStudentDialog.xaml
    /// </summary>
    public partial class AssignStudentDialog : Window
    {
        public AssignStudentDialog()
        {
            InitializeComponent();
        }

        // Refresh the display of unassigned students
        private void Refresh()
        {
            var unassignedStudents = from s in DataSource.Students
                                          where s.TeacherID == 0
                                          select s;

            if(unassignedStudents.Count() == 0)
            {
                txtMessage.Visibility = Visibility.Visible;
                list.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtMessage.Visibility = Visibility.Collapsed;
                list.Visibility = Visibility.Collapsed;

                list.ItemsSource = unassignedStudents;
            }
        }

        private void AssignStudentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        // Enroll a student in the teacher's class
        private void Student_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;

                int studentID = (int)button.Tag;

                var student = (from s in DataSource.Students
                              where s.StudentID == studentID
                              select s).FirstOrDefault();

                string message = string.Format($"Add {student.FirstName} {student.LastName} to your class?");
                if(MessageBox.Show(message, "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    SessionContext.CurrentTeacher.EnrollInClass(student);
                    Refresh();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Error enrolling student", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            // Close the dialog box
            this.Close();
        }
    }
}

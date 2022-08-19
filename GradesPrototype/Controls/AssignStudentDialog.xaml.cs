using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using Grades.DataModel;
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
            SessionContext.DBContext.Students.Load();
            var unassignedStudents = from s in SessionContext.DBContext.Students.Expand("User, Grades")
                                          where s.TeacherUserId == null
                                          select s;

            if(unassignedStudents.Count() == 0)
            {
                txtMessage.Visibility = Visibility.Visible;
                list.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtMessage.Visibility = Visibility.Collapsed;
                list.Visibility = Visibility.Visible;

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

                Guid studentID = (Guid)button.Tag;

                var student = (from s in SessionContext.DBContext.Students
                              where s.UserId == studentID
                              select s).First();

                if(MessageBox.Show($"Add {student.FirstName} {student.LastName} to your class?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Guid teacherId = SessionContext.CurrentTeacher.UserId;

                    SessionContext.CurrentTeacher.EnrollInClass(student);
                    SessionContext.DBContext.UpdateObject(student);
                    SessionContext.Save();

                    Refresh();
                }
            }
            catch (ClassFullException exp)
            {
                MessageBox.Show(string.Format($"{exp.Message}. Class: {exp.ClassName}"), "Error enrolling student", MessageBoxButton.OK, MessageBoxImage.Error);
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

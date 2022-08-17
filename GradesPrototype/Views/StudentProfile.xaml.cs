using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GradesPrototype.Controls;
using GradesPrototype.Data;
using GradesPrototype.Services;
using Microsoft.Win32;
using Newtonsoft.Json;

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

        // Enable a teacher to add a grade to a student
        private void AddGrade_Click(object sender, RoutedEventArgs e)
        {
            if(SessionContext.UserRole != Role.Teacher)
            {
                return;
            }

            try
            {
                GradeDialog dialog = new GradeDialog();
                if (dialog.ShowDialog().Value)
                {
                    Grade grade = new Grade(
                        0, dialog.assessmentDate.SelectedDate.Value.ToString("d"),
                        dialog.subject.SelectedValue.ToString(),
                        dialog.assessmentGrade.Text,
                        dialog.comments.Text);

                    DataSource.Grades.Add(grade);

                    SessionContext.CurrentStudent.AddGrade(grade);

                    Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "User if an exception occurs", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //  Generate the grades report for the currently selected student
        private void SaveReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "JSON documents|*.json";
                dialog.FileName = "Grades";
                dialog.DefaultExt = ".json";

                // Show file dialog for get file path to save report
                bool? result = dialog.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    // Get the grades for the currently selected student.
                    List<Grade> grades = (from g in DataSource.Grades
                                          where g.StudentID == SessionContext.CurrentStudent.StudentID
                                          select g).ToList();

                    // Show JSON report and waiting confirm
                    string gradesAsJson = JsonConvert.SerializeObject(grades, Formatting.Indented);
                    MessageBoxResult reply = MessageBox.Show(gradesAsJson, "Save Report?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    // Save report to file
                    if(reply == MessageBoxResult.Yes)
                    {
                        using (StreamWriter sw = new StreamWriter(dialog.OpenFile()))
                        {
                            sw.Write(gradesAsJson);
                            //sw.BaseStream.Position = 0; 
                        }
                    }
                }       
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Generating Report", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadReport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "JSON documents|*.json";

            bool? result = dialog.ShowDialog();

        }
        #endregion

        // Display the details for the current student including the grades for the student
        public void Refresh()
        {
            studentName.DataContext = SessionContext.CurrentStudent;

            // If the current user is a student, hide the Back, Remove, and Add Grade buttons
            // (these features are only applicable to teachers)
            if (SessionContext.UserRole == Role.Student)
            {
                btnBack.Visibility = Visibility.Hidden;
                btnRemove.Visibility = Visibility.Hidden;
                btnAddGrade.Visibility = Visibility.Hidden;
            }
            else
            {
                btnBack.Visibility = Visibility.Visible;
                btnRemove.Visibility = Visibility.Visible;
                btnAddGrade.Visibility = Visibility.Visible;
            }

            // Find and display all the grades for the student in the studentGrades ItemsControl
            List<Grade> grades = new List<Grade> ((from Grade g in DataSource.Grades
                                               where g.StudentID == SessionContext.CurrentStudent.StudentID
                                               select g).ToList());

            studentGrades.ItemsSource = grades;
        }
    }
}

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
using System.Xml;
using Grades.DataModel;
using GradesPrototype.Controls;
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
                    SessionContext.Save();

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
                //GradeDialog dialog = new GradeDialog();
                //if (dialog.ShowDialog().Value)
                //{
                //    Grade grade = new Grade(
                //        0, dialog.assessmentDate.SelectedDate.Value.ToString("d"),
                //        dialog.subject.SelectedValue.ToString(),
                //        dialog.assessmentGrade.Text,
                //        dialog.comments.Text);

                //    DataSource.Grades.Add(grade);

                //    SessionContext.CurrentStudent.AddGrade(grade);

                //    Refresh();
                //}
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
                dialog.Filter = "XML documents|*.xml";

                // Set the default filename to Grades.txt
                dialog.FileName = "Grades";
                dialog.DefaultExt = ".xml";

                // Show file dialog for get file path to save report
                bool? result = dialog.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    // Get the grades for the currently selected student.
                    IEnumerable<Grade> grades = (from g in SessionContext.DBContext.Grades
                                          where g.StudentUserId == SessionContext.CurrentStudent.UserId
                                          select g);

                    // Serialize the grades to a MemoryStream. 
                    MemoryStream ms = FormatAsXMLStream(grades);

                    // Preview the report data in a MessageBox and ask the user whether they wish to save the report.
                    string formattedReportData = FormatXMLData(ms);
                    MessageBoxResult reply = MessageBox.Show(formattedReportData, "Save Report?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (reply == MessageBoxResult.Yes)
                    {
                        // If the user says yes, then save the data to the file that the user specified earlier
                        // If the file already exists it will be overwritten (the SaveFileDialog box will already have asked the user whether this is OK)
                        FileStream file = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write);
                        ms.CopyTo(file);
                        file.Close();
                    }
                }       
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Generating Report", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Utility and Helper Methods

        // Format the list of grades as an XML document and write it to a MemoryStream
        private MemoryStream FormatAsXMLStream(IEnumerable<Grades.DataModel.Grade> grades)
        {
            // Save the XML document to a MemoryStream by using an XmlWriter
            MemoryStream ms = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(ms);

            // The document root has the format <Grades Student="Eric Gruber">
            writer.WriteStartDocument();
            writer.WriteStartElement("Grades");
            writer.WriteAttributeString("Student", String.Format("{0} {1}", SessionContext.CurrentStudent.FirstName, SessionContext.CurrentStudent.LastName));

            // Format the grades for the student and add them as child elements of the root node
            // Grade elements have the format <Grade Date="01/01/2012" Subject="Math" Assessment="A-" Comments="Good" />
            foreach (Grades.DataModel.Grade grade in grades)
            {
                writer.WriteStartElement("Grade");
                writer.WriteAttributeString("Date", grade.AssessmentDate.ToString());
                writer.WriteAttributeString("Subject", grade.Subject.Name);
                writer.WriteAttributeString("Assessment", grade.Assessment);
                writer.WriteAttributeString("Comments", grade.Comments);
                writer.WriteEndElement();
            }

            // Finish the XML document with the appropriate end elements
            writer.WriteEndElement();
            writer.WriteEndDocument();

            // Flush the XmlWriter and close it to ensure that all the data is written to the MemoryStream
            writer.Flush();
            writer.Close();

            // The MemoryStream now contains the formatted data
            // Reset the MemoryStream so it can be read from the start and then return it
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        // Format the XML data in the stream as a neatly constructed string
        private string FormatXMLData(Stream stream)
        {
            // Use a StringBuilder to construct the string
            StringBuilder builder = new StringBuilder();

            // Use an XmlTextReader to read the XML data from the stream
            XmlTextReader reader = new XmlTextReader(stream);

            // Read and process the XML data a node at a time
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.XmlDeclaration:
                        // The node is an XML declaration such as <?xml version='1.0'>
                        builder.Append(String.Format("<?{0} {1}>\n", reader.Name, reader.Value));
                        break;

                    case XmlNodeType.Element:
                        // The node is an element (enclosed between '<' and '/>')
                        builder.Append(String.Format("<{0}", reader.Name));
                        if (reader.HasAttributes)
                        {
                            // Output each of the attributes of the element in the form "name='value'"
                            while (reader.MoveToNextAttribute())
                            {
                                builder.Append(String.Format(" {0}='{1}'", reader.Name, reader.Value));
                            }
                        }
                        builder.Append(">\n");
                        break;

                    case XmlNodeType.EndElement:
                        // The node is the closing tag at the end of an element
                        builder.Append(String.Format("</{0}>", reader.Name));
                        break;

                }
            }

            // Reset the stream
            stream.Seek(0, SeekOrigin.Begin);

            // Return the string containing the formatted data
            return builder.ToString();
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
            List<Grade> grades = new List<Grade>((from Grade g in SessionContext.DBContext.Grades
                                                  where g.StudentUserId == SessionContext.CurrentStudent.UserId
                                                  select g).ToList());

            studentGrades.ItemsSource = grades;
        }
    }

    // Value converter that converts the integer subject id into the string subject name, for display purposes
    [ValueConversion(typeof(string), typeof(int))]
    class SubjectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              System.Globalization.CultureInfo culture)
        {
            int subjectId = (int)value;
            var subject = SessionContext.DBContext.Subjects.FirstOrDefault(s => s.Id == subjectId);

            return subject.Name != string.Empty ? subject.Name : "N/A";
        }

        #region Predefined code

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

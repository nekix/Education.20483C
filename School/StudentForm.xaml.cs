using System;
using System.Windows;

namespace School
{
    /// <summary>
    /// Interaction logic for StudentForm.xaml
    /// </summary>
    public partial class StudentForm : Window
    {
        #region Predefined code

        public StudentForm()
        {
            InitializeComponent();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            // Validate first name
            if (string.IsNullOrEmpty(firstName.Text))
            {
                MessageBox.Show($"The student must have a first name", "Error");
                return;
            }

            // Validate last name
            if (string.IsNullOrEmpty(lastName.Text))
            {
                MessageBox.Show($"The student must have a last name", "Error");
                return;
            }

            // Validate date of birth
            if (!DateTime.TryParse(dateOfBirth.Text, out DateTime result))
            {
                MessageBox.Show($"The date of birth must be a valid date", "Error");
                return;
            }

            // Validate age
            if(DateTime.Now.Subtract(result).Days / 365.25 < 5)
            {
                MessageBox.Show($"The student must at least 5 years old", "Error");
                return;
            }

            this.DialogResult = true;
        }

        #endregion
    }
}

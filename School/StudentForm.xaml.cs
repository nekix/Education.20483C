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
            //Validate first name
            if (string.IsNullOrEmpty(firstName.Text))
            {
                MessageBox.Show($"The student must have a first name", "Error");
                return;
            }

            //Validate last name
            if (string.IsNullOrEmpty(lastName.Text))
            {
                MessageBox.Show($"The student must have a last name", "Error");
                return;
            }


            this.DialogResult = true;
        }

        #endregion
    }
}

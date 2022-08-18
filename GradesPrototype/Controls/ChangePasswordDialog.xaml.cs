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
using Grades.DataModel;
using GradesPrototype.Services;

namespace GradesPrototype.Controls
{
    /// <summary>
    /// Interaction logic for ChangePasswordDialog.xaml
    /// </summary>
    public partial class ChangePasswordDialog : Window
    {
        public ChangePasswordDialog()
        {
            InitializeComponent();
        }

        // If the user clicks OK to change the password, validate the information that the user has provided
        private void ok_Click(object sender, RoutedEventArgs e)
        {
            User user;

            // Get the details of the current user
            if (SessionContext.UserRole == Role.Student)
            {
                user = SessionContext.CurrentStudent.User;
            }
            else
            {
                user = SessionContext.CurrentTeacher.User;
            }

            // Check that the old password is correct for the current user
            if (!user.VerifyPassword(oldPassword.Password))
            {
                MessageBox.Show("Old password is incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Check that the new password and confirm password fields are the same
            if (string.Compare(newPassword.Password, confirm.Password) != 0)
            {
                MessageBox.Show("The new password and confirm password fields are different", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Attempt to change the password
            // If the password is not sufficiently complex, display an error message
            if (!user.SetPassword(SessionContext.UserRole, newPassword.Password))
            {
                MessageBox.Show("The new password is not sufficiently complex", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SessionContext.Save();
            
            // Indicate that the data is valid
            this.DialogResult = true;
        }
    }
}

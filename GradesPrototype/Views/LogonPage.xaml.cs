﻿using System;
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

namespace GradesPrototype.Views
{
    /// <summary>
    /// Interaction logic for LogonPage.xaml
    /// </summary>
    public partial class LogonPage : UserControl
    {
        public LogonPage()
        {
            InitializeComponent();
        }

        #region Event Members
        public event EventHandler LogonSuccess;

        #endregion

        #region Logon Validation

        //Init logon process
        public void Logon_Click(object sender, RoutedEventArgs e)
        {
            SessionContext.UserName = username.Text;

            //Set dummy current student
            if ((SessionContext.UserRole = (bool)userrole.IsChecked ? Role.Teacher : Role.Student) == Role.Student)
            {
                SessionContext.CurrentStudent = "Eric Gruber";
            }

            LogonSuccess?.Invoke(this, null);
        }

        #endregion
    }
}

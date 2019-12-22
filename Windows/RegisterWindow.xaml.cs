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
using System.Windows.Shapes;
using Client.Windows;

namespace Client.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void BackToLogin(object sender, RoutedEventArgs e)
        {
            LoginWindow window = new LoginWindow();
            UiControl.ChangeWindow(this, window);

        }

        private void Register(object sender, RoutedEventArgs e)
        {
            // registration logic

            LoginWindow window = new LoginWindow();
            UiControl.ChangeWindow(this, window);

        }
    }
}

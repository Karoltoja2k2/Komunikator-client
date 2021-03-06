﻿using Client.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace Client.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy LogPage1.xaml
    /// </summary>
    public partial class LogPage1 : Page
    {
        private static Window parentWin;
        public LogPage2 page2;
        private Socket socket;

        private User profile;
        private Serializer serializer = new Serializer();
        private const int BUFFER_SIZE = 1048576;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];


        public LogPage1(Window parent, Socket socketParam)
        {
            this.socket = socketParam;
            page2 = new LogPage2(parent, socket);
            parentWin = parent;
            InitializeComponent();
        }


        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        public void serverError()
        {
            LoginWindow loginWindow = new LoginWindow();
            UiControl.ChangeWindow(parentWin, loginWindow);
            return;
        }


        private void Login(object sender, RoutedEventArgs e)
        {
            loginButton.IsEnabled = false;
            // login validation
            // to be done

            int accNumber;
            string stringAccNumber = numberInput.Text;
            string password = passwordInput.Password;
            if (String.IsNullOrEmpty(stringAccNumber))
            {
                accNumber = 12364404;
                password = "lolek123123";
            }
            else
            {
                accNumber = Int32.Parse(stringAccNumber);
            }

            alertText.Text = "Logowanie";
            Order loginOrder = new Order(3, accNumber, password);
            byte[] sendbuff = serializer.Serialize_Obj(loginOrder);
            try
            {
                socket.Send(sendbuff, sendbuff.Length, 0);
            }
            catch (SocketException) { serverError(); }

            socket.BeginReceive(buffer, 0, buffer.Length, 0, receiveCallBack, socket);

             
           
        }

        private void receiveCallBack(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            int received;
            try
            {
                received = socket.EndReceive(ar);
            }
            catch (SocketException) { Dispatcher.Invoke(new Action(() => serverError())); return; }
            catch (ObjectDisposedException) { Dispatcher.Invoke(new Action(() => serverError())); return; }


            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            Order loginVerify = (Order)serializer.Deserialize_Obj(recBuf, new Order());

            if (loginVerify.succes == true)
            {
                profile = loginVerify.acc;
                profile.friendList = loginVerify.friendList;
                profile.pendingOrders = loginVerify.pendingOrders;
                
                foreach(int friend in profile.friendList)
                {
                    profile.conversations.Add(new Conversation(profile.accNumber, friend));
                }

                int convToFind;
                Conversation foundConv;
                foreach (Order msg in loginVerify.messages)
                {
                    convToFind = msg.sender != profile.accNumber ? msg.sender : msg.receiver;
                    foundConv = profile.conversations.Find(conv => conv.receiver == convToFind);
                    if (foundConv != null)
                        foundConv.messages.Add(msg);
                }



                Dispatcher.Invoke(new Action(() => manageResponse(true)));
            }
            else
            {
                Dispatcher.Invoke(new Action(() => manageResponse(false, loginVerify.helpMsg)));
                return;
            }

            
        }

        private void manageResponse(bool success, string msg=null)
        {
            if (success)
            {
                numberInput.BorderBrush = Brushes.Green;
                passwordInput.BorderBrush = Brushes.Green;

                MainWindow window = new MainWindow(socket, profile);
                UiControl.ChangeWindow(parentWin, window);
            }
            else
            {
                loginButton.IsEnabled = true;
                numberInput.BorderBrush = Brushes.Red;
                passwordInput.BorderBrush = Brushes.Red;
                alertText.Text = msg;
            }
        }

        private void RegistrationPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(page2);
        }
    }
}

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

namespace Client.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private byte[] buffer;
        private Order msgObj;


        public ChatWindow()
        {
            InitializeComponent();
        }

        private void Send_Msg(object sender, RoutedEventArgs e)
        {
            if (MainWindow.socket != null)
            {
                string msg = messageInput.Text;
                if (!String.IsNullOrEmpty(msg))
                {
                    msgObj = new Order(MainWindow.userAcc.token, 125125, 1251251, msg, DateTime.Now);
                    Serializer serializer = new Serializer();
                    byte[] buffer = serializer.Serialize_Obj(msgObj);

                    MainWindow.socket.Send(buffer, 0, buffer.Length, 0);
                    messageInput.Text = "";
                }
            }
        }
    }
}

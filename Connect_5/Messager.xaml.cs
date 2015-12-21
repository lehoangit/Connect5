using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System;
using System.Threading;
using Quobject.SocketIoClientDotNet.Client;

namespace Connect_5
{
    /// <summary>
    /// Interaction logic for Messager.xaml
    /// </summary>
    public partial class Messager : UserControl
    {
        public string NicknameA { get; set; }
        public string NicknameB { get; set; }
        public string ChatA { get; set; }
        public string ChatB { get; set; }
        public string Server { get; set; }
        public string TempServer { get; set; }
        public string TempChatB { get; set; }
        public string TempChatA { get; set; }
        public DispatcherTimer timer;

        // socketIO

        public Socket socket { get; set; }

        public Messager()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(1);
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            cTime time = new cTime();// show messasger
            if (TempServer != Server)
            {
                txtChats.Text += time.GetTimeCurrent() + " " + "Server" + ": " + Server + "\n";
                TempServer = Server;
            }
            if (TempChatB != ChatB)
            {
                txtChats.Text += time.GetTimeCurrent() + " " + NicknameB + ": " + ChatB + "\n";
                TempChatB = ChatB;
            }
            if (TempChatA != ChatA && Server == ChatA)
            {
                txtChats.Text += time.GetTimeCurrent() + " " + NicknameA + ": " + ChatA + "\n";
                TempChatA = ChatA;
            }
        }
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (NicknameA == null)
            {
                MessageBox.Show("Trò chơi chưa bắt đầu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            else if (txtContent.Text == "")
            {
                MessageBox.Show("Nhập nội dung chat", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            else
            {
                // show messasger
                cTime time = new cTime();
                ChatA = txtContent.Text;
                ConnectSocket.SendMessage(socket, NicknameA, ChatA);
                //if (Server == ChatA)
                //    txtChats.Text += time.GetTimeCurrent() + " " + NicknameA + ": " + ChatA + "\n";
            }
            txtContent.Clear();
        }
    }
}

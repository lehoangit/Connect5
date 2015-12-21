using Connect_5.Properties;
using Quobject.SocketIoClientDotNet.Client;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Connect_5
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// Khai báo lũ biến bệnh hoạn
        /// </summary>
        #region KhaiBaoBien
        private int _rows = Settings.Default._rows;
        private int _columns = Settings.Default._columns;

        private int _doday = Settings.Default._length;

        public string NicknameA = null, NicknameB = "Computer";

        // bàn cờ huyền thoại , làm mãi méo xong
        cBanCo banco;
        Socket socket;

        Semaphore pro = new Semaphore(0, 1);

        #endregion


        #region Sự kiện mấy cái nút trên Form
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            askClose();
        }
        private void wpfConnect_5_Closing(object sender, CancelEventArgs e)
        {

            askClose();
        }
        private void askClose()
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Thoát trò chơi", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.OK:
                    Application.Current.Shutdown();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            wpfConnect_5_Loaded(sender, e);
            if (radHumanComputer.IsChecked == true) // người chơi với máy
            {
                banco.Option.WhoPlayWith = Player.Com;
                mgChat.IsEnabled = false;
                banco.DiNgauNhien();
            }
            if (radTwoHuman.IsChecked == true) // 2 người chơi với nhau
            {
                banco.Option.WhoPlayWith = Player.Human;
                mgChat.IsEnabled = false;
            }
            if (radHumanOnline.IsChecked == true) // người chơi online
            {
                banco.Option.WhoPlayWith = Player.HumanOnline;
                mgChat.NicknameA = NicknameA;
                mgChat.NicknameB = NicknameB;
                mgChat.IsEnabled = true;

                // xử lý kết nối
                
                socket = IO.Socket(Settings.Default._connectString);
                mgChat.socket = socket;
                cBanCo.socket = socket;

                ConnectSocket.Connect(socket,NicknameA, mgChat);

                
            }


            if (radComputerOnline.IsChecked == true) // máy chơi online
            {
                mgChat.IsEnabled = true;
                banco.Option.WhoPlayWith = Player.ComputerOnline;
            }
            canvasBoard.IsEnabled = true;
        }
        #endregion

        /// <summary>
        /// Load Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wpfConnect_5_Loaded(object sender, RoutedEventArgs e)
        {
            //socket = IO.Socket(Settings.Default._connectString);

            ////////////////////////////////////
            NicknameA = txtNickname.Text;
            banco = null;
            banco = new cBanCo(this, canvasBoard);

            _drawingBoard();
        }

        // vẽ bàn cờ ban đầu
        private void _drawingBoard()
        {
            Rectangle rec = new Rectangle();
            rec.Stroke = new SolidColorBrush(Colors.Blue);
            rec.Fill = new SolidColorBrush(Colors.Aqua);
            rec.StrokeThickness = 1;
            rec.Height = canvasBoard.ActualHeight; rec.Width = canvasBoard.ActualWidth;
            canvasBoard.Children.Add(rec);

            for (int i = 0; i <= _rows; i++)
            {
                Line line = new Line();
                line.Stroke = new SolidColorBrush(Colors.Blue);
                line.StrokeThickness = 3;

                line.X1 = 0;
                line.Y1 = i * _doday;
                line.X2 = _doday * _columns;
                line.Y2 = i * _doday;
                canvasBoard.Children.Add(line);
            }
            for (int i = 0; i <= _columns; i++)
            {
                Line line = new Line();
                line.Stroke = new SolidColorBrush(Colors.Blue);
                line.StrokeThickness = 3;

                line.X1 = i * _doday;
                line.Y1 = 0;
                line.X2 = i * _doday;
                line.Y2 = _doday * _columns;
                canvasBoard.Children.Add(line);
            }
        }

        private void lblProgress_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            lblProgress.Content = banco.progress + "%";
        }
    }
}

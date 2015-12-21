using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;
using Connect_5.Properties;
using System.Windows.Input;

using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;


using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;

namespace Connect_5
{
    enum Player
    {
        None,
        Human,
        Com,
        HumanOnline,
        ComputerOnline
    }
    struct Node
    {
        public int Row;
        public int Column;
        public Node(int rw, int cl)
        {
            Row = rw;
            Column = cl;
        }
    }

    class cBanCo
    {
        // tọa độ
        public static int rows = -1, columns = -1;
        public static Socket socket;

        private DispatcherTimer timer;

        //Các biến chính
        private int row = Settings.Default._rows;
        private int column = Settings.Default._columns; //Số hàng, cột
        private int length = Settings.Default._length;//Độ dài mỗi ô
        public static Player currPlayer; //lượt đi
        public Player[,] board; //mảng lưu vị trí các con cờ
        private Player end; //biến kiểm tra trò chơi kết thúc
        private MainWindow frmParent; //Form thực hiện
        private Canvas canvasBanCo; // Nơi vẽ bàn cờ
        private cLuongGiaBanCo eBoard; //Bảng lượng giá bàn cờ
        private c5OWin OWin; // Kiểm tra 5 ô win

        public cOption Option; // Tùy chọn trò chơi
                               //Các biến phụ

        private Image coAo1; // cờ ảo cho người chơi thứ 1
        private Image coAo2; // cờ ảo cho người chơi thứ 2

        // Điểm lượng giá
        public int[] PhongThu = new int[5] { 0, 1, 9, 85, 769 };
        public int[] TanCong = new int[5] { 0, 2, 28, 256, 2308 };

        // đa tiến trình
        public BackgroundWorker bw;
        public string progress;


        //Properties
        public Player End
        {
            get { return end; }
            set { end = value; }
        }


        public int Row
        {
            get { return row; }
        }
        public int Column
        {
            get { return column; }
        }


        //Contructors
        public cBanCo(MainWindow frm, Canvas can)
        {
            Option = new cOption();
            OWin = new c5OWin();
            row = column = Settings.Default._columns;
            currPlayer = Player.Com;
            end = Player.None;
            frmParent = frm;
            canvasBanCo = can;
            board = new Player[row, column];
            eBoard = new cLuongGiaBanCo(this);


            coAo1 = new Image();
            coAo2 = new Image();
            coAo1.Source = new BitmapImage(new Uri("pack://application:,,,/Images/x.png"));
            coAo2.Source = new BitmapImage(new Uri("pack://application:,,,/Images/o.png"));
            CreateCoAo();

            canvasBanCo.MouseDown += new MouseButtonEventHandler(canvasBanCo_MouseDown);


            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(1);
            timer.Start();
            //// đa tiến trình
            //progress = "0";

            //bw = new BackgroundWorker();
            //bw.WorkerReportsProgress = true;
            //bw.WorkerSupportsCancellation = true;
            //bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            //bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            //bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (rows != -1 && columns != -1)
            {
                veQuanCo(rows, columns); //Vẽ con cờ theo lượt chơi
                rows = -1; columns = -1;
            }
        }



        //Player pp = Player.Com;
        //private void bw_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    LuongGia(pp);
        //}
        //private void startProgress(Player cc)
        //{
        //    if (bw.IsBusy != true)
        //    {
        //        pp = cc;
        //        bw.RunWorkerAsync();
        //    }
        //}
        //private void cancelProgress()
        //{
        //    if (bw.WorkerSupportsCancellation == true)
        //    {
        //        bw.CancelAsync();
        //    }
        //}


        //private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    if ((e.Cancelled == true))
        //    {
        //        progress = "Canceled!";
        //    }

        //    else if (!(e.Error == null))
        //    {
        //        progress = ("Error: " + e.Error.Message);
        //    }

        //    else
        //    {
        //        progress = "Done!";
        //    }
        //}
        //private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    progress = (e.ProgressPercentage.ToString() + "%");
        //}

        ///////////////////////////////////////////////////////////////////////////////////

        public void CreateCoAo()
        {
            coAo1.Source = new BitmapImage(new Uri("pack://application:,,,/Images/x.png"));
            coAo2.Source = new BitmapImage(new Uri("pack://application:,,,/Images/o.png"));

            coAo2.Width = coAo2.Height = length;
            coAo2.HorizontalAlignment = 0;
            coAo2.VerticalAlignment = 0;
            coAo2.Opacity = 0;

            coAo1.Width = coAo1.Height = length;
            coAo1.HorizontalAlignment = 0;
            coAo1.VerticalAlignment = 0;
            coAo1.Opacity = 0;
        }

        public void DiNgauNhien()
        {
            if (currPlayer == Player.Com)
            {
                board[row / 2, column / 2] = currPlayer;
                veQuanCo(row / 2, column / 2);
                currPlayer = Player.Human;
            }
        }
        enum LoiTreu
        {
            Đồ_con_gà,
            Thua_máy_mà_không_biết_nhục,
            Làm_ơn_thắng_dùm_một_lần_đi
        }

        //Hàm đánh cờ
        public void canvasBanCo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            #region Người đánh với máy
            if (Option.WhoPlayWith == Player.Com)//Nếu chọn kiểu chơi đánh với máy
            {
                Point toado = e.GetPosition(canvasBanCo); //Lấy tọa độ chuột

                //Xử lý tọa độ
                int cl = ((int)toado.X / length);
                int rw = ((int)toado.Y / length);
                Node node = new Node();
                if (board[rw, cl] == Player.None) //Nếu ô bấm chưa có cờ
                {
                    if (currPlayer == Player.Human && end == Player.None)//Nếu lượt đi là người và trận đấu chưa kết thúc
                    {
                        board[rw, cl] = currPlayer;//Lưu loại cờ vừa đánh vào mảng
                        veQuanCo(rw, cl);//Vẽ con cờ theo lượt chơi
                        end = CheckEnd(rw, cl);//Kiểm tra xem trận đấu kết thúc chưa
                        if (end == Player.Human)//Nếu người thắng cuộc là người
                        {
                            MessageBox.Show("Thắng máy rồi! Bạn giỏi quá");
                            canvasBanCo.IsEnabled = false;
                        }
                        else if (end == Player.None) //Nếu trận đấu chưa kết thúc
                        {
                            currPlayer = Player.Com;//Thiết lập lại lượt chơi
                        }
                    }

                    if (currPlayer == Player.Com && end == Player.None)//Nếu lượt đi là máy và trận đấu chưa kết thúc
                    {
                        //Tìm đường đi cho máy
                        eBoard.ResetBoard();

                        //Lượng giá bàn cờ cho máy
                        //Thread t = new Thread(LuongGia);
                        LuongGia(currPlayer);
                        //startProgress(currPlayer);

                        node = eBoard.GetMaxNode();//lưu vị trí máy sẽ đánh
                        int r, c;
                        r = node.Row; c = node.Column;
                        board[r, c] = currPlayer; //Lưu loại cờ vừa đánh vào mảng
                        veQuanCo(r, c); //Vẽ con cờ theo lượt chơi
                        end = CheckEnd(r, c);//Kiểm tra xem trận đấu kết thúc chưa

                        if (end == Player.Com)//Nếu máy thắng
                        {
                            MessageBox.Show("Đồ quỷ! Gà quá đi mất!");
                            canvasBanCo.IsEnabled = false;
                        }
                        else if (end == Player.None)
                        {
                            currPlayer = Player.Human;//Thiết lập lại lượt chơi
                        }
                    }
                }
            }
            #endregion
            #region 2 người đánh nhau!
            else if (Option.WhoPlayWith == Player.Human) //Nếu chọn kiểu chơi 2 người đánh với nhau
            {
                //Player.Com sẽ đóng vai trò người chơi thứ 2
                Point toado = e.GetPosition(canvasBanCo);//Lấy thông tin tọa độ chuột
                                                         //Xử lý tọa độ
                int cl = ((int)toado.X / length);
                int rw = ((int)toado.Y / length);
                if (board[rw, cl] == Player.None)//Nếu ô bấm chưa có cờ
                {
                    if (currPlayer == Player.Human && end == Player.None)//Nếu lượt đi là người và trận đấu chưa kết thúc
                    {
                        board[rw, cl] = currPlayer;//Lưu loại cờ vừa đánh vào mảng
                        veQuanCo(rw, cl);//Vẽ con cờ theo lượt chơi
                        end = CheckEnd(rw, cl);//Kiểm tra xem trận đấu kết thúc chưa
                        if (end == Player.Human)//Nếu người chơi 1 thắng
                        {
                            MessageBox.Show("Người chơi O Chiến thắng!");
                            canvasBanCo.IsEnabled = false;
                        }
                        else
                        {
                            currPlayer = Player.Com;//Thiết lập lại lượt chơi
                        }
                    }
                    else if (currPlayer == Player.Com && end == Player.None)
                    {
                        board[rw, cl] = currPlayer;//Lưu loại cờ vừa đánh vào mảng
                        veQuanCo(rw, cl);//Vẽ con cờ theo lượt chơi
                        end = CheckEnd(rw, cl);//Kiểm tra xem trận đấu kết thúc chưa
                        if (end == Player.Com)//Nếu người chơi 2 thắng
                        {
                            MessageBox.Show("Người chơi X Chiến thắng!");
                            canvasBanCo.IsEnabled = false;
                        }
                        else
                        {
                            currPlayer = Player.Human;//Thiết lập lại lượt chơi
                        }
                    }
                }
            }
            #endregion
            //#region Máy đánh online
            //else if (Option.WhoPlayWith == Player.ComputerOnline)//Nếu kiểu chơi người đánh online
            //{

            //}
            //#endregion
            #region Người online
            else if (Option.WhoPlayWith == Player.HumanOnline)
            {//Player.Com sẽ là người ở phương xa
                Point toado = e.GetPosition(canvasBanCo);

                int cl = ((int)toado.X / length);
                int rw = ((int)toado.Y / length);
                if (board[rw, cl] == Player.None)//Nếu ô bấm chưa có cờ
                    ConnectSocket.SendPosition(socket, rw, cl);
            }
            #endregion
        }

        //delegate cLuongGiaBanCo(Player player);
        //Hàm lượng giá thế cờ
        public void LuongGia(Player player)
        {
            int cntHuman = 0, cntCom = 0;//Biến đếm Human,Com

            //System.Threading.Thread.Sleep(50);
            //bw.ReportProgress(0);
            #region Lượng giá cho hàng
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column - 4; j++)
                {
                    //Khởi tạo biến đếm
                    cntHuman = cntCom = 0;
                    //Đếm số lượng con cờ trên 5 ô kế tiếp của 1 hàng
                    for (int k = 0; k < 5; k++)
                    {
                        if (board[i, j + k] == Player.Human) cntHuman++;
                        if (board[i, j + k] == Player.Com) cntCom++;
                    }
                    //Lượng giá
                    //Nếu 5 ô kế tiếp chỉ có 1 loại cờ (hoặc là Human,hoặc la Com)
                    if (cntHuman * cntCom == 0 && cntHuman != cntCom)
                    {
                        //Gán giá trị cho 5 ô kế tiếp của 1 hàng
                        for (int k = 0; k < 5; k++)
                        {
                            //Nếu ô đó chưa có quân đi
                            if (board[i, j + k] == Player.None)
                            {
                                //Nếu trong 5 ô đó chỉ tồn tại cờ của Human
                                if (cntCom == 0)
                                {
                                    //Nếu đối tượng lượng giá là Com
                                    if (player == Player.Com)
                                    {
                                        //Vì đối tượng người chơi là Com mà trong 5 ô này chỉ có Human
                                        //nên ta sẽ cộng thêm điểm phòng thủ cho Com
                                        eBoard.GiaTri[i, j + k] += PhongThu[cntHuman];
                                    }
                                    //Ngược lại cộng điểm phòng thủ cho Human
                                    else
                                    {
                                        eBoard.GiaTri[i, j + k] += TanCong[cntHuman];
                                    }
                                    // bỏ luật VN

                                }
                                //Tương tự như trên
                                if (cntHuman == 0) //Nếu chỉ tồn tại Com
                                {
                                    if (player == Player.Human) //Nếu người chơi là Người
                                    {
                                        eBoard.GiaTri[i, j + k] += PhongThu[cntCom];
                                    }
                                    else
                                    {
                                        eBoard.GiaTri[i, j + k] += TanCong[cntCom];
                                    }
                                    // bỏ luật VN

                                }
                                if ((j + k - 1 > 0) && (j + k + 1 <= column - 1) && (cntHuman == 4 || cntCom == 4)
                                   && (board[i, j + k - 1] == Player.None || board[i, j + k + 1] == Player.None))
                                {
                                    eBoard.GiaTri[i, j + k] *= 3;
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            //System.Threading.Thread.Sleep(50);
            //bw.ReportProgress(25);
            #region Lượng giá cho cột
            for (int i = 0; i < row - 4; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    cntHuman = cntCom = 0;
                    for (int k = 0; k < 5; k++)
                    {
                        if (board[i + k, j] == Player.Human) cntHuman++;
                        if (board[i + k, j] == Player.Com) cntCom++;
                    }
                    if (cntHuman * cntCom == 0 && cntCom != cntHuman)
                    {
                        for (int k = 0; k < 5; k++)
                        {
                            if (board[i + k, j] == Player.None)
                            {
                                if (cntCom == 0)
                                {
                                    if (player == Player.Com) eBoard.GiaTri[i + k, j] += PhongThu[cntHuman];
                                    else eBoard.GiaTri[i + k, j] += TanCong[cntHuman];
                                    // bỏ luật VN
                                }
                                if (cntHuman == 0)
                                {
                                    if (player == Player.Human) eBoard.GiaTri[i + k, j] += PhongThu[cntCom];
                                    else eBoard.GiaTri[i + k, j] += TanCong[cntCom];
                                    // bỏ luật VN
                                }
                                if ((i + k - 1) >= 0 && (i + k + 1) <= row - 1 && (cntHuman == 4 || cntCom == 4)
                                    && (board[i + k - 1, j] == Player.None || board[i + k + 1, j] == Player.None))
                                {
                                    eBoard.GiaTri[i + k, j] *= 3;
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            //System.Threading.Thread.Sleep(50);
            //bw.ReportProgress(50);
            #region  Lượng giá cho đường chéo xuống
            for (int i = 0; i < row - 4; i++)
            {
                for (int j = 0; j < column - 4; j++)
                {
                    cntHuman = cntCom = 0;
                    for (int k = 0; k < 5; k++)
                    {
                        if (board[i + k, j + k] == Player.Human) cntHuman++;
                        if (board[i + k, j + k] == Player.Com) cntCom++;
                    }
                    if (cntHuman * cntCom == 0 && cntCom != cntHuman)
                    {
                        for (int k = 0; k < 5; k++)
                        {
                            if (board[i + k, j + k] == Player.None)
                            {
                                if (cntCom == 0)
                                {
                                    if (player == Player.Com) eBoard.GiaTri[i + k, j + k] += PhongThu[cntHuman];
                                    else eBoard.GiaTri[i + k, j + k] += TanCong[cntHuman];
                                    // bỏ luật VN
                                }
                                if (cntHuman == 0)
                                {
                                    if (player == Player.Human) eBoard.GiaTri[i + k, j + k] += PhongThu[cntCom];
                                    else eBoard.GiaTri[i + k, j + k] += TanCong[cntCom];
                                    // bỏ luật VN
                                }
                                if ((i + k - 1) >= 0 && (j + k - 1) >= 0 && (i + k + 1) <= row - 1 && (j + k + 1) <= column - 1 && (cntHuman == 4 || cntCom == 4)
                                    && (board[i + k - 1, j + k - 1] == Player.None || board[i + k + 1, j + k + 1] == Player.None))
                                {
                                    eBoard.GiaTri[i + k, j + k] *= 3;
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            //System.Threading.Thread.Sleep(50);
            //bw.ReportProgress(75);
            #region Lượng giá cho đường chéo lên
            for (int i = 4; i < row - 4; i++)
            {
                for (int j = 0; j < column - 4; j++)
                {
                    cntCom = 0; cntHuman = 0;
                    for (int k = 0; k < 5; k++)
                    {
                        if (board[i - k, j + k] == Player.Human) cntHuman++;
                        if (board[i - k, j + k] == Player.Com) cntCom++;
                    }
                    if (cntHuman * cntCom == 0 && cntHuman != cntCom)
                    {
                        for (int k = 0; k < 5; k++)
                        {
                            if (board[i - k, j + k] == Player.None)
                            {
                                if (cntCom == 0)
                                {
                                    if (player == Player.Com)
                                        eBoard.GiaTri[i - k, j + k] += PhongThu[cntHuman];
                                    else
                                        eBoard.GiaTri[i - k, j + k] += TanCong[cntHuman];
                                    // bỏ luật VN
                                }
                                if (cntHuman == 0)
                                {
                                    if (player == Player.Human)
                                        eBoard.GiaTri[i - k, j + k] += PhongThu[cntCom];
                                    else
                                        eBoard.GiaTri[i - k, j + k] += TanCong[cntCom];
                                    // bỏ luật VN
                                }
                                if ((i - k + 1) <= row - 1 && (j + k - 1) >= 0
                                    && (i - k - 1) >= 0 && (j + k + 1) <= column - 1
                                    && (cntHuman == 4 || cntCom == 4)
                                    && (board[i - k + 1, j + k - 1] == Player.None || board[i - k - 1, j + k + 1] == Player.None))
                                {
                                    eBoard.GiaTri[i - k, j + k] *= 3;
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            //System.Threading.Thread.Sleep(50);
            //bw.ReportProgress(100);
        }

        //Hàm lấy đối thủ của người chơi hiện tại
        private Player DoiNgich(Player cur)
        {
            if (cur == Player.Com) return Player.Human;
            if (cur == Player.Human) return Player.Com;
            return Player.None;
        }
        //Hàm kiểm tra trận đấu kết thúc chưa
        private Player CheckEnd(int rw, int cl)
        {
            int rowTemp = rw;
            int colTemp = cl;
            int count1, count2, count3, count4;
            count1 = count2 = count3 = count4 = 1;
            Player cur = board[rw, cl];
            OWin.Reset();
            OWin.Add(new Node(rowTemp, colTemp));
            #region Kiem Tra Hang Ngang
            while (colTemp - 1 >= 0 && board[rowTemp, colTemp - 1] == cur)
            {
                OWin.Add(new Node(rowTemp, colTemp - 1));
                count1++;
                colTemp--;
            }
            colTemp = cl;
            while (colTemp + 1 <= column - 1 && board[rowTemp, colTemp + 1] == cur)
            {
                OWin.Add(new Node(rowTemp, colTemp + 1));
                count1++;
                colTemp++;
            }
            if (count1 == 5)
            {
                if ((colTemp - 5 >= 0 && colTemp + 1 <= column - 1 && board[rowTemp, colTemp + 1] == board[rowTemp, colTemp - 5] && board[rowTemp, colTemp + 1] == DoiNgich(cur)) ||
                    (colTemp - 5 < 0 && (board[rowTemp, colTemp + 1] == DoiNgich(cur))) ||
                    (colTemp + 1 > column - 1 && (board[rowTemp, colTemp - 5] == DoiNgich(cur))))
                { }
                else
                    return cur;
            }
            #endregion
            #region Kiem Tra Hang Doc
            OWin.Reset();
            colTemp = cl;
            OWin.Add(new Node(rowTemp, colTemp));

            while (rowTemp - 1 >= 0 && board[rowTemp - 1, colTemp] == cur)
            {
                OWin.Add(new Node(rowTemp - 1, colTemp));
                count2++;
                rowTemp--;
            }
            rowTemp = rw;
            while (rowTemp + 1 <= row - 1 && board[rowTemp + 1, colTemp] == cur)
            {
                OWin.Add(new Node(rowTemp + 1, colTemp));
                count2++;
                rowTemp++;
            }
            if (count2 == 5)
            {
                if ((rowTemp - 5 >= 0 && rowTemp + 1 <= column - 1 && board[rowTemp + 1, colTemp] == board[rowTemp - 5, colTemp] && board[rowTemp + 1, colTemp] == DoiNgich(cur)) ||
                    (rowTemp - 5 < 0 && (board[rowTemp + 1, colTemp] == DoiNgich(cur))) ||
                    (rowTemp + 1 > row - 1 && (board[rowTemp - 5, colTemp] == DoiNgich(cur))))
                { }
                else
                    return cur;
            }
            #endregion
            #region Kiem Tra Duong Cheo Chinh (\)
            colTemp = cl;
            rowTemp = rw;
            OWin.Reset();
            OWin.Add(new Node(rowTemp, colTemp));
            while (rowTemp - 1 >= 0 && colTemp - 1 >= 0 && board[rowTemp - 1, colTemp - 1] == cur)
            {
                OWin.Add(new Node(rowTemp - 1, colTemp - 1));
                count3++;
                rowTemp--;
                colTemp--;
            }
            rowTemp = rw;
            colTemp = cl;
            while (rowTemp + 1 <= row - 1 && colTemp + 1 <= column - 1 && board[rowTemp + 1, colTemp + 1] == cur)
            {
                OWin.Add(new Node(rowTemp + 1, colTemp + 1));
                count3++;
                rowTemp++;
                colTemp++;
            }
            if (count3 == 5)
            {
                if ((colTemp - 5 >= 0 && rowTemp - 5 >= 0 && colTemp + 1 <= column - 1 && rowTemp + 1 <= row - 1 && board[rowTemp + 1, colTemp + 1] == board[rowTemp - 5, colTemp - 5] && board[rowTemp + 1, colTemp + 1] == DoiNgich(cur)) ||
                       ((colTemp - 5 < 0 || rowTemp - 5 < 0) && (board[rowTemp + 1, colTemp + 1] == DoiNgich(cur))) ||
                       ((colTemp + 1 > column - 1 || rowTemp + 1 > row - 1) && (board[rowTemp - 5, colTemp - 5] == DoiNgich(cur))))
                { }
                else
                    return cur;
            }
            #endregion
            #region Kiem Tra Duong Cheo Phu
            rowTemp = rw;
            colTemp = cl;
            OWin.Reset();
            OWin.Add(new Node(rowTemp, colTemp));
            while (rowTemp + 1 <= row - 1 && colTemp - 1 >= 0 && board[rowTemp + 1, colTemp - 1] == cur)
            {
                OWin.Add(new Node(rowTemp + 1, colTemp - 1));
                count4++;
                rowTemp++;
                colTemp--;
            }
            rowTemp = rw;
            colTemp = cl;
            while (rowTemp - 1 >= 0 && colTemp + 1 <= column - 1 && board[rowTemp - 1, colTemp + 1] == cur)
            {
                OWin.Add(new Node(rowTemp - 1, colTemp + 1));
                count4++;
                rowTemp--;
                colTemp++;
            }
            if (count4 == 5)
            {
                if ((rowTemp - 1 >= 0 && rowTemp + 5 <= row - 1 && colTemp + 1 <= column - 1 && colTemp - 5 >= 0 && rowTemp + 1 <= row - 1 && board[rowTemp - 1, colTemp + 1] == board[rowTemp + 5, colTemp - 5] && board[rowTemp - 1, colTemp + 1] == DoiNgich(cur)) ||
                      ((colTemp - 5 < 0 || rowTemp + 5 > row - 1) && (board[rowTemp - 1, colTemp + 1] == DoiNgich(cur))) ||
                      ((colTemp + 1 > column - 1 || rowTemp - 1 < 0) && (board[rowTemp + 5, colTemp - 5] == DoiNgich(cur))))
                { }
                else
                    return cur;
            }
            #endregion
            return Player.None;
        }
        private void veQuanCo(int rw, int cl)
        {
            Image Chess1 = new Image();
            if (currPlayer == Player.Human)
            {
                Chess1.Source = new BitmapImage(new Uri("pack://application:,,,/Images/x.png"));

                Chess1.Width = Chess1.Height = length - 10;
                Chess1.HorizontalAlignment = 0;
                Chess1.VerticalAlignment = 0;
                Chess1.Margin = new Thickness(cl * length + 3, rw * length + 3, 0, 0);
                Chess1.Opacity = 100;
                canvasBanCo.Children.Add(Chess1);
            }
            else if (currPlayer == Player.Com)
            {
                Image Chess2 = new Image();
                Chess2.Source = new BitmapImage(new Uri("pack://application:,,,/Images/o.png"));
                Chess2.Width = Chess2.Height = length - 10;
                Chess2.HorizontalAlignment = 0;
                Chess2.VerticalAlignment = 0;
                Chess2.Margin = new Thickness(cl * length + 3, rw * length + 3, 0, 0);
                Chess2.Opacity = 100;
                canvasBanCo.Children.Add(Chess2);

            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////
    }
}

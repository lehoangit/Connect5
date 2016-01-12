using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Threading;
using System.Windows;

namespace Connect_5
{
    class ConnectSocket
    {
        public static bool first;
        public static string oldname;
        public static string Message { get; set; }
        public static cCells Cell { get; set; }

        public static void Connect(Socket socket, string name, Messager mg)
        {
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                
                mg.Server = Message = "Connected";
            });
            socket.On(Socket.EVENT_MESSAGE, (data) =>
            {
                mg.Server = Message = ((JObject)data)["message"].ToString();
                mg.Server = Message = data.ToString();
            });
            socket.On(Socket.EVENT_CONNECT_ERROR, (data) =>
            {
                mg.Server = Message = data.ToString();
            });
            socket.On("ChatMessage", (data) =>
            {
                // kiểm tra lượt đi có đúng là mình đầu tiên hay kg
                first = data.ToString().Contains("You are the first player!");
                //mg.Server = Message = data.ToString();
                mg.Server = Message = ((JObject)data)["message"].ToString();
                if (((JObject)data)["message"].ToString() == "Welcome!")
                {
                    socket.Emit("MyNameIs", name);
                    socket.Emit("ConnectToOtherPlayer");
                    oldname = name;
                }
                // nhận chat
                if (data.ToString().Contains("from"))
                {
                    mg.ChatB = ((JObject)data)["message"].ToString();
                    mg.NicknameB = ((JObject)data)["from"].ToString();
                }
            });
            socket.On("EndGame", (data) =>
            {
                mg.Server = ((JObject)data)["message"].ToString();
                Thread.Sleep(200);
                mg.Server = ((JObject)data)["highlight"].ToString();

                if (data.ToString().Contains(" left the game!"))
                    cBanCo.end = Player.None; // nobody win the game!

                if (data.ToString().Contains(" won the game!"))
                {
                    //cBanCo.end = Player.HumanOnline; // ring to won the game
                    //cBanCo.flag = Player.HumanOnline;
                    //cBanCo.end = Player.Human;
                    //GetPos( ( (JObject) data)["highlight"].ToString() );
                }

            });
            socket.On(Socket.EVENT_ERROR, (data) =>
            {
                mg.Server = Message = ((JObject)data)["message"].ToString();
            });
            socket.On("NextStepIs", (data) =>
            {
                var o = JObject.Parse(data.ToString());

                // kiểm tra lượt đánh, nếu == 1 là người kia đánh
                if ((int)o["player"] == 1)
                {
                    cBanCo.currPlayer = Player.Com;
                    cBanCo.Position.Row = (int)o["row"];
                    cBanCo.Position.Column = (int)o["col"];
                }

                // kiểm tra lượt đánh, nếu == 0 là bạn vừa mới đánh
                if ((int)o["player"] == 0)
                {
                    cBanCo.currPlayer = Player.Human;
                    cBanCo.Position.Row = (int)o["row"];
                    cBanCo.Position.Column = (int)o["col"];
                }


            });
        }

        // lay gia tri 5 diem chien thang
        public static void GetPos(string a)
        {
            bool flag = true; int index = 0;
            foreach (char item in a)
            {
                if (item > 47 && item < 58 && flag == true)
                {
                    cBanCo.OWin.GiaTri[index].Row = (int)item;
                    flag = false;
                }
                if (item >= 48 || item <= 57 && flag == false)
                {
                    cBanCo.OWin.GiaTri[index].Row = (int)item;
                    flag = true;
                    index++;
                }
            }

        }

        public static void SendPosition(Socket socket, int row, int col)
        {
            socket.On(Socket.EVENT_ERROR, (data) =>
            {
                MessageBox.Show(data.ToString());
            });
            socket.Emit("MyStepIs", JObject.FromObject(new { row = row, col = col }));
        }
        public static void ChangName(Socket socket, string name)
        {
            socket.Emit("MyNameIs", name);
            socket.Emit("message:", oldname + "is now called" + name);
            oldname = name;
        }
        public static void SendMessage(Socket socket, string name, string txt)
        {
            socket.Emit("ChatMessage", txt);
            socket.Emit("message:" + txt, "from:" + name);
        }
    }
}

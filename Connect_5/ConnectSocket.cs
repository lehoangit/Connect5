using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
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
                mg.Server = Message = ((JObject)data)["message"].ToString();
            });
            socket.On("ChatMessage", (data) =>
            {
                // kiểm tra lượt đi có đúng là mình đầu tiên hay kg
                first = data.ToString().Contains("You are the first player!");
                mg.Server = Message = data.ToString();
                //mg.Server = Message = ((JObject)data)["message"].ToString();
                if (((JObject)data)["message"].ToString() == "Welcome!")
                {
                    socket.Emit("MyNameIs", name);
                    socket.Emit("ConnectToOtherPlayer");
                    oldname = name;
                }

                // nhận chat
                //if (((JObject)data)["from"].ToString() != "")
                //{
                //    mg.ChatB = ((JObject)data)["message"].ToString();
                //    mg.NicknameB = ((JObject)data)["from"].ToString();
                //}
            });
            socket.On("EndGame", (data) =>
            {
                mg.Server = ((JObject)data)["message"].ToString();
                if (data.ToString().Contains(mg.NicknameA))
                    cBanCo.end = Player.Human; // you won the game!
                else
                    cBanCo.end = Player.Com; // your friend won the game!

                Thread.Sleep(200);
                //var tmp = ((JObject)data)["highlight"]; // save temple position won the game
                mg.Server = data.ToString();

                mg.Server = ((JObject)data)["message"].ToString();
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
                    cBanCo.currPlayer = Player.Com;

                // kiểm tra lượt đánh, nếu == 0 là bạn vừa mới đánh
                if ((int)o["player"] == 0)
                    cBanCo.currPlayer = Player.Human;

                cBanCo.rows = (int)o["row"];
                cBanCo.columns = (int)o["col"];
            });
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

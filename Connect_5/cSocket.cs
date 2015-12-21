using Connect_5.Properties;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;

namespace Connect_5
{
    class cSocket
    {
        public cSocket()
        {
            ccSocket = IO.Socket(Settings.Default._connectString);
        }

        public Socket ccSocket { get; set; }
        public string NickName { get; set; }

        public void eventConnect_Register()
        {
            //object result = null;
            ccSocket.On(Socket.EVENT_CONNECT, () =>
            {
                //"Connected";
            });
            //return result;
        }

        public object eventMessage()
        {
            object result = null;
            ccSocket.On(Socket.EVENT_MESSAGE, (data) =>
            {
                result = data;
            });
            return result;
        }

        public object eventConnectError()
        {
            object result = null;
            ccSocket.On(Socket.EVENT_CONNECT_ERROR, (data) =>
            {
                result = data;
            });
            return result;
        }


        public object eventChatMessage()
        {
            object result = null;
            ccSocket.On("ChatMessage", (data) =>
            {
                result = ((JObject)data)["message"].ToString();
                if ((string)result == "Welcome!")
                {
                    ccSocket.Emit("MyNameIs", NickName);
                    ccSocket.Emit("ConnectToOtherPlayer");
                }
            });
            return result;
        }
        public object eventError()
        {
            object result = null;
            ccSocket.On(Socket.EVENT_ERROR, (data) =>
            {
                result = data;
            });
            return result;
        }
        public object eventNextStepIs()
        {
            object result = null;
            ccSocket.On("NextStepIs", (data) =>
            {
                result = "NextStepIs: " + data;
            });
            return result;
        }
        public void eventMyStepIs(int row, int column)
        {
            ccSocket.Emit("MyStepIs", JObject.FromObject(new { row, column }));
        }
    }
}

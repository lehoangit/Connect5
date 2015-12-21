using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connect_5
{
    class cOption
    {
        private Player whoPlayWith = Player.None;//Kiểu chơi
         private string playerA = "";//Tên người chơi thứ 1
        private string playerB = "";//Tên người chơi thứ 2
        private Player luotChoi= Player.Human;//Lượt đi

        public Player LuotChoi
        {
            get{return luotChoi; }
            set{ luotChoi = value;}
        }
        public Player WhoPlayWith
        {
            get { return whoPlayWith; }
            set { whoPlayWith = value; }
        }
        public string PlayerAName
        {
            get { return playerA; }
            set { playerA = value; }
        }
        public string PlayerBName
        {
            get { return playerB; }
            set { playerB = value; }
        }
    }
}

using System;

namespace Connect_5
{
    internal class cTime
    {
        public DateTime _time;
        public cTime()
        {
            _time = GetNow();
        }
        private DateTime GetNow()
        {
            return DateTime.Now;
        }
        public string GetTimeCurrent()
        {
            string time = _time.Hour.ToString() + ":";
            time += _time.Minute.ToString() + ":";
            time += _time.Second.ToString() + " ";
            return time;
        }
    }
}
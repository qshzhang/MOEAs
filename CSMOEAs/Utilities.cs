using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusRouteCalculator
{
    /// <summary>
    /// Provides a human readable time in total hours, minutes, and seconds
    /// </summary>
    class Time
    {
        public int Hours;
        public int Minutes;
        public int Seconds;
        public int Total
        {
            get
            {
                return (Seconds + (60 * Minutes) + (3600 * Hours));
            }
            set
            {
                Total = value;
            }
        }

        public override string ToString()
        {
            string temp = "";
            if (Hours < 10)
            {
                temp += "0" + Hours.ToString() + ":";
            }
            else
            {
                temp += Hours.ToString() + ":";
            }
            if (Minutes < 10)
            {
                temp += "0" + Minutes.ToString() + ":";
            }
            else
            {
                temp += Minutes.ToString() + ":";
            }
            if (Seconds < 10)
            {
                temp += "0" + Seconds.ToString();
            }
            else
            {
                temp += Seconds.ToString();
            }
            return temp;
        }

        public Time(string _timeString)
        {
            string temp = _timeString.Substring(0, 2);
            Hours = int.Parse(temp);
            temp = _timeString.Substring(3, 2);
            Minutes = int.Parse(temp);
            temp = _timeString.Substring(6, 2);
            Seconds = int.Parse(temp);
        }

        public Time(int _hours, int _minutes, int _seconds)
        {
            Hours = _hours;
            Minutes = _minutes;
            Seconds = _seconds;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Time);
        }

        public bool Equals(Time other)
        {
            return (Hours == other.Hours && Minutes == other.Minutes && Seconds == other.Seconds);
        }

        public static Time operator ++(Time a)
        {
            a.Minutes++;
            if(a.Minutes > 59)
            {
                a.Minutes -= 60;
                a.Hours++;
            }
            if(a.Hours > 23)
            {
                a.Hours -= 24;
            }
            return a;
        }

        public static Time operator +(Time a, Time b)
        {
            int tempH = 0;
            int tempM = 0;
            int tempS = 0;

            tempS = a.Seconds + b.Seconds;
            if (tempS > 59)
            {
                tempM++;
                tempS -= 60;
            }
            tempM += a.Minutes;
            tempM += b.Minutes;
            if (tempM > 59)
            {
                tempH++;
                tempM -= 60;
            }
            tempH += a.Hours;
            tempH += b.Hours;
            if(tempH > 23)
            {
                tempH -= 24;
            }

            return new Time(tempH, tempM, tempS);
        }

        public static Time operator +(Time a, int b)
        {
            int tempH = 0;
            int tempM = 0;
            int tempS = 0;

            tempS = a.Seconds;
            tempM = a.Minutes;
            tempH = a.Hours;

            tempM += b;
            while (tempM > 59)
            {
                tempH++;
                tempM -= 60;
                if(tempH > 23)
                {
                    tempH -= 24;
                }
            }

            return new Time(tempH, tempM, tempS);
        }

        public static Time operator -(Time a, Time b)
        {
            int tempH = 0;
            int tempM = 0;
            int tempS = 0;

            tempS = a.Seconds - b.Seconds;
            if (tempS < 0)
            {
                tempM--;
                tempS += 60;
            }
            tempM += a.Minutes;
            tempM -= b.Minutes;
            if (tempM < 0)
            {
                tempH--;
                tempM += 60;
            }
            tempH += a.Hours;
            tempH -= b.Hours;
            if (tempH < 0)
            {
                tempH += 24;
            }

            return new Time(tempH, tempM, tempS);
        }

        public static bool operator >=(Time a, Time b)
        {
            if (a.Hours > b.Hours || (a.Hours == b.Hours && (a.Minutes > b.Minutes || (a.Minutes == b.Minutes && a.Seconds > b.Seconds))) || a == b)
            {
                return true;
            }
            return false;
        }

        public static bool operator <=(Time a, Time b)
        {
            if (a.Hours < b.Hours || (a.Hours == b.Hours && (a.Minutes < b.Minutes || (a.Minutes == b.Minutes && a.Seconds < b.Seconds))) || a == b)
            {
                return true;
            }
            return false;
        }

        public static bool operator >(Time a, Time b)
        {
            if(a.Hours > b.Hours || (a.Hours == b.Hours && (a.Minutes > b.Minutes || (a.Minutes == b.Minutes && a.Seconds > b.Seconds))))
            {
                return true;
            }
            return false;
        }

        public static bool operator <(Time a, Time b)
        {
            if (a.Hours < b.Hours || (a.Hours == b.Hours && (a.Minutes < b.Minutes || (a.Minutes == b.Minutes && a.Seconds < b.Seconds))))
            {
                return true;
            }
            return false;
        }

        public static bool operator ==(Time a, Time b)
        {
            return (a.Hours == b.Hours && a.Minutes == b.Minutes && a.Seconds == b.Seconds);
        }

        public static bool operator !=(Time a, Time b)
        {
            return (a.Hours != b.Hours || a.Minutes != b.Minutes || a.Seconds != b.Seconds);
        }

        public int ToMinutes()
        {
            return this.Hours*60 + this.Minutes;
        }
    }

    class Vector2
    {
        const int EARTH_RADIUS = 6371;  // In km

        public float x { get; set; }
        public float y { get; set; }

        public Vector2(float _x, float _y)
        {
            x = _x;
            y = _y;
        }

        private static float DegreesToRad(float degrees)
        {
            return degrees * (float)Math.PI / 180f;
        }

        public static float DistanceBetweenPoints(float lat1, float long1, float lat2, float long2)
        {
            return (float)Math.Acos(Math.Sin(DegreesToRad(lat1)) * Math.Sin(DegreesToRad(lat2)) + Math.Cos(DegreesToRad(lat1))
                    * Math.Cos(DegreesToRad(lat2)) * Math.Cos(DegreesToRad(long2 - long1))) * EARTH_RADIUS; ;
        }
    }

    class BusStopComparer : IComparer<BusStop>
    {
        public int Compare(BusStop x, BusStop y)
        {
            return x.ArrivalTime.Total.CompareTo(y.ArrivalTime.Total);
        }
    }

    class StopTimeComparer : IComparer<StopTime>
    {
        public int Compare(StopTime x, StopTime y)
        {
            return x.stopSequence.CompareTo(y.stopSequence);
        }
    }
}

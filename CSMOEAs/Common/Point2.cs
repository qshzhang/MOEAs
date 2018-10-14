using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Common
{
    public class Point2
    {
        public double P_X { get; set; }
        public double P_Y { get; set; }
        public Point2()
        {
            P_X = Double.MaxValue;
            P_Y = Double.MaxValue;
        }
    }
}

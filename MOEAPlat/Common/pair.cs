using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Common
{
    public class pair : IComparable<pair>
    {
        public int pos;
        public double val;
        public pair(int i, double dist)
        {
            pos = i;
            this.val = dist;
        }
        public int CompareTo(pair o)
        {
            if (o.val < val) return -1;
            else if (o.val > val) return 1;
            else return 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Common
{
    public class PairRelation : IComparable<PairRelation>
    {
        public int pos;
        public double val;
        public PairRelation(int i, double dist)
        {
            this.pos = i;
            this.val = dist;
        }
        public int CompareTo(PairRelation o)
        {
            if (o.val < val) return -1;
            else if (o.val > val) return 1;
            else return 0;
        }
    }
}

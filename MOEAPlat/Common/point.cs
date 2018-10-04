using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Common
{
    public class point
    {
        public double f1 { get; set; }
        public double f2 { get; set; }
        public point()
        {
            f1 = Double.MaxValue;
            f2 = Double.MaxValue;
        }
    }
}

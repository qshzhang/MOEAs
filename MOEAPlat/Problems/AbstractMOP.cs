using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    public abstract class AbstractMOP : MultiObjectiveProblem
    {
        protected String name;
        protected double[,] domain;
        protected double[,] range;
        protected int objDimension;
        protected int parDimension;
        protected int cneqNum = 0;
        protected int ceqNum = 0;

        public String getName()
        {
            return name;
        }

        //public double[] getIdealPoint()
        //{
        //    return idealpoint;
        //}

        public int getObjectiveSpaceDimension()
        {
            return objDimension;
        }

        public int getParameterSpaceDimension()
        {
            return parDimension;
        }

        public double[,] getRange()
        {
            return range;
        }

        public double[,] getDomain()
        {
            return domain;
        }

        public abstract void evaluate(MoChromosome chromosome);
        public abstract void init();

        public int getCNeqNum()
        {
            return cneqNum;
        }

        public int getCEqNum()
        {
            return ceqNum;
        }
    }
}

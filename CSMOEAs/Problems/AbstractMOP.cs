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

        public String GetName()
        {
            return name;
        }

        //public double[] getIdealPoint()
        //{
        //    return idealpoint;
        //}

        public int GetObjectiveSpaceDimension()
        {
            return objDimension;
        }

        public int GetParameterSpaceDimension()
        {
            return parDimension;
        }

        public double[,] GetRange()
        {
            return range;
        }

        public double[,] GetDomain()
        {
            return domain;
        }

        public abstract void Evaluate(MoChromosome chromosome);
        public abstract void Init();

        public int GetCNeqNum()
        {
            return cneqNum;
        }

        public int GetCEqNum()
        {
            return ceqNum;
        }
    }
}

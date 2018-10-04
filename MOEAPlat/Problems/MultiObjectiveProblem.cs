using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    public interface MultiObjectiveProblem
    {
        int getObjectiveSpaceDimension();
        int getParameterSpaceDimension();
        int getCNeqNum();
        int getCEqNum();
        double[,] getDomain();
        void evaluate(MoChromosome chromosome);
        string getName();
    }
}

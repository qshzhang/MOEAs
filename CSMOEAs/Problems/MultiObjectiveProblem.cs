using MOEAPlat.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems
{
    public interface MultiObjectiveProblem
    {
        int GetObjectiveSpaceDimension();
        int GetParameterSpaceDimension();
        int GetCNeqNum();
        int GetCEqNum();
        double[,] GetDomain();
        void Evaluate(MoChromosome chromosome);
        string GetName();
    }
}

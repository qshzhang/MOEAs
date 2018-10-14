using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Problems.WFG
{
    public abstract class WFG : AbstractMOP
    {
        private double epsilon = (double)1e-7;

        protected int k_; //Var for walking fish group
        protected int M_;
        protected int l_;
        protected int[] A_;
        protected int[] S_;
        protected int D_ = 1;
        protected Random random = new Random();

        /** 
        * Constructor
        * Creates a WFG problem
        * @param k position-related parameters
        * @param l distance-related parameters
        * @param M Number of objectives
        * @param solutionType The solution type must "Real" or "BinaryReal". 
        */
        public WFG(int M)
        {
            this.k_ = 2 * (M - 1);
            this.l_ = 10;
            this.M_ = M;
            parDimension = this.k_ + this.l_;
            objDimension = this.M_;

            Init();

        } // WFG

        public override void Init()
        {
            // TODO Auto-generated method stub
            //parDimension = 3;
            this.domain = new double[this.parDimension, 2];

            for (int var = 0; var < parDimension; var++)
            {
                domain[var, 0] = 0;
                domain[var, 1] = 2 * (var + 1);
            }

            this.range = new double[objDimension, 2];
        }

        /**
         * Gets the x vector (consulte WFG tooltik reference)
         */
        public double[] calculate_x(double[] t)
        {
            double[] x = new double[M_];

            for (int i = 0; i < M_ - 1; i++)
            {
                x[i] = Math.Max(t[M_ - 1], A_[i]) * (t[i] - (double)0.5) + (double)0.5;
            }

            x[M_ - 1] = t[M_ - 1];

            return x;
        } // calculate_x

        /**
         * Normalizes a vector (consulte WFG toolkit reference)
         */
        public double[] normalise(double[] z)
        {
            double[] result = new double[z.Length];

            for (int i = 0; i < z.Length; i++)
            {
                double bound = (double)2.0 * (i + 1);
                result[i] = z[i] / bound;
                result[i] = correct_to_01(result[i]);
            }

            return result;
        } // normalize    


        /**
         */
        public double correct_to_01(double a)
        {
            double min = (double)0.0;
            double max = (double)1.0;

            double min_epsilon = min - epsilon;
            double max_epsilon = max + epsilon;

            if ((a <= min && a >= min_epsilon) || (a >= min && a <= min_epsilon))
            {
                return min;
            }
            else if ((a >= max && a <= max_epsilon) || (a <= max && a >= max_epsilon))
            {
                return max;
            }
            else
            {
                return a;
            }
        } // correct_to_01  

        /**
        * Gets a subvector of a given vector
        * (Head inclusive and tail inclusive)
        * @param z the vector
        * @return the subvector
        */
        public double[] subVector(double[] z, int head, int tail)
        {
            int size = tail - head + 1;
            double[] result = new double[size];

            Array.Copy(z, head, result, head - head, tail + 1 - head);

            //System.arraycopy(z, head, result, head - head, tail + 1 - head);

            return result;
        } // subVector

    }
}

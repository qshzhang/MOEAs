using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOEAPlat.Common
{
    public static class Matrix
    {
        public static double[,] InverseMatrix(double[,] Mat)
        {
            int nRows = Mat.GetLength(0);
            int nCols = Mat.GetLength(1);
            //if (nRows != nCols) throw new ArgumentException("只有方阵才可以求逆");
            double[,] M = Mat.CopyMatrix();
            var pnRow = new int[nCols];
            var pnCol = new int[nCols];
            double d = 0.0, p = 0.0;
            int k, u, v;

            double[, ] mat = M;

            for (k = 0; k < nCols; k++)
            {
                d = 0.0;
                for (int i = k; i < nCols; i++)
                    for (int j = k; j < nCols; j++)
                    {
                        u = i * nCols + j;
                        p = Math.Abs(mat[i, j]);
                        if (p > d)//选主元
                        {
                            d = p;
                            pnRow[k] = i;
                            pnCol[k] = j;
                        }
                    }

                if (d == 0.0)
                    return null;

                if (pnRow[k] != k)
                    SwapRow(ref mat, k, pnRow[k], nCols);
                if (pnCol[k] != k)
                    SwapColumn(ref mat, k, pnCol[k], nRows);

                v = k * nCols + k;
                mat[k, k] = 1.0 / mat[k, k];
                for (int j = 0; j < nCols; j++)
                    if (j != k)
                    {
                        u = k * nCols + j;
                        mat[k, j] *= mat[k, k];
                    }

                for (int i = 0; i < nCols; i++)
                    if (i != k)
                        for (int j = 0; j < nCols; j++)
                            if (j != k)
                            {
                                u = i * nCols + j;
                                mat[i, j] -= mat[i, k] * mat[k, j];
                            }

                for (int i = 0; i < nCols; i++)
                    if (i != k)
                    {
                        u = i * nCols + k;
                        mat[i, k] *= -mat[k, k];
                    }
            }

            //恢复行列次序
            for (k = nCols - 1; k >= 0; k--)
            {
                if (pnCol[k] != k)
                    SwapRow(ref mat, k, pnCol[k], nCols);
                if (pnRow[k] != k)
                    SwapColumn(ref mat, k, pnRow[k], nRows);
            }
            return M;
        }

        private static double[,] CopyMatrix(this double[,] mat)
        {
            var NewMatrix = new Double[mat.GetLength(0), mat.GetLength(1)];
            Array.Copy(mat, NewMatrix, mat.Length);
            return NewMatrix;
        }

        private static void SwapRow(ref double[, ] arr, int i, int j, int nCols)
        {
            double tp = 0.0;
            for(int r = 0;r < nCols; r++)
            {
                tp = arr[i, r];
                arr[i, r] = arr[j, r];
                arr[j, r] = tp;
            }
        }

        private static void SwapColumn(ref double[,] arr, int i, int j, int nRows)
        {
            double tp = 0.0;
            for (int r = 0; r < nRows; r++)
            {
                tp = arr[r, i];
                arr[r, i] = arr[r, j];
                arr[r, j] = tp;
            }
        }

        public static double[] MatrixMultiple(double[,] matrix, double[] vec)
        {
            if (matrix == null) return null;
            int dim = vec.Length;

            double[] v = new double[dim];
            for (int i = 0; i < dim; i++)
            {
                v[i] = 0.0;
                for (int j = 0; j < dim; j++)
                {
                    v[i] += matrix[i, j] * vec[j];
                }
            }
            return v;
        }

        public static double[, ] IMatrix(double[,] matrix, int dim)
        {
            double[,] mat = new double[dim * 2, dim * 2];
            for(int i = 0;i < dim; i++)
            {
                for(int j = 0;j < 2 * dim; j++)
                {
                    if(j < dim)
                    {
                        mat[i, j] = matrix[i, j];
                    }
                    else
                    {
                        mat[i, j] = j - dim == i ? 1 : 0;
                    }
                }
            }
            for(int i = 0;i < dim; i++)
            {
                if(mat[i,i] < 1e-6)
                {
                    int j;
                    for ( j = i + 1; j < dim; j++)
                    {
                        if (mat[j, i] != 0) break;
                    }
                    if (j == dim) return null;
                    for(int r = i; r < 2 * dim; r++)
                    {
                        mat[i, r] += mat[j, r]; 
                    }
                }
                double ep = mat[i, i];
                for (int r = i; r < 2 * dim; r++)
                {
                    mat[i, r] /= ep;
                }

                for(int j = i + 1; j < dim; j++)
                {
                    double e = -1 * (mat[j, i] / mat[i, i]);
                    for(int r = i; r < 2 * dim; r++)
                    {
                        mat[j, r] += e * mat[i, r];
                    }
                }
            }

            for(int i = dim - 1; i >= 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    double e = -1 * (mat[j, i] / mat[i, i]);
                    for (int r = i; r < 2 * dim; r++)
                    {
                        mat[j, r] += e * mat[i, r];
                    }
                }
            } 

            double[,] result = new double[dim, dim];
            for(int i = 0;i < dim; i++)
            {
                for(int r = dim; r < 2 * dim; r++)
                {
                    result[i, r - dim] = mat[i, r];
                }
            }
            return result;
        }


        //public static double[, ] InverseMatrix(double[, ] matrix, int dim)
        //{
        //    if (matrix == null || dim == 0)
        //    {
        //        return null;
        //    }

        //    double dDeterminant = Determinant(matrix, dim);
        //    if (Math.Abs(dDeterminant) <= 1E-6)
        //    {
        //        return null;
        //    }

        //    double[, ] result = AdjointMatrix(matrix, dim);

        //    for (int i = 0; i < dim; i++)
        //    {
        //        for (int j = 0; j < dim; j++)
        //        {
        //            result[i, j] = result[i, j] / dDeterminant;
        //        }
        //    }
        //    return result;
        //}

        //public static double[] MatrixMultiple(double[, ] matrix, double[] vec)
        //{
        //    if (matrix == null) return null;
        //    int dim = vec.Length;

        //    double[] v = new double[dim];
        //    for(int i = 0;i < dim; i++)
        //    {
        //        v[i] = 0.0;
        //        for(int j = 0;j < dim; j++)
        //        {
        //            v[i] += matrix[i, j] * vec[j];
        //        }
        //    }
        //    return v;
        //}

        //private static double Determinant(double[, ] matrix, int dim)
        //{

        //    if (dim == 0) return 0;
        //    else if (dim == 1) return matrix[0, 0];
        //    else if (dim == 2)
        //    {
        //        return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
        //    }

        //    double dSum = 0, dSign = 1;
        //    for (int i = 0; i < dim; i++)
        //    {
        //        double[, ] matrixTemp = new double[dim - 1, dim - 1];
        //        for (int j = 1; j < dim; j++)
        //        {
        //            for (int k = 0; k < dim - 1; k++)
        //            {
        //                matrixTemp[j - 1, k] = matrix[j, k >= i ? k + 1 : k];
        //            }

        //        }

        //        dSum += (matrix[0, i] * dSign * Determinant(matrixTemp, dim-1));
        //        dSign = dSign * -1;
        //    }
        //    return dSum;
        //}

        //private static double[, ] AdjointMatrix(double[, ] matrix, int dim)
        //{
        //    double[, ] result = new double[dim, dim];

        //    for (int i = 0; i < dim; i++)
        //    {
        //        for (int j = 0; j < dim; j++)
        //        {
        //            double[, ] temp = new double[dim - 1, dim - 1];

        //            for (int x = 0; x < dim - 1; x++)
        //            {
        //                for (int y = 0; y < dim - 1; y++)
        //                {
        //                    temp[x, y] = matrix[x < i ? x : x + 1, y < j ? y : y + 1];
        //                }
        //            }

        //            result[j, i] = ((i + j) % 2 == 0 ? 1 : -1) * Determinant(temp, dim - 1);
        //        }
        //    }
        //    return result;
        //}
    }
}

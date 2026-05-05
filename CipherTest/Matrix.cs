using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherTest
{
    internal class Matrix
    {
        public int[][] Body;
        public Matrix(int[][] body)
        {
            Body = body;
        }
        public int GetDeterminant()
        {
            if (Body.Length != Body[0].Length) return 0;

            int mSize = Body[0].Length;

            if (mSize == 1) return Body[0][0];
            if (mSize == 2) return Body[0][0] * Body[1][1] - Body[0][1] * Body[1][0];

            int res = 0;
            for (int j = 0; j < mSize; j++)
            {
                res += Body[0][j] * (int)Math.Pow(-1, j) * GetMinor(0, j).GetDeterminant();
            }
            return res;
        }

        public Matrix GetMinor(int skipRow, int skipCol)
        {
            int mSize = Body.Length;
            int[][] minor = new int[mSize - 1][];
            int ri = 0;
            for (int i = 0; i < mSize; i++)
            {
                if (i == skipRow) continue;
                minor[ri] = new int[mSize - 1];
                int ci = 0;
                for (int j = 0; j < mSize; j++)
                {
                    if (j == skipCol) continue;
                    minor[ri][ci++] = Body[i][j];
                }
                ri++;
            }
            return new Matrix(minor);
        }

        public Matrix GetInverse(int[][] matrix)
        {
            if (GetDeterminant() == 0) return null;
            int[][] minorMatrix = new int[matrix.Length][];
            for (int i = 0; i < matrix.Length; i++) {
                minorMatrix[i] = new int[matrix[i].Length];
                for (int j = 0; j < matrix[i].Length; j++) {
                    minorMatrix[i][j] = GetMinor(i, j).GetDeterminant() * (int)Math.Pow(-1, i + j);
                }
            }

            int[][] inverseMatrix = new int[matrix.Length][];
            for (int i = 0; i < matrix.Length; i++)
            {
                inverseMatrix[i] = new int[matrix.Length];
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    inverseMatrix[i][j] = minorMatrix[j][i];
                }
            }
            return new Matrix (inverseMatrix);
        }

        public static Matrix Multiply(int[][] matrix1, int[][] matrix2)
        {
            if (matrix1[0].Length != matrix2.Length) return null;
            int[][] resultMatrix = new int[matrix2.Length][];
            for(int i = 0; i < resultMatrix.Length; i++)
            {
                resultMatrix[i] = new int[matrix2[0].Length];
                for(int j = 0; j < resultMatrix[0].Length; j++)
                {
                    resultMatrix[i][j] = 0;
                    for(int n = 0; n < resultMatrix.Length; n++)
                    {
                        resultMatrix[i][j] += matrix1[i][n] * matrix2[n][j];
                    }
                }
            }
            Console.WriteLine($"Multiply: \n{Stringify(resultMatrix)}");
            
            return new Matrix(resultMatrix);
        }

        public override string ToString()
        {
            string res = "";
            foreach (int[] i in Body)
            {
                res += '|';
                foreach (int j in i)
                {
                    res += $" {j}\t |";
                }
                res += "\n";
            }
            return res;
        }

        public static string Stringify(int[][] matrix)
        {
            string res = "";
            foreach (int[] i in matrix)
            {
                res += '|';
                foreach (int j in i)
                {
                    res += $" {j}\t |";
                }
                res += "\n";
            }
            return res;
        }
    }
}

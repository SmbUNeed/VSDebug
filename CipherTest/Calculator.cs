using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherTest
{
    internal class Calculator
    {
        static string alphabet = "abcdefghijklmnopqrstvuwxyz.,?! ";
        private static int GetMatrixDeterminant(int[][] matrix)
        {
            if (matrix.Length != matrix[0].Length) return 0;

            int mSize = matrix[0].Length;

            if (mSize == 1) return matrix[0][0];
            if (mSize == 2) return matrix[0][0] * matrix[1][1] - matrix[0][1] * matrix[1][0];

            int res = 0;
            for (int lr = 0; lr < 2; lr++)
            {
                for (int i = 0; i < mSize; i++)
                {
                    int mul = 1;
                    for (int j = 0; j < mSize; j++)
                    {
                        int index = j + i;
                        if (index >= mSize) index -= mSize;
                        if (lr == 0) mul *= matrix[j][index];
                        else mul *= matrix[index][mSize - j - 1];
                    }
                    if (lr == 0) res += mul;
                    else res -= mul;
                }
            }
            return res;
        }

        private static int[][] GetMinor(int[][] matrix, int skipRow, int skipCol)
        {
            int mSize = matrix.Length;
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
                    minor[ri][ci++] = matrix[i][j];
                }
                ri++;
            }
            return minor;
        }

        public static int[][] GetInverseMatrix(int[][] matrix)
        {
            if (GetMatrixDeterminant(matrix) == 0) return null;
            int[][] minorMatrix = new int [matrix.Length][];
            for(int i = 0; i < matrix.Length; i++)
                for (int j = 0; j < matrix[i].Length; j++)
                    minorMatrix[i][j] = GetMatrixDeterminant(GetMinor(matrix, i, j)) * (int)Math.Pow(-1, i+j);
            int[][] inverseMatrix = new int[matrix.Length][];
            for (int i = 0; i < matrix.Length; i++)
                for (int j = 0; j < matrix[i].Length; j++)
                    inverseMatrix[i][j] = minorMatrix[matrix.Length -  i - 1][matrix.Length - j - 1];
            return minorMatrix;
        }
    }
}

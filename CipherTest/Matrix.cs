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

        private static int ModInverse(int a, int mod)
        {
            a = ((a % mod) + mod) % mod;
            if (a == 0) return -1;

            int old_r = a, r = mod;
            int old_s = 1, s = 0;

            while (r != 0)
            {
                int q = old_r / r;
                (old_r, r) = (r, old_r - q * r);
                (old_s, s) = (s, old_s - q * s);
            }

            if (old_r != 1) return -1;
            return (old_s % mod + mod) % mod;
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

        public Matrix GetAdjugate()
        {
            int[][] res = new int[Body.Length][];
            for (int i = 0; i < Body.Length; i++)
            {
                res[i] = new int[Body[i].Length];
                for (int j = 0; j < Body[i].Length; j++)
                {
                    res[i][j] = GetMinor(i, j).GetDeterminant() * (int)Math.Pow(-1, i + j);
                }
            }
            return new Matrix(res);
        }

        public Matrix GetInverse(int mod)
        {
            int det = ((GetDeterminant() % mod) + mod) % mod;
            int detInv = ModInverse(det, mod);
            if (detInv == -1) return null;

            Matrix adj = GetAdjugate().Transponate();

            int[][] res = new int[Body.Length][];
            for (int i = 0; i < Body.Length; i++)
            {
                res[i] = new int[Body[i].Length];
                for (int j = 0; j < Body[i].Length; j++)
                    res[i][j] = ((adj.Body[i][j] * detInv) % mod + mod) % mod;
            }
            return new Matrix(res);
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
            
            return new Matrix(resultMatrix);
        }

        public static int[] MultiplyWithKey(int[][] matrix1, int[] stringEncode, int mod)
        {
            int strLen = stringEncode.Length;
            int[][] matrix = new int[strLen][];

            for (int i = 0; i < strLen; i++)
                matrix[i] = new int[] { stringEncode[i] };

            Matrix multiplied = Multiply(matrix1, matrix);

            int[] result = new int[strLen];
            for (int i = 0; i < strLen; i++)
                result[i] = ((multiplied.Body[i][0] % mod) + mod) % mod;
            return result;
        }

        public Matrix Transponate()
        {
            int[][] result = new int[Body.Length][];
            for(int i = 0; i < Body.Length; i++)
            {
                result[i] = new int[Body[0].Length];
                for(int j = 0;j < Body[0].Length; j++)
                {
                    result[i][j] = Body[j][i];
                }
            }
            return new Matrix(result);
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

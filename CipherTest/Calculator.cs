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
        public static int GetMatrixDeterminant(int[][] matrix)
        {
            if (matrix.Length != matrix[0].Length) return 0;
            int mSize = matrix.Length;
            int leftD = 0;
            int rightD = 0;
            int counter = 0;
            for (int i = 0; i < mSize; i++)
            {
                int und = 1;
                for (int j = 0; j < mSize; j++)
                {
                    und *= matrix[j + counter][j + counter]; // Переписать
                    if (und == 0) break;
                }
                leftD += und;
                counter++;
            }
            counter = 0;
            return 0;
        }

        public static bool CheckMatrixDeterminantIsCorrect(int determinant)
        {
            return false;
        }

        public static int[][] GetInverseMatrix(int[][] matrix)
        {
            return new int[matrix.Length][];
        }

        /*public static int[][] SubMatrix(int[][] matrix, int x, int y)
        {
            int[][] resultMatrix = new int[matrix.Length-1][];
            int countX = -1;
            int countY = -1;
            if (matrix.Length != matrix[0].Length) return new int[matrix.Length][];
            for (int i = 0; i < matrix.Length; i++)
            {
                if (i == x) continue;
                countX++;
                for (int j = 0;  j < matrix[i].Length; j++)
                {
                    if (j == y) continue;
                    countY++;
                    resultMatrix[countX][countY] = matrix[i][j];
                }
            }
            return resultMatrix;
        }*/
    }
}

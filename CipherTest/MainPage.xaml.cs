using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CipherTest
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            int[][] matrix = new int[][]
            {
                new int[] {1, 3, 2},
                new int[] {2, 5, 1},
                new int[] {9, 6, 6}
            };

            int[][] matrix1 = new int[][]
            {
                new int[] {5, 8, -4},
                new int[] {6, 9, -5},
                new int[] {4, 7, -3},
            };
            
            int[][] matrix2 = new int[][]
            {
                new int[] {2},
                new int[] {-3},
                new int[] {1}
            };

            string s = "пися";
            Matrix m = new Matrix(matrix);
            string encoded = Encoder.EncodeString(s, m);
            string decoded = Encoder.DecodeString(encoded, m);

            Console.WriteLine(encoded + "\n" + decoded);
        }
    }
}

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
        Matrix curMatrix = new Matrix(new int[3][]);
        TextBox[][] TextBlocks;
        public MainPage()
        {
            InitializeComponent();
            TextBlocks = new TextBox[3][]
            {
                new TextBox[] {m11, m12, m13 },
                new TextBox[] {m21, m22, m23 },
                new TextBox[] {m31, m32, m33 },
            };
            foreach (var tbs in TextBlocks)
                foreach (var t in tbs)
                    t.TextChanged += (s, e) => MatrixChanged(s, e);
        }

        private Matrix GetMatrix()
        {
            int[][] m = new int[3][];
            for(int i = 0; i < 3; i++)
            {
                m[i] = new int[3];
                for(int j = 0; j < 3; j++)
                {
                    if (!int.TryParse(TextBlocks[i][j].Text, out m[i][j])) m[i][j] = 0;
                }
            }
            return new Matrix(m);
        }

        private void MatrixChanged(object sender, TextChangedEventArgs e)
        {
            curMatrix = GetMatrix();
            if (curMatrix.GetInverse(Encoder.Alphabet.Length) == null)
            {
                WarningTB.Text = "Некорректный ключ";
                return;
            }
            else WarningTB.Text = "";
        }

        private void Encode(object sender, RoutedEventArgs e)
        {
            if (curMatrix.GetInverse(Encoder.Alphabet.Length) == null) return;
            ResultBox.Text = Encoder.EncodeString(InputBox.Text, curMatrix);
        }

        private void Decode(object sender, RoutedEventArgs e)
        {
            if (curMatrix.GetInverse(Encoder.Alphabet.Length) == null) return;
            ResultBox.Text = Encoder.DecodeString(InputBox.Text, curMatrix);
        }
    }
}

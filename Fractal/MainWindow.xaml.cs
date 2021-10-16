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

namespace Fractal
{
    public partial class MainWindow : Window
    {
        static List<Point> Points = new List<Point>();
        static int Deep = 8;

        public MainWindow()
        {
            InitializeComponent();

            string condition1 = "LbFRAFARFbL";
            string condition2 = "RAFLBFBLFAR";
            string str = "A";
            for (int i = 0; i < Deep; i++)
                str = LGenerator(str, condition1, condition2);
            str = DeleteConditions(str);
            LParser(str, 15);
            Draw(Brushes.Green, 2);
        }

        void Draw(Brush color, int thickness)
        {
            for (int i = 1; i < Points.Count; i++)
            {
                Field.Children.Add(new Line()
                {
                    X1 = Points[i - 1].X,
                    X2 = Points[i].X,
                    Y1 = Points[i - 1].Y,
                    Y2 = Points[i].Y,
                    StrokeThickness = thickness,
                    Stroke = color
                });
            }
        }

        string LGenerator(string input, string condition1, string condition2)
        {
            input = input.Replace("A", condition1);
            input = input.Replace("B", condition2);
            input = input.Replace("b", "B");
            return input;
        }

        string DeleteConditions(string input)
        {
            input = input.Replace("A", string.Empty);
            input = input.Replace("B", string.Empty);
            return input;
        }

        void LParser(string str, double len)
        {
            int direction = 40;
            Points.Add(new Point(500, 150));
            foreach(char c in str)
            {
                if (c == 'L')
                    direction--;
                else if (c == 'R')
                    direction++;
                else
                    Points.Add(AddPoint(Points[^1].X, Points[^1].Y, direction, len));
            }
        }

        Point AddPoint(double x, double y, int direction, double len)
        {
            direction = direction % 4;
            if (direction == 1)
                return new Point(x, y - len);
            else if (direction == 2)
                return new Point(x + len, y);
            else if (direction == 3)
                return new Point(x, y + len);
            return new Point(x - len, y);
        }
    }
}

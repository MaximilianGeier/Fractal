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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static List<Point> Points = new List<Point>();
        static int Deep = 10;

        public MainWindow()
        {
            InitializeComponent();
/*            int x = 20;
            int y = 20;
            Points.Add(new Point(x, y));
            Fractal(1, 800, x, y, 4);
            Draw(Brushes.Green, 2);*/

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

        void Fractal(int deraction, int len, double x, double y, int Deep)
        {
            Deep--;
            if(deraction == 1)
            {
                if (Deep <= 1)   //true
                {
                    Points.Add(new Point(x, y + len));
                    Points.Add(new Point(x + len, y + len));
                    Points.Add(new Point(x + len, y));
                    return;
                }
                else  //true
                {
                    Fractal(4, len / 3, x, y, Deep);
                    Points.Add(new Point(x, y + ((len / 3) * 2)));
                    Fractal(1, len / 3, x, y + ((len / 3) * 2), Deep);
                    Points.Add(new Point(x + ((len / 3) * 2), y + ((len / 3) * 2)));
                    Fractal(1, len / 3, x + ((len / 3) * 2), y + ((len / 3) * 2), Deep);
                    Points.Add(new Point(x + len, y + len / 3));
                    Fractal(2, len / 3, x + len, y + len / 3, Deep);
                }
            }
            else if(deraction == 2)
            {
                if(Deep <= 1) //true
                {
                    Points.Add(new Point(x - len, y));
                    Points.Add(new Point(x - len, y - len));
                    Points.Add(new Point(x, y - len));
                    return;
                }
                else
                {
                    Fractal(3, len / 3, x, y, Deep);
                    Points.Add(new Point(x - ((len / 3) * 2), y));
                    Fractal(2, len / 3, x - ((len / 3) * 2), y, Deep);
                    Points.Add(new Point(x - ((len / 3) * 2), y - ((len / 3) * 2)));
                    Fractal(2, len / 3, x - (len / 3) * 2, y - ((len / 3) * 2), Deep);
                    Points.Add(new Point(x - (len / 3), y - len));
                    Fractal(1, len / 3, x - (len / 3), y - len, Deep);
                }
            }
            else if (deraction == 3)
            {
                if(Deep <= 1)  //true
                {
                    Points.Add(new Point(x, y - len));
                    Points.Add(new Point(x - len, y - len));
                    Points.Add(new Point(x - len, y));
                }
                else
                {
                    Fractal(2, len / 3, x, y, Deep);
                    Points.Add(new Point(x, y - ((len / 3) * 2)));
                    Fractal(3, len / 3, x, y - ((len / 3) * 2), Deep);
                    Points.Add(new Point(x - ((len / 3) * 2), y - ((len / 3) * 2)));
                    Fractal(3, len / 3, x - ((len / 3) * 2), y - ((len / 3) * 2), Deep);
                    Points.Add(new Point(x - len, y - (len / 3)));
                    Fractal(4, len / 3, x - len, y - (len / 3), Deep);

                }
            }
            else if (deraction == 4)
            {
                if(Deep <= 1)  //true
                {
                    Points.Add(new Point(x + len, y));
                    Points.Add(new Point(x + len, y + len));
                    Points.Add(new Point(x, y + len));
                    return;
                }
                else  //true
                {
                    Fractal(1, len / 3, x, y, Deep);
                    Points.Add(new Point(x + ((len / 3) * 2), y));
                    Fractal(4, len / 3, x + ((len / 3) * 2), y, Deep);
                    Points.Add(new Point(x + ((len / 3) * 2), y + ((len / 3) * 2)));
                    Fractal(4, len / 3, x + ((len / 3) * 2), y + ((len / 3) * 2), Deep);
                    Points.Add(new Point(x + (len / 3), y + len));
                    Fractal(3, len / 3, x + (len / 3), y + len, Deep);
                }
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
            input.Replace("A", string.Empty);
            return input.Replace("B", string.Empty);
        }

        void LParser(string str, double len)
        {
            int direction = 40;
            Points.Add(new Point(800, 150));
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

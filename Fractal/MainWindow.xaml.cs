using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
        public MainWindow()
        {
            InitializeComponent();

            CalculateAndDrawFractal(FractalName.HilbertCurve);
        }

        void CalculateAndDrawFractal(FractalName fractalName)
        {
            switch (fractalName)
            {
                case FractalName.HilbertCurve:
                    HilbertCurve hilbertCurvefractal = new HilbertCurve(5);
                    hilbertCurvefractal.CalculateAndDraw(ref Field);
                    break;
                case FractalName.Tree:
                    TreeFractal treefractal = new TreeFractal(4, 5, 100);
                    treefractal.CalculateAndDraw(ref Field, Width, Height);
                    break;
            }
        }

        enum FractalName
        {
            HilbertCurve,
            Tree
        }
    }

    class HilbertCurve
    {
        public readonly int Deep;
        List<Point> points;

        public HilbertCurve(int deep)
        {
            Deep = deep;
        }

        public void CalculateAndDraw(ref Canvas canvas)
        {
            points = new List<Point>();

            string condition1 = "LbFRAFARFbL";
            string condition2 = "RAFLBFBLFAR";
            string str = "A";
            str = LGenerator(str, condition1, condition2, Deep);
            str = DeleteConditions(str);
            LParser(str, 15);
            Draw(ref canvas, Brushes.Green, 2);
        }

        void Draw(ref Canvas canvas, Brush color, int thickness)
        {
            for (int i = 1; i < points.Count; i++)
            {
                canvas.Children.Add(new Line()
                {
                    X1 = points[i - 1].X,
                    X2 = points[i].X,
                    Y1 = points[i - 1].Y,
                    Y2 = points[i].Y,
                    StrokeThickness = thickness,
                    Stroke = color
                });
            }
        }

        string LGenerator(string input, string condition1, string condition2, int deep)
        {
            deep--;
            if (deep >= 0)
                input = LGenerator(input, condition1, condition2, deep);
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
            points.Add(new Point(5, 5));
            foreach (char c in str)
            {
                if (c == 'L')
                    direction--;
                else if (c == 'R')
                    direction++;
                else
                    points.Add(AddPoint(points[^1].X, points[^1].Y, direction, len));
            }
        }

        Point AddPoint(double x, double y, int direction, double len)
        {
            direction = direction % 4;
            if (direction == 1)
                return new Point(x, y - len);
            else if (direction == 2)
                return new Point(x - len, y);
            else if (direction == 3)
                return new Point(x, y + len);
            return new Point(x + len, y);
        }
    }

    class TreeFractal
    {
        public readonly int BranchNum; 
        public readonly int Depth;     
        public readonly double Length; 
        public readonly double AngleIndent;
        public readonly double AngleDigression;
        public readonly double AngleInc;
        List<Line> lines;

        public TreeFractal(int branchNum, int depth, double length)
        {
            BranchNum = branchNum;
            Depth = depth;
            Length = length;

            AngleIndent = Math.PI / 5;
            double angle = Math.PI - AngleIndent * 2;
            AngleDigression = angle / (BranchNum + 1);
            AngleInc = angle / (BranchNum + 1);
        }
        public void CalculateAndDraw(ref Canvas canvas, double winWidth, double winHeight)
        {
            lines = new List<Line>();

            DoFactorial(new Point(winWidth * 0.5, winHeight * 0.9), Math.PI / 2, 1);

            SetLinesOnCanvas(ref canvas);
        }

        void DoFactorial(Point lastPoint, double angle, int depth)
        {
            for (int i = 0; i < BranchNum; i++)
            {
                double currentAngle = angle - Math.PI / 2 + AngleIndent + AngleDigression + AngleInc * i;
                double currentLength = Length * ((Depth - depth + 1d) / Depth);
                Point currentPoint = new Point(lastPoint.X + Math.Cos(currentAngle) * currentLength, lastPoint.Y - Math.Sin(currentAngle) * currentLength);
                
                AddLine(lastPoint, currentPoint);
                if (depth < Depth)
                {
                    DoFactorial(currentPoint, currentAngle, depth + 1);
                }
            }
        }
        void AddLine(Point point1, Point point2)
        {
            Line line = new Line();
            line.X1 = point1.X;
            line.Y1 = point1.Y;
            line.X2 = point2.X;
            line.Y2 = point2.Y;
            line.StrokeThickness = 2;
            lines.Add(line);
        }

        void SetLinesOnCanvas(ref Canvas canvas)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                byte value = (byte)((i/(double)lines.Count) * 210);
                lines[i].Stroke = new SolidColorBrush(Color.FromRgb(value, value, value));
                canvas.Children.Add(lines[i]);
            }
        }
    }
}

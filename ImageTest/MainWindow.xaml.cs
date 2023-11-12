using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ImageTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Rectangle DragRectangle = null;
        private Point StartPoint, LastPoint;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel()
            { 
                AddRectangleAction = AddRectangle,
                ChangeHeightAction = ChangeHeight
            };
        }

        private void CanDraw_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                StartPoint = Mouse.GetPosition(CanDraw);
                LastPoint = StartPoint;
                DragRectangle = new Rectangle();
                DragRectangle.Width = 1;
                DragRectangle.Height = 24;
                DragRectangle.Stroke = Brushes.MediumSlateBlue;
                DragRectangle.StrokeThickness = 2;
                DragRectangle.Cursor = Cursors.Cross;

                CanDraw.Children.Add(DragRectangle);
                Canvas.SetLeft(DragRectangle, StartPoint.X);
                Canvas.SetTop(DragRectangle, StartPoint.Y);

                CanDraw.MouseMove += canDraw_MouseMove;
                CanDraw.MouseUp += canDraw_MouseUp;
                CanDraw.CaptureMouse();
            }
            else if(e.ChangedButton == MouseButton.Right)
            {
                int count = CanDraw.Children.Count;
                if(count > 1)
                {
                    CanDraw.Children.RemoveAt(count - 1);
                }
            }
        }

        internal void ChangeHeight(int height)
        {
            foreach(var rect in CanDraw.Children)
            {
                if(rect is Rectangle)
                {
                    (rect as Rectangle).Height = height;
                    (rect as Rectangle).Stroke = Brushes.Aquamarine;
                }
            }
        }

        internal void AddRectangle(int x, int y, int width, int height)
        {
            var rectangle = new Rectangle();
            rectangle.Width = width;
            rectangle.Height = height;
            rectangle.Stroke = Brushes.MediumSlateBlue;
            rectangle.StrokeThickness = 2;
            CanDraw.Children.Add(rectangle);
            Canvas.SetLeft(rectangle, x);
            Canvas.SetTop(rectangle, y);
        }

        private void canDraw_MouseMove(object sender, MouseEventArgs e)
        {
            LastPoint = Mouse.GetPosition(CanDraw);
            DragRectangle.Width = Math.Abs(LastPoint.X - StartPoint.X);
            //DragRectangle.Height = Math.Abs(LastPoint.Y - StartPoint.Y);
            DragRectangle.Height = 24;
            Canvas.SetLeft(DragRectangle, Math.Min(LastPoint.X, StartPoint.X));
            //Canvas.SetTop(DragRectangle, Math.Min(LastPoint.Y, StartPoint.Y));
        }

        private void canDraw_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CanDraw.ReleaseMouseCapture();
            CanDraw.MouseMove -= canDraw_MouseMove;
            CanDraw.MouseUp -= canDraw_MouseUp;
            //CanDraw.Children.Remove(DragRectangle);
            DragRectangle = null;

            if (LastPoint.X < 0) LastPoint.X = 0;
            if (LastPoint.X >= CanDraw.Width) LastPoint.X = CanDraw.Width - 1;
            if (LastPoint.Y < 0) LastPoint.Y = 0;
            if (LastPoint.Y >= CanDraw.Height) LastPoint.Y = CanDraw.Height - 1;

            int x = (int)Math.Min(LastPoint.X, StartPoint.X);
            int y = (int)Math.Min(LastPoint.Y, StartPoint.Y);
            int width = (int)Math.Abs(LastPoint.X - StartPoint.X) + 1;
            int height = (int)Math.Abs(LastPoint.Y - StartPoint.Y) + 1;

            

            
        }
    }
}

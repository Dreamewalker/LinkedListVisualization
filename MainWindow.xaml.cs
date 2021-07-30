using LinkedListVisualization.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LinkedListVisualization
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        // 窗口拖动缩放相关
        public const int WM_SYSCOMMAND = 0x112;
        public HwndSource HwndSource;

        public enum ResizeDirection
        {
            Left = 1,
            Right = 2,
            Top = 3,
            TopLeft = 4,
            TopRight = 5,
            Bottom = 6,
            BottomLeft = 7,
            BottomRight = 8,
        }

        public Dictionary<ResizeDirection, Cursor> cursors = new Dictionary<ResizeDirection, Cursor>
            {
            {ResizeDirection.Top, Cursors.SizeNS},
            {ResizeDirection.Bottom, Cursors.SizeNS},
            {ResizeDirection.Left, Cursors.SizeWE},
            {ResizeDirection.Right, Cursors.SizeWE},
            {ResizeDirection.TopLeft, Cursors.SizeNWSE},
            {ResizeDirection.BottomRight, Cursors.SizeNWSE},
            {ResizeDirection.TopRight, Cursors.SizeNESW},
            {ResizeDirection.BottomLeft, Cursors.SizeNESW}
            };

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public MainWindow()
        {
            InitializeComponent();
            this.SourceInitialized += MainWindow_SourceInitialized;
            this.Loaded += MainWindow_Loaded;
            this.MouseMove += MainWindow_MouseMove;
        }

        void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                FrameworkElement element = e.OriginalSource as FrameworkElement;
                if (element != null && !element.Name.Contains("Resize"))
                {
                    this.Cursor = Cursors.Arrow;
                }
            }
        }

        void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            this.HwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ControlTemplate customWindowTemplate = App.Current.Resources["CustomWindowTemplete"] as ControlTemplate;
            if (customWindowTemplate != null)
            {
                var TopLeft = customWindowTemplate.FindName("ResizeTopLeft", this) as Rectangle;
                TopLeft.MouseMove += ResizePressed;
                TopLeft.MouseDown += ResizePressed;
                var Top = customWindowTemplate.FindName("ResizeTop", this) as Rectangle;
                Top.MouseMove += ResizePressed;
                Top.MouseDown += ResizePressed;
                var TopRight = customWindowTemplate.FindName("ResizeTopRight", this) as Rectangle;
                TopRight.MouseMove += ResizePressed;
                TopRight.MouseDown += ResizePressed;
                var Left = customWindowTemplate.FindName("ResizeLeft", this) as Rectangle;
                Left.MouseMove += ResizePressed;
                Left.MouseDown += ResizePressed;
                var Right = customWindowTemplate.FindName("ResizeRight", this) as Rectangle;
                Right.MouseMove += ResizePressed;
                Right.MouseDown += ResizePressed;
                var BottomLeft = customWindowTemplate.FindName("ResizeBottomLeft", this) as Rectangle;
                BottomLeft.MouseMove += ResizePressed;
                BottomLeft.MouseDown += ResizePressed;
                var Bottom = customWindowTemplate.FindName("ResizeBottom", this) as Rectangle;
                Bottom.MouseMove += ResizePressed;
                Bottom.MouseDown += ResizePressed;
                var BottomRight = customWindowTemplate.FindName("ResizeBottomRight", this) as Rectangle;
                BottomRight.MouseMove += ResizePressed;
                BottomRight.MouseDown += ResizePressed;
            }
        }

        public void ResizePressed(object sender, MouseEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            ResizeDirection direction = (ResizeDirection)Enum.Parse(typeof(ResizeDirection), element.Name.Replace("Resize", ""));
            this.Cursor = cursors[direction];
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ResizeWindow(direction);
            }
        }

        public void ResizeWindow(ResizeDirection direction)
        {
            SendMessage(HwndSource.Handle, WM_SYSCOMMAND, (IntPtr)(61440 + direction), IntPtr.Zero);
        }

        private void TopBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void NewWidgetTest_Click(object sender, RoutedEventArgs e)
        {
            Arrow arrow = new Arrow();
            ListElement listElement = new ListElement();
            GeneralCanvas.Children.Add(arrow);
            GeneralCanvas.Children.Add(listElement);
            Canvas.SetTop(arrow, 200);
            Canvas.SetLeft(arrow, 400);
            Canvas.SetTop(listElement, 300);
            Canvas.SetLeft(listElement, 500);

            Storyboard storyboard = new Storyboard();
            double thisCompleteTime = arrow.Expand(storyboard, 0);
            listElement.Show(storyboard, 0);

            thisCompleteTime = arrow.Rotate(storyboard, thisCompleteTime + 1, 60);
            thisCompleteTime = arrow.Move(storyboard, thisCompleteTime + 0.5, 60, 80);
            thisCompleteTime = listElement.Move(storyboard, thisCompleteTime + 0.5, 100, 0);

            thisCompleteTime = listElement.Close(storyboard, thisCompleteTime + 2);
            arrow.Close(storyboard, thisCompleteTime + 1);
            BeginStoryboard(storyboard);
            //storyboard.Begin();

            //System.Threading.Thread.Sleep(1000);
            listElement.SetProperty(7, 255, 155, 155);
        }

        private void NewLinkedList(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = new Storyboard();

            ListElement[] listElements = new ListElement[5];
            Arrow[] arrows = new Arrow[4];
            double currentTime = 0;
            for (int i = 0; i < 5; ++i)
            {
                listElements[i] = new ListElement(i, (byte)(40 * i), 155, 155);
                GeneralCanvas.Children.Add(listElements[i]);
                Canvas.SetLeft(listElements[i], 500 + 160 * i);
                Canvas.SetTop(listElements[i], 400);
                listElements[i].Show(storyboard, 0);
            }

            for (int i = 0; i < 4; ++i)
            {
                arrows[i] = new Arrow();
                GeneralCanvas.Children.Add(arrows[i]);
                Canvas.SetLeft(arrows[i], 590 + 160 * i);
                Canvas.SetTop(arrows[i], 420);
                currentTime = arrows[i].Expand(storyboard, 0);
            }

            currentTime = arrows[3].Close(storyboard, currentTime + 0.5);
            listElements[4].Close(storyboard, currentTime);
            storyboard.Begin();
        }

    }
}

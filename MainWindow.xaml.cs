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

            // 禁用输入框右键菜单
            CreateNodeNumEditView.ContextMenu = null;

            // Control Panel中ToolBar按钮数组初始化
            toolBarButtons[0] = ToolBarCreateButton;
            toolBarButtons[1] = ToolBarInsertButton;
            toolBarButtons[2] = ToolBarDeleteButton;
            toolBarButtons[3] = ToolBarUpdateButton;
            toolBarButtons[4] = ToolBarQueryButton;

            // Opr Panel中各个面板数组初始化
            oprPanelGrids[0] = CreateOprGrid;
            oprPanelGrids[1] = InsertOprGrid;
            oprPanelGrids[2] = DeleteOprGrid;
            oprPanelGrids[3] = UpdateOprGrid;
            oprPanelGrids[4] = QueryOprGrid;
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


        private void NewLinkedList(object sender, RoutedEventArgs e)
        {
            // Width bias = 785
            // Height bias = 200
            Storyboard storyboard = new Storyboard();

            ListElement[] listElements = new ListElement[5];
            Arrow[] arrows = new Arrow[4];
            double currentTime = 0;
            for (int i = 0; i < 5; ++i)
            {
                listElements[i] = new ListElement(i, (byte)(40 * i), 155, 155);
                GeneralCanvas.Children.Add(listElements[i]);
                Canvas.SetLeft(listElements[i], 785 + 500 + 160 * i);
                Canvas.SetTop(listElements[i], 200 + 400);
                listElements[i].currentCanvasLeft = 785 + 500 + 160 * i;
                listElements[i].currentCanvasTop = 600;

                listElements[i].Show(storyboard, 0);
            }

            for (int i = 0; i < 4; ++i)
            {
                arrows[i] = new Arrow();
                GeneralCanvas.Children.Add(arrows[i]);
                Canvas.SetLeft(arrows[i], 785 + 590 + 160 * i);
                Canvas.SetTop(arrows[i], 200 + 420);
                arrows[i].currentCanvasLeft = 785 + 590 + 160 * i;
                arrows[i].currentCanvasTop = 200 + 420;

                currentTime = arrows[i].Expand(storyboard, 0);
            }

            currentTime = arrows[3].Close(storyboard, currentTime + 0.5);
            listElements[4].Close(storyboard, currentTime);
            storyboard.Begin();
        }

        private double currentScaleRate = 1;
        private void ViewboxGeneralCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta == 0)
            {
                return;
            }

            double destination = e.Delta / Math.Abs(e.Delta);

            if (currentScaleRate < 0.3 && destination < 0)
            {
                return;
            }

            if (currentScaleRate > 5 && destination > 0)
            {
                return;
            }

            currentScaleRate += destination * 0.1;
            Point targetZoomFocus = e.GetPosition(ViewboxGeneralCanvas);

            GeneralCanvasScaleTransform.ScaleX = currentScaleRate;
            GeneralCanvasScaleTransform.ScaleY = currentScaleRate;

            Point cursorPosAfter = e.GetPosition(ViewboxGeneralCanvas);

            double deltaX = targetZoomFocus.X * destination * 0.1;
            double deltaY = targetZoomFocus.Y * destination * 0.1;

            double newCanvasLeft = Canvas.GetLeft(ViewboxGeneralCanvas) - deltaX;
            double newCanvasTop = Canvas.GetTop(ViewboxGeneralCanvas) - deltaY;
            Canvas.SetLeft(ViewboxGeneralCanvas, newCanvasLeft);
            Canvas.SetTop(ViewboxGeneralCanvas, newCanvasTop);

        }

        private double prevHoriChange = 0;
        private double prevVerChange = 0;
        private void ThumbGeneralCanvas_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            double currentLeft = Canvas.GetLeft(ViewboxGeneralCanvas);
            double currentRight = this.Width - 40 - currentScaleRate * GeneralCanvas.Width - currentLeft;
            double currentTop = Canvas.GetTop(ViewboxGeneralCanvas);
            double currentBotton = this.Height - 40 - currentScaleRate * GeneralCanvas.Height - currentTop;

            if (e.HorizontalChange > prevHoriChange)
            {
                // 向右移动
                if (currentLeft <= 400)
                {
                    double newCanvasLeft = currentLeft + e.HorizontalChange - prevHoriChange;
                    Canvas.SetLeft(ViewboxGeneralCanvas, newCanvasLeft);
                }
            }
            else
            {
                // 向左移动
                if (currentRight <= 0)
                {
                    double newCanvasLeft = currentLeft + e.HorizontalChange - prevHoriChange;
                    Canvas.SetLeft(ViewboxGeneralCanvas, newCanvasLeft);
                }
            }


            if (e.VerticalChange > prevVerChange)
            {
                // 向下移动
                if (currentTop <= 0)
                {
                    double newCanvasTop = currentTop + e.VerticalChange - prevVerChange;
                    Canvas.SetTop(ViewboxGeneralCanvas, newCanvasTop);
                }
            }
            else
            {
                // 向上移动
                if (currentBotton <= 0)
                {
                    double newCanvasTop = currentTop + e.VerticalChange - prevVerChange;
                    Canvas.SetTop(ViewboxGeneralCanvas, newCanvasTop);
                }
            }
            prevHoriChange = e.HorizontalChange;
            prevVerChange = e.VerticalChange;
        }

        private void ThumbGeneralCanvas_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            prevHoriChange = 0;
            prevVerChange = 0;
        }

        private int currentNewListType = 0;
        private void NewSingleLinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentNewListType == 0)
            {
                return;
            }
            else if (currentNewListType == 1)
            {
                NewRecycleLinkButton.MinWidth = 1;
            }
            else
            {
                NewBidirectLinkButton.MinWidth = 1;
            }
            NewSingleLinkButton.MinWidth = 0;
            Storyboard storyboard = new Storyboard();

            //TypeMaskLabel.Content = "单链表";

            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;

            // Move Anim
            DoubleAnimation doubleAnimation = new DoubleAnimation(Canvas.GetLeft(TypeSelectedMask), 0, new Duration(TimeSpan.FromMilliseconds(500 + 500 * (currentNewListType - 0))));
            doubleAnimation.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(doubleAnimation, TypeSelectedMask);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));

            storyboard.Children.Add(doubleAnimation);

            storyboard.Begin();

            currentNewListType = 0;
        }

        private void NewRecycleLinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentNewListType == 1)
            {
                return;
            }
            else if (currentNewListType == 0)
            {
                NewSingleLinkButton.MinWidth = 1;
            }
            else
            {
                NewBidirectLinkButton.MinWidth = 1;
            }
            NewRecycleLinkButton.MinWidth = 0;

            //TypeMaskLabel.Content = "循环链表";
            Storyboard storyboard = new Storyboard();

            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;

            // Move Anim
            DoubleAnimation doubleAnimation = new DoubleAnimation(Canvas.GetLeft(TypeSelectedMask), 97, new Duration(TimeSpan.FromMilliseconds(500 + 500 * Math.Abs(currentNewListType - 1))));
            doubleAnimation.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(doubleAnimation, TypeSelectedMask);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));

            storyboard.Children.Add(doubleAnimation);

            storyboard.Begin();

            currentNewListType = 1;

        }

        private void NewBidirectLinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentNewListType == 2)
            {
                return;
            }
            else if (currentNewListType == 1)
            {
                NewRecycleLinkButton.MinWidth = 1;
            }
            else
            {
                NewSingleLinkButton.MinWidth = 1;
            }
            NewBidirectLinkButton.MinWidth = 0;
            //TypeMaskLabel.Content = "双向链表";
            Storyboard storyboard = new Storyboard();

            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;

            // Move Anim
            DoubleAnimation doubleAnimation = new DoubleAnimation(Canvas.GetLeft(TypeSelectedMask), 194, new Duration(TimeSpan.FromMilliseconds(500 + 500 * Math.Abs(currentNewListType - 2))));
            doubleAnimation.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(doubleAnimation, TypeSelectedMask);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));

            storyboard.Children.Add(doubleAnimation);

            storyboard.Begin();

            currentNewListType = 2;
        }

        /*
        private int currentImplSelect = 0;
        private void ImplPointerButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentImplSelect == 0)
            {
                return;
            }

            ImplPointerButton.MinWidth = 0;
            ImplArrayButton.MinWidth = 1;

            if (Canvas.GetLeft(HeadSelectedMask) > 50)
            {
                HeadOnButton.MinWidth = 1;
                HeadOffButton.MinWidth = 0;
            }
            else
            {
                HeadOnButton.MinWidth = 0;
                HeadOffButton.MinWidth = 1;
            }
            //HeadSelectedMask.Opacity = 1;

            Storyboard storyboard = new Storyboard();


            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;

            // Show HeadSelectMask Anim
            DoubleAnimation showAmin = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(500)));
            showAmin.EasingFunction = new ExponentialEase()
            {
                EasingMode = EasingMode.EaseOut
            };
            Storyboard.SetTarget(showAmin, HeadSelectedMask);
            Storyboard.SetTargetProperty(showAmin, new PropertyPath("Opacity"));
            storyboard.Children.Add(showAmin);

            // Move Anim
            DoubleAnimation doubleAnimation = new DoubleAnimation(Canvas.GetLeft(ImplSelectedMask), 0, new Duration(TimeSpan.FromMilliseconds(1000)));
            doubleAnimation.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(doubleAnimation, ImplSelectedMask);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));

            storyboard.Children.Add(doubleAnimation);

            storyboard.Begin();

            currentImplSelect = 0;
        }

        private void ImplArrayButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentImplSelect == 1)
            {
                return;
            }

            ImplPointerButton.MinWidth = 1;
            ImplArrayButton.MinWidth = 0;

            HeadOnButton.MinWidth = 0;
            HeadOffButton.MinWidth = 0;
            //HeadSelectedMask.Opacity = 0;

            Storyboard storyboard = new Storyboard();


            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;

            // Hide HeadSelectMask Anim
            DoubleAnimation hideAmin = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(500)));
            hideAmin.EasingFunction = new ExponentialEase()
            {
                EasingMode = EasingMode.EaseOut
            };
            Storyboard.SetTarget(hideAmin, HeadSelectedMask);
            Storyboard.SetTargetProperty(hideAmin, new PropertyPath("Opacity"));
            storyboard.Children.Add(hideAmin);

            // Move Anim
            DoubleAnimation doubleAnimation = new DoubleAnimation(Canvas.GetLeft(ImplSelectedMask), 97, new Duration(TimeSpan.FromMilliseconds(1000)));
            doubleAnimation.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(doubleAnimation, ImplSelectedMask);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));

            storyboard.Children.Add(doubleAnimation);

            storyboard.Begin();

            currentImplSelect = 1;
        }
        */

        private int currentHeadSelect = 0;
        private void HeadOnButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentHeadSelect == 0 || HeadOnButton.MinWidth < 0.5)
            {
                return;
            }

            HeadOnButton.MinWidth = 0;
            HeadOffButton.MinWidth = 1;

            Storyboard storyboard = new Storyboard();


            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;

            // Move Anim
            DoubleAnimation doubleAnimation = new DoubleAnimation(Canvas.GetLeft(HeadSelectedMask), 0, new Duration(TimeSpan.FromMilliseconds(1000)));
            doubleAnimation.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(doubleAnimation, HeadSelectedMask);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));

            storyboard.Children.Add(doubleAnimation);

            storyboard.Begin();

            currentHeadSelect = 0;
        }

        private void HeadOffButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentHeadSelect == 1 || HeadOffButton.MinWidth < 0.5)
            {
                return;
            }

            HeadOnButton.MinWidth = 1;
            HeadOffButton.MinWidth = 0;

            Storyboard storyboard = new Storyboard();


            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;

            // Move Anim
            DoubleAnimation doubleAnimation = new DoubleAnimation(Canvas.GetLeft(HeadSelectedMask), 97, new Duration(TimeSpan.FromMilliseconds(1000)));
            doubleAnimation.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(doubleAnimation, HeadSelectedMask);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));

            storyboard.Children.Add(doubleAnimation);

            storyboard.Begin();

            currentHeadSelect = 1;
        }

        private int currentTailSelect = 0;
        private void TailOnButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentTailSelect == 0 || TailOnButton.MinWidth < 0.5)
            {
                return;
            }

            TailOnButton.MinWidth = 0;
            TailOffButton.MinWidth = 1;

            Storyboard storyboard = new Storyboard();


            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;

            // Move Anim
            DoubleAnimation doubleAnimation = new DoubleAnimation(Canvas.GetLeft(TailSelectedMask), 0, new Duration(TimeSpan.FromMilliseconds(1000)));
            doubleAnimation.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(doubleAnimation, TailSelectedMask);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));

            storyboard.Children.Add(doubleAnimation);

            storyboard.Begin();

            currentTailSelect = 0;
        }

        private void TailOffButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentTailSelect == 1 || TailOffButton.MinWidth < 0.5)
            {
                return;
            }

            TailOnButton.MinWidth = 1;
            TailOffButton.MinWidth = 0;

            Storyboard storyboard = new Storyboard();


            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;

            // Move Anim
            DoubleAnimation doubleAnimation = new DoubleAnimation(Canvas.GetLeft(TailSelectedMask), 97, new Duration(TimeSpan.FromMilliseconds(1000)));
            doubleAnimation.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(doubleAnimation, TailSelectedMask);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));

            storyboard.Children.Add(doubleAnimation);

            storyboard.Begin();

            currentTailSelect = 1;
        }

        private void CreateNewListButton_Click(object sender, RoutedEventArgs e)
        {
            string title = "";
            switch (currentNewListType)
            {
                case 0:
                    title += "单链表·";
                    break;
                case 1:
                    title += "循环链表·";
                    break;
                case 2:
                    title += "双向链表·";
                    break;
            }

            if (currentHeadSelect == 0)
            {
                title += "有头结点·";
            }
            else
            {
                title += "无头结点·";
            }

            if (currentTailSelect == 0)
            {
                title += "有尾指针";
            }
            else
            {
                title += "无尾指针";
            }

            OprPanelTitle.Content = title;

            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;

            Storyboard storyboard = new Storyboard();
            
            DoubleAnimation newPanelAnimation = new DoubleAnimation(0, -370, new Duration(TimeSpan.FromMilliseconds(1500)));
            newPanelAnimation.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(newPanelAnimation, NewPanel);
            Storyboard.SetTargetProperty(newPanelAnimation, new PropertyPath("(Canvas.Left)"));

            storyboard.Children.Add(newPanelAnimation);

            DoubleAnimation oprPanelAnimation = new DoubleAnimation(370, 0, new Duration(TimeSpan.FromMilliseconds(1500)));
            oprPanelAnimation.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(oprPanelAnimation, OprPanel);
            Storyboard.SetTargetProperty(oprPanelAnimation, new PropertyPath("(Canvas.Left)"));

            storyboard.Children.Add(oprPanelAnimation);

            
            storyboard.Begin();
        }

        private void ResetOprPanel(object sender, EventArgs e)
        {
            GeneralCanvas.Children.Clear();
            toolBarButtons[0].MinWidth = 1;
            for (int i = 1; i < 5; ++i)
            {
                toolBarButtons[i].MinWidth = 0;
            }

            ToolBarButton_Click(ToolBarCreateButton, null);
            for (int i = 1; i < 5; ++i)
            {
                toolBarButtons[i].MinWidth = 0;
            }
            /*
            Canvas.SetLeft(OprSelectedMask, 0);

            for (int i = 0; i < 5; ++i)
            {
                Canvas.SetLeft(oprPanelGrids[i], 370 * i);
            }
            */
        }

        private void BackNewButton_Click(object sender, RoutedEventArgs e)
        {
            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;

            Storyboard storyboard = new Storyboard();

            DoubleAnimation newPanelAnimation = new DoubleAnimation(-370, 0, new Duration(TimeSpan.FromMilliseconds(1500)));
            newPanelAnimation.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(newPanelAnimation, NewPanel);
            Storyboard.SetTargetProperty(newPanelAnimation, new PropertyPath("(Canvas.Left)"));

            storyboard.Children.Add(newPanelAnimation);

            DoubleAnimation oprPanelAnimation = new DoubleAnimation(0, 370, new Duration(TimeSpan.FromMilliseconds(1500)));
            oprPanelAnimation.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(oprPanelAnimation, OprPanel);
            Storyboard.SetTargetProperty(oprPanelAnimation, new PropertyPath("(Canvas.Left)"));

            storyboard.Children.Add(oprPanelAnimation);

            CloseWidgetsOnCanvas(storyboard, 0);

            storyboard.Completed += new EventHandler(ResetOprPanel);
            storyboard.Begin();
        }

        private void CreateNodeNumEditView_TextChanged(object sender, TextChangedEventArgs e)
        {
            string targetContent = "";
            int currentCursorPos = CreateNodeNumEditView.SelectionStart;
            foreach (char c in CreateNodeNumEditView.Text)
            {
                if (c <= '9' && c >= '0')
                {
                    targetContent += c;
                }
            }
            if (targetContent.Length > 5)
            {
                targetContent = targetContent.Substring(0, 5);
            }
            int diff = targetContent.Length - CreateNodeNumEditView.Text.Length;
            CreateNodeNumEditView.Text = targetContent;
            CreateNodeNumEditView.SelectionStart = currentCursorPos + diff;
        }

        private void CreateNodeNumEditView_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CreateNodeNumEditView.Text.Length <= 0)
            {
                CreateNodeNumEditView.Text = "0";
            }
        }

        private Button[] toolBarButtons = new Button[5];
        private Grid[] oprPanelGrids = new Grid[5];
        private int currentToolBarSelect = 0;

        private void ToolBarButton_Click(object sender, RoutedEventArgs e)
        {
            int clickedButtonIndex = 0;
            for (int i = 1; i < 5; ++i)
            {
                if (sender == toolBarButtons[i])
                {
                    clickedButtonIndex = i;
                }
            }

            if (clickedButtonIndex == currentToolBarSelect || ((Button)sender).MinWidth == 0)
            {
                return;
            }

            toolBarButtons[clickedButtonIndex].MinWidth = 0;
            toolBarButtons[currentToolBarSelect].MinWidth = 1;

            Storyboard storyboard = new Storyboard();
            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;
            // Tool Bar Anim
            DoubleAnimation toolBarAnim = new DoubleAnimation(currentToolBarSelect * 74, clickedButtonIndex * 74, new Duration(TimeSpan.FromMilliseconds(500 + 500 * Math.Abs(clickedButtonIndex - currentToolBarSelect))));
            toolBarAnim.EasingFunction = nonLinearEasingFunction;
            Storyboard.SetTarget(toolBarAnim, OprSelectedMask);
            Storyboard.SetTargetProperty(toolBarAnim, new PropertyPath("(Canvas.Left)"));
            storyboard.Children.Add(toolBarAnim);

            // Panel Switch Anim
            DoubleAnimation[] panelSwitchAnims = new DoubleAnimation[5];
            for (int i = 0; i < 5; ++i)
            {
                panelSwitchAnims[i] = new DoubleAnimation((i - currentToolBarSelect) * 370, (i - clickedButtonIndex) * 370, new Duration(TimeSpan.FromMilliseconds(500 + 500 * Math.Abs(clickedButtonIndex - currentToolBarSelect))));
                panelSwitchAnims[i].EasingFunction = nonLinearEasingFunction;
                Storyboard.SetTarget(panelSwitchAnims[i], oprPanelGrids[i]);
                Storyboard.SetTargetProperty(panelSwitchAnims[i], new PropertyPath("(Canvas.Left)"));
                storyboard.Children.Add(panelSwitchAnims[i]);
            }
            storyboard.Begin();
            currentToolBarSelect = clickedButtonIndex;
        }

        private Node root = null;
        private Node prevRoot = null;
        private Node rearNode = null;
        private VisualPointer rearPointer = null;
        private VisualPointer rootPointer = null;
        private Dictionary<string, VisualPointer> generalVisualPtrSet = new Dictionary<string, VisualPointer>();
        private Dictionary<string, VisualPointer> prevGeneralVisualPtrSet = null;

        private void RemoveWidgetsOnCanvas(object sender, EventArgs e)
        {
            if (prevRoot != null)
            {
                prevRoot.RemoveFromCanvas(GeneralCanvas);
            }
            foreach (var pointer in prevGeneralVisualPtrSet.Values)
            {
                GeneralCanvas.Children.Remove(pointer);
            }
        }

        private double CloseWidgetsOnCanvas(Storyboard storyboard, double prevCompleteTime)
        {
            prevRoot = root;
            prevGeneralVisualPtrSet = generalVisualPtrSet;
            generalVisualPtrSet = new Dictionary<string, VisualPointer>();

            if (GeneralCanvas.Children.Count == 0)
            {
                return -0.2;
            }
            /*
            if (rearPointer != null && rearPointer.PointerType.Opacity > 0.5)
            {
                rearPointer.Close(storyboard, 0);
            }

            if (rootPointer != null && rootPointer.PointerType.Opacity > 0.5)
            {
                rootPointer.Close(storyboard, 0);
            }
            */

            if (prevRoot != null)
            {
                prevRoot.CloseAnim(storyboard, 0);
            }

            foreach (var pointer in prevGeneralVisualPtrSet.Values)
            {
                if (pointer.PointerType.Opacity > 0.5)
                {
                    pointer.Close(storyboard, 0);
                }
            }
            return prevCompleteTime + 0.7;
        }

        private void CreateEmptyButton_Click(object sender, RoutedEventArgs e)
        {
            // GeneralCanvas.Children.Clear();
            Storyboard storyboard = new Storyboard();

            // clear widgets on canvas
            double clearDoneTime = CloseWidgetsOnCanvas(storyboard, 0);


            if (currentHeadSelect == 0)
            {
                root = new Node(-1, 155, 155, 155, null);

                rootPointer = new VisualPointer("Root", root);
                generalVisualPtrSet.Add("Root", rootPointer);
                
                if (currentTailSelect == 0)
                {
                    rearNode = root;
                    rearPointer = new VisualPointer("Rear", rearNode);
                    generalVisualPtrSet.Add("Rear", rearPointer);
                    //rearArrow.Show(storyboard, clearDoneTime + 0.2);
                }
                root.InitialDrawLinear(GeneralCanvas, storyboard, false, clearDoneTime + 0.2, generalVisualPtrSet);
            }
            storyboard.Completed += new EventHandler(RemoveWidgetsOnCanvas);

            storyboard.Begin();
            for (int i = 1; i < 5; ++i)
            {
                toolBarButtons[i].MinWidth = 1;
            }
        }

        private void CreateRandomButton_Click(object sender, RoutedEventArgs e)
        {
            //GeneralCanvas.Children.Clear();
            Storyboard storyboard = new Storyboard();

            // clear widgets on canvas
            double clearDoneTime = CloseWidgetsOnCanvas(storyboard, 0);
            

            Random random = new Random();
            if (currentHeadSelect == 0)
            {
                root = new Node(-1, 155, 155, 155, null);

                rootPointer = new VisualPointer("Root", root);
                generalVisualPtrSet.Add("Root", rootPointer);
            }
            else if (CreateNodeNumEditView.Text != "0")
            {
                root = new Node(random.Next(0, 100), 155, 155, 155, null);

                rootPointer = new VisualPointer("Root", root);
                generalVisualPtrSet.Add("Root", rootPointer);
            }
            else
            {
                return;
            }

            int nodeNum = int.Parse(CreateNodeNumEditView.Text) - currentHeadSelect;
            Node currentPtr = root;
            rearNode = root;
            for (int i = 0; i < nodeNum; ++i)
            {
                currentPtr.CreateNextNode(random.Next(0, 100), currentNewListType == 2);

                currentPtr = currentPtr.nextPtr;
            }

            rearNode = currentPtr;

            if (currentNewListType == 1)
            {
                rearNode.nextPtr = root;
                rearNode.nextArrow = new Arrow();
            }

            if (currentTailSelect == 0)
            {
                rearPointer = new VisualPointer("Rear", rearNode);
                generalVisualPtrSet.Add("Rear", rearPointer);
            }

            if (currentNewListType == 1)
            {
                root.InitialDrawRecycle(GeneralCanvas, storyboard, nodeNum + 1, clearDoneTime + 0.2, generalVisualPtrSet);
            }
            else
            {
                root.InitialDrawLinear(GeneralCanvas, storyboard, currentNewListType == 2, clearDoneTime + 0.2, generalVisualPtrSet);
            }
            
            storyboard.Completed += new EventHandler(RemoveWidgetsOnCanvas);
            storyboard.Begin();

            for (int i = 1; i < 5; ++i)
            {
                toolBarButtons[i].MinWidth = 1;
            }
        }

        private void CreateIncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = new Storyboard();

            // clear widgets on canvas
            double clearDoneTime = CloseWidgetsOnCanvas(storyboard, 0);

            int nodeNum = int.Parse(CreateNodeNumEditView.Text);
            Random random = new Random();
            List<int> randomArray = new List<int>();
            for (int c = 0; c < nodeNum; ++c)
            {
                randomArray.Add(random.Next(0, 100));
            }
            randomArray.Sort();

            int i = 0;
            if (currentHeadSelect == 0)
            {
                root = new Node(-1, 155, 155, 155, null);

                rootPointer = new VisualPointer("Root", root);
                generalVisualPtrSet.Add("Root", rootPointer);
            }
            else if (CreateNodeNumEditView.Text != "0")
            {
                root = new Node(randomArray[0], 155, 155, 155, null);

                rootPointer = new VisualPointer("Root", root);
                generalVisualPtrSet.Add("Root", rootPointer);
                i = 1;
            }
            else
            {
                return;
            }
            Node currentPtr = root;
            for (; i < nodeNum; ++i)
            {
                currentPtr.CreateNextNode(randomArray[i], currentNewListType == 2);
                currentPtr = currentPtr.nextPtr;
            }

            rearNode = currentPtr;

            if (currentTailSelect == 0)
            {
                rearPointer = new VisualPointer("Rear", rearNode);
                generalVisualPtrSet.Add("Rear", rearPointer);
            }
            

            if (currentNewListType == 1)
            {
                currentPtr.nextPtr = root;
                currentPtr.nextArrow = new Arrow();
                root.InitialDrawRecycle(GeneralCanvas, storyboard, nodeNum + 1 - currentHeadSelect, clearDoneTime + 0.2, generalVisualPtrSet);
            }
            else
            {
                root.InitialDrawLinear(GeneralCanvas, storyboard, currentNewListType == 2, clearDoneTime + 0.2, generalVisualPtrSet);
            }
            storyboard.Completed += new EventHandler(RemoveWidgetsOnCanvas);
            storyboard.Begin();

            for (i = 1; i < 5; ++i)
            {
                toolBarButtons[i].MinWidth = 1;
            }
        }

        private void CreateDecreaseBuuton_Click(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = new Storyboard();

            // clear widgets on canvas
            double clearDoneTime = CloseWidgetsOnCanvas(storyboard, 0);

            int nodeNum = int.Parse(CreateNodeNumEditView.Text);
            Random random = new Random();
            List<int> randomArray = new List<int>();
            for (int c = 0; c < nodeNum; ++c)
            {
                randomArray.Add(random.Next(0, 100));
            }
            randomArray.Sort();

            int i = nodeNum - 1;
            if (currentHeadSelect == 0)
            {
                root = new Node(-1, 155, 155, 155, null);

                rootPointer = new VisualPointer("Root", root);
                generalVisualPtrSet.Add("Root", rootPointer);
            }
            else if (CreateNodeNumEditView.Text != "0")
            {
                root = new Node(randomArray[i], 155, 155, 155, null);

                rootPointer = new VisualPointer("Root", root);
                generalVisualPtrSet.Add("Root", rootPointer);
                --i;
            }
            else
            {
                return;
            }
            Node currentPtr = root;
            for (; i >= 0; --i)
            {
                currentPtr.CreateNextNode(randomArray[i], currentNewListType == 2);
                currentPtr = currentPtr.nextPtr;
            }

            rearNode = currentPtr;

            if (currentTailSelect == 0)
            {
                rearPointer = new VisualPointer("Rear", rearNode);
                generalVisualPtrSet.Add("Rear", rearPointer);
            }

            if (currentNewListType == 1)
            {
                currentPtr.nextPtr = root;
                currentPtr.nextArrow = new Arrow();
                root.InitialDrawRecycle(GeneralCanvas, storyboard, nodeNum + 1 - currentHeadSelect, clearDoneTime + 0.2, generalVisualPtrSet);
            }
            else
            {
                root.InitialDrawLinear(GeneralCanvas, storyboard, currentNewListType == 2, clearDoneTime + 0.2, generalVisualPtrSet);
            }
            storyboard.Completed += new EventHandler(RemoveWidgetsOnCanvas);
            storyboard.Begin();

            for (i = 1; i < 5; ++i)
            {
                toolBarButtons[i].MinWidth = 1;
            }
        }


        private void InsertConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string[] instructions = new string[] { "nNew newNodePtr, 4", "nMoveAbs newNodePtr, 1380, 435.455", "nSetSameNext newNodePtr, Root", "nSetNextPtr Root, newNodePtr", "Halt" };
            programCounter = 0;
            Storyboard storyboard = new Storyboard();
            double completeTime = 0;

            while (programCounter >= 0)
            {
                completeTime = ExecuteNextInstruction(instructions[programCounter], storyboard, completeTime);
                ++programCounter;
            }

            storyboard.Begin();
        }






        // Assemble Interpreter
        private string[] Decode(string instruction)
        {
            List<string> temp = new List<string>();
            string tempStr = "";
            for (int i = 0; i < instruction.Length; ++i)
            {
                if (instruction[i] != ' ' && instruction[i] != ',')
                {
                    tempStr += instruction[i];
                }
                else if (tempStr.Length > 0)
                {
                    temp.Add(tempStr);
                    tempStr = "";
                }
            }
            temp.Add(tempStr);

            string[] retArray = new string[temp.Count];
            for (int i = 0; i < temp.Count; ++i)
            {
                retArray[i] = temp[i];
            }

            return retArray;
        }


        private int programCounter = 0;

        public double ExecuteNextInstruction(string instruction, Storyboard storyboard, double prevCompleteTime)
        {
            string[] decodeResult = Decode(instruction);
            double completeTime = prevCompleteTime;
            switch (decodeResult[0])
            {
                case "gNew":
                    {
                        VisualPointer visualPointer = new VisualPointer(decodeResult[1], null);
                        generalVisualPtrSet.Add(decodeResult[1], visualPointer);
                        Canvas.SetLeft(visualPointer, 785 + 400);
                        Canvas.SetTop(visualPointer, 800);
                        visualPointer.currentCanvasLeft = 785 + 400;
                        visualPointer.currentCanvasTop = 800;

                        GeneralCanvas.Children.Add(visualPointer);
                        completeTime = visualPointer.Show(storyboard, prevCompleteTime);
                        break;
                    }
                    
                case "gMove":
                    {
                        VisualPointer srcPtr = null;
                        generalVisualPtrSet.TryGetValue(decodeResult[2], out srcPtr);
                        VisualPointer dstPtr = null;
                        generalVisualPtrSet.TryGetValue(decodeResult[1], out dstPtr);

                        List<VisualPointer> srcRelatedPtrs = srcPtr.pointingNode.GetRelatedPointers(generalVisualPtrSet);
                        List<VisualPointer> dstRelatedPtrs = dstPtr.pointingNode == null ? new List<VisualPointer>() : dstPtr.pointingNode.GetRelatedPointers(generalVisualPtrSet);

                        int dstPosition = 0;
                        for (dstPosition = 0; dstPosition < dstRelatedPtrs.Count && dstRelatedPtrs[dstPosition] != dstPtr; ++dstPosition)
                        {

                        }

                        for (int i = dstPosition + 1; i < dstRelatedPtrs.Count; ++i)
                        {
                            dstRelatedPtrs[i].MoveToAnim(storyboard, prevCompleteTime, dstRelatedPtrs[i - 1].currentCanvasLeft, dstRelatedPtrs[i - 1].currentCanvasTop, dstRelatedPtrs[i - 1].currentAngle);
                        }

                        dstPtr.pointingNode = srcPtr.pointingNode;

                        VisualPointer lastSrc = srcRelatedPtrs[srcRelatedPtrs.Count - 1];
                        completeTime = dstPtr.MoveToAnim(storyboard, prevCompleteTime, lastSrc.currentCanvasLeft + 60 * Math.Sin(lastSrc.Rotation.Angle / 180 * Math.PI), lastSrc.currentCanvasTop + 60 * Math.Cos(lastSrc.currentAngle / 180 * Math.PI), lastSrc.currentAngle);

                        break;
                    }
                    
                case "gDelete":
                    {
                        VisualPointer freeDstPtr = null;
                        generalVisualPtrSet.TryGetValue(decodeResult[1], out freeDstPtr);

                        List<VisualPointer> freeDstRelatedPtrs = freeDstPtr.pointingNode.GetRelatedPointers(generalVisualPtrSet);

                        completeTime = freeDstPtr.Close(storyboard, prevCompleteTime);

                        int freeDstIdx = 0;
                        for (; freeDstIdx < freeDstRelatedPtrs.Count && freeDstRelatedPtrs[freeDstIdx] != freeDstPtr; ++freeDstIdx)
                        {

                        }

                        for (int i = freeDstIdx + 1; i < freeDstRelatedPtrs.Count; ++i)
                        {
                            completeTime = freeDstRelatedPtrs[i].MoveToAnim(storyboard, completeTime, freeDstRelatedPtrs[i - 1].currentCanvasLeft, freeDstRelatedPtrs[i - 1].currentCanvasTop, freeDstRelatedPtrs[i - 1].currentAngle);
                        }

                        generalVisualPtrSet.Remove(decodeResult[1]);
                        break;
                    }

                case "nNew":
                    {
                        Node newNode = new Node(int.Parse(decodeResult[2]), 155, 155, 155, null);
                        VisualPointer newPointer = new VisualPointer(decodeResult[1], newNode);
                        generalVisualPtrSet.Add(decodeResult[1], newPointer);

                        Canvas.SetLeft(newNode.listElement, 785 + 400);
                        Canvas.SetTop(newNode.listElement, 800);
                        newNode.listElement.currentCanvasLeft = 785 + 400;
                        newNode.listElement.currentCanvasTop = 800;

                        newNode.listElement.Show(storyboard, prevCompleteTime);
                        GeneralCanvas.Children.Add(newNode.listElement);

                        Canvas.SetLeft(newPointer, 785 + 400 + 40 - 50);
                        Canvas.SetTop(newPointer, 800 + 40 + 60);
                        newPointer.currentCanvasLeft = 785 + 400 + 40 - 50;
                        newPointer.currentCanvasTop = 800 + 40 + 60;

                        completeTime = newPointer.Show(storyboard, prevCompleteTime);
                        GeneralCanvas.Children.Add(newPointer);

                        break;
                    }

                case "nMoveAbs":
                    {
                        VisualPointer dstPtr = null;
                        generalVisualPtrSet.TryGetValue(decodeResult[1], out dstPtr);

                        double moveAbsLeft = double.Parse(decodeResult[2]);
                        double moveAbsTop = double.Parse(decodeResult[3]);

                        // 移动结点
                        double deltaX = moveAbsLeft - dstPtr.pointingNode.listElement.currentCanvasLeft;
                        double deltaY = moveAbsTop - dstPtr.pointingNode.listElement.currentCanvasTop;
                        completeTime = dstPtr.pointingNode.listElement.Move(storyboard, prevCompleteTime, deltaX, deltaY);

                        // 移动该结点的前驱后继指针
                        Node dstNode = dstPtr.pointingNode;
                        if (dstNode.nextPtr != null)
                        {
                            completeTime = dstNode.nextArrow.MoveBaseAnim(storyboard, prevCompleteTime, moveAbsLeft + 40, moveAbsTop + 40);
                        }

                        if (dstNode.prevPtr != null)
                        {
                            completeTime = dstNode.prevArrow.MoveBaseAnim(storyboard, prevCompleteTime, moveAbsLeft + 40, moveAbsTop + 40);
                        }

                        // 移动该结点相关的通用指针
                        double canvasLeftBias = dstNode.listElement.currentCanvasLeft + 40 - 50;
                        double canvasTopBias = dstNode.listElement.currentCanvasTop + 80 + 20;
                        List<VisualPointer> relatedPointers = dstNode.GetRelatedPointers(generalVisualPtrSet);
                        foreach (VisualPointer visualPointer in relatedPointers)
                        {
                            visualPointer.MoveToAnim(storyboard, prevCompleteTime, canvasLeftBias, canvasTopBias, visualPointer.currentAngle);
                            canvasTopBias += 60;
                        }
                        // 随结点移动指向该结点的指针
                        foreach (Arrow arrow in dstNode.arrowsPointingToMe)
                        {
                            completeTime = arrow.PointingAnim(storyboard, prevCompleteTime, moveAbsLeft + 40, moveAbsTop + 40);
                        }
                        break;
                    }

                case "nMoveRel":
                    {
                        VisualPointer dstPtr = null;
                        generalVisualPtrSet.TryGetValue(decodeResult[1], out dstPtr);

                        double moveAbsLeft = double.Parse(decodeResult[2]) + dstPtr.pointingNode.listElement.currentCanvasLeft;
                        double moveAbsTop = double.Parse(decodeResult[3]) + dstPtr.pointingNode.listElement.currentCanvasTop;

                        // 移动结点
                        double deltaX = moveAbsLeft - dstPtr.pointingNode.listElement.currentCanvasLeft;
                        double deltaY = moveAbsTop - dstPtr.pointingNode.listElement.currentCanvasTop;
                        completeTime = dstPtr.pointingNode.listElement.Move(storyboard, prevCompleteTime, deltaX, deltaY);

                        // 移动该结点的前驱后继指针
                        Node dstNode = dstPtr.pointingNode;
                        if (dstNode.nextPtr != null)
                        {
                            completeTime = dstNode.nextArrow.MoveBaseAnim(storyboard, prevCompleteTime, moveAbsLeft + 40, moveAbsTop + 40);
                        }

                        if (dstNode.prevPtr != null)
                        {
                            completeTime = dstNode.prevArrow.MoveBaseAnim(storyboard, prevCompleteTime, moveAbsLeft + 40, moveAbsTop + 40);
                        }

                        // 移动该结点相关的通用指针
                        double canvasLeftBias = dstNode.listElement.currentCanvasLeft + 40 - 50;
                        double canvasTopBias = dstNode.listElement.currentCanvasTop + 80 + 20;
                        List<VisualPointer> relatedPointers = dstNode.GetRelatedPointers(generalVisualPtrSet);
                        foreach (VisualPointer visualPointer in relatedPointers)
                        {
                            visualPointer.MoveToAnim(storyboard, prevCompleteTime, canvasLeftBias, canvasTopBias, visualPointer.currentAngle);
                            canvasTopBias += 60;
                        }
                        // 随结点移动指向该结点的指针
                        foreach (Arrow arrow in dstNode.arrowsPointingToMe)
                        {
                            completeTime = arrow.PointingAnim(storyboard, prevCompleteTime, moveAbsLeft + 40, moveAbsTop + 40);
                        }
                        break;
                    }
                case "nSetSameNext":
                    {
                        VisualPointer dstPtr = null;
                        generalVisualPtrSet.TryGetValue(decodeResult[1], out dstPtr);
                        VisualPointer srcPtr = null;
                        generalVisualPtrSet.TryGetValue(decodeResult[2], out srcPtr);

                        Node dstNode = dstPtr.pointingNode;
                        // 移动或新建指向srcPtr指向结点的后继结点的箭头
                        if (dstNode.nextPtr == null)
                        {
                            dstNode.nextArrow = new Arrow();
                            Canvas.SetLeft(dstNode.nextArrow, dstNode.listElement.currentCanvasLeft + 40);
                            Canvas.SetTop(dstNode.nextArrow, dstNode.listElement.currentCanvasTop + 40 - 17.5);
                            dstNode.nextArrow.currentCanvasLeft = dstNode.listElement.currentCanvasLeft + 40;
                            dstNode.nextArrow.currentCanvasTop = dstNode.listElement.currentCanvasTop + 40 - 17.5;

                            double targetAngle = Math.Atan2(srcPtr.pointingNode.nextPtr.listElement.currentCanvasTop - dstNode.listElement.currentCanvasTop, srcPtr.pointingNode.nextPtr.listElement.currentCanvasLeft - dstNode.listElement.currentCanvasLeft) / Math.PI * 180;
                            dstNode.nextArrow.Rotation.Angle = targetAngle;
                            dstNode.nextArrow.currentAngle = targetAngle;

                            double targetScaleRate = Math.Sqrt(Math.Pow(dstNode.listElement.currentCanvasTop - srcPtr.pointingNode.nextPtr.listElement.currentCanvasTop, 2) + Math.Pow(dstNode.listElement.currentCanvasLeft - srcPtr.pointingNode.nextPtr.listElement.currentCanvasLeft, 2)) / 190;
                            dstNode.nextArrow.ScaleTrans.ScaleX = targetScaleRate;
                            dstNode.nextArrow.currentScaleX = targetScaleRate;

                            GeneralCanvas.Children.Add(dstNode.nextArrow);
                            completeTime = dstNode.nextArrow.Expand(storyboard, completeTime);
                        }
                        else
                        {
                            completeTime = dstNode.nextArrow.PointingAnim(storyboard, completeTime, srcPtr.pointingNode.nextPtr.listElement.currentCanvasLeft + 40, srcPtr.pointingNode.nextPtr.listElement.currentCanvasTop + 40);
                        }

                        dstNode.nextPtr = srcPtr.pointingNode.nextPtr;
                        break;
                    }

                case "nSetNextPtr":
                    {
                        VisualPointer dstPtr = null;
                        generalVisualPtrSet.TryGetValue(decodeResult[1], out dstPtr);
                        VisualPointer srcPtr = null;
                        generalVisualPtrSet.TryGetValue(decodeResult[2], out srcPtr);

                        completeTime = dstPtr.pointingNode.nextArrow.PointingAnim(storyboard, prevCompleteTime, srcPtr.pointingNode.listElement.currentCanvasLeft + 40, srcPtr.pointingNode.listElement.currentCanvasTop + 40);
                        dstPtr.pointingNode.nextPtr = srcPtr.pointingNode;
                        break;
                    }
                case "Halt":
                    {
                        programCounter = -2;
                        break;
                    }
            }
            return completeTime;
        }

    }
}

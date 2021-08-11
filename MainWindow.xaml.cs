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
using System.IO;

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

            // Insert Panel数组初始化
            insertPanelButtons[0] = InsertHeadSelect;
            insertPanelButtons[1] = InsertMiddleSelect;
            insertPanelButtons[2] = InsertRearSelect;

            // Delete Panel数组初始化
            deletePanelButtons[0] = DeleteHeadSelect;
            deletePanelButtons[1] = DeleteMiddleSelect;
            deletePanelButtons[2] = DeleteRearSelect;
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

            double newCanvasLeft = currentLeft + e.HorizontalChange - prevHoriChange;
            Canvas.SetLeft(ViewboxGeneralCanvas, newCanvasLeft);

            double newCanvasTop = currentTop + e.VerticalChange - prevVerChange;
            Canvas.SetTop(ViewboxGeneralCanvas, newCanvasTop);
            /*
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
            */
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
            storyboardStatus = 0;
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
        private Dictionary<string, int> scalarSet = new Dictionary<string, int>();

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
            if (((Button)sender).MinWidth < 0.5)
            {
                return;
            }
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

                if (currentNewListType == 1)
                {
                    root.nextPtr = root;
                    root.nextArrow = new Arrow();
                    root.arrowsPointingToMe.Add(root.nextArrow);
                }
                root.InitialDrawLinear(GeneralCanvas, storyboard, false, clearDoneTime + 0.2, generalVisualPtrSet);
            }
            else
            {
                ExecuteNextInstruction("gNew Root, 1285, 900", storyboard, clearDoneTime + 0.2);
                generalVisualPtrSet.TryGetValue("Root", out rootPointer);
                if (currentTailSelect == 0)
                {
                    ExecuteNextInstruction("gNew Rear, 1285, 960", storyboard, clearDoneTime + 0.2);
                    generalVisualPtrSet.TryGetValue("Rear", out rearPointer);
                }
                root = null;
                rearNode = null;
            }
            HideNoticeLabelAnim(storyboard, 0);
            storyboard.Completed += new EventHandler(RemoveWidgetsOnCanvas);

            storyboard.Begin();
            scalarSet.Clear();
            ListBackup();
            for (int i = 1; i < 5; ++i)
            {
                toolBarButtons[i].MinWidth = 1;
            }
        }

        private void CreateRandomButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).MinWidth < 0.5)
            {
                return;
            }
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
                ListBackup();
                HideNoticeLabelAnim(storyboard, 0);
                storyboard.Begin();
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
                if (rearNode == root)
                {
                    root.nextPtr = root;
                    root.nextArrow = new Arrow();
                }
                rearNode.nextPtr = root;
                rearNode.nextArrow = new Arrow();
                root.arrowsPointingToMe.Add(rearNode.nextArrow);
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
            HideNoticeLabelAnim(storyboard, 0);
            storyboard.Completed += new EventHandler(RemoveWidgetsOnCanvas);
            storyboard.Begin();
            scalarSet.Clear();
            ListBackup();
            for (int i = 1; i < 5; ++i)
            {
                toolBarButtons[i].MinWidth = 1;
            }
        }

        private void CreateIncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).MinWidth < 0.5)
            {
                return;
            }
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
                ListBackup();
                HideNoticeLabelAnim(storyboard, 0);
                storyboard.Begin();
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
                if (rearNode == root)
                {
                    root.nextPtr = root;
                    root.nextArrow = new Arrow();
                }
                currentPtr.nextPtr = root;
                currentPtr.nextArrow = new Arrow();
                root.arrowsPointingToMe.Add(currentPtr.nextArrow);
                root.InitialDrawRecycle(GeneralCanvas, storyboard, nodeNum + 1 - currentHeadSelect, clearDoneTime + 0.2, generalVisualPtrSet);
            }
            else
            {
                root.InitialDrawLinear(GeneralCanvas, storyboard, currentNewListType == 2, clearDoneTime + 0.2, generalVisualPtrSet);
            }
            HideNoticeLabelAnim(storyboard, 0);
            storyboard.Completed += new EventHandler(RemoveWidgetsOnCanvas);
            storyboard.Begin();
            scalarSet.Clear();
            ListBackup();
            for (i = 1; i < 5; ++i)
            {
                toolBarButtons[i].MinWidth = 1;
            }
        }

        private void CreateDecreaseBuuton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).MinWidth < 0.5)
            {
                return;
            }
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
                ListBackup();
                HideNoticeLabelAnim(storyboard, 0);
                storyboard.Begin();
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
                if (rearNode == root)
                {
                    root.nextPtr = root;
                    root.nextArrow = new Arrow();
                }
                currentPtr.nextPtr = root;
                currentPtr.nextArrow = new Arrow();
                root.arrowsPointingToMe.Add(currentPtr.nextArrow);
                root.InitialDrawRecycle(GeneralCanvas, storyboard, nodeNum + 1 - currentHeadSelect, clearDoneTime + 0.2, generalVisualPtrSet);
            }
            else
            {
                root.InitialDrawLinear(GeneralCanvas, storyboard, currentNewListType == 2, clearDoneTime + 0.2, generalVisualPtrSet);
            }
            HideNoticeLabelAnim(storyboard, 0);
            storyboard.Completed += new EventHandler(RemoveWidgetsOnCanvas);
            storyboard.Begin();
            scalarSet.Clear();
            ListBackup();
            for (i = 1; i < 5; ++i)
            {
                toolBarButtons[i].MinWidth = 1;
            }
        }

        private void NumEditView_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox widget = (TextBox)sender;
            string targetContent = "";
            int currentCursorPos = widget.SelectionStart;
            foreach (char c in widget.Text)
            {
                if (c <= '9' && c >= '0')
                {
                    targetContent += c;
                }
            }
            if (targetContent.Length > 2)
            {
                targetContent = targetContent.Substring(0, 2);
            }
            int diff = targetContent.Length - widget.Text.Length;
            widget.Text = targetContent;
            widget.SelectionStart = currentCursorPos + diff;
        }

        private void NumEditView_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox widget = (TextBox)sender;
            if (widget.Text.Length <= 0)
            {
                widget.Text = "0";
            }
        }

        // Insert Panel
        private int currentInsertPositionSelect = 0;

        private string[] codes = new string[]
        {
                "newNodePtr = new Node(4);",
                "newNodePtr->next = root->next;",
                "root->next = newNodePtr;",
                "return SUCCESS; "
        };

        private string[] instructions = new string[]
        {
            "Exception NO_VALID_INSTRUCTION"
        };
        private readonly Button[] insertPanelButtons = new Button[3];
        private void InsertPanelButton_Click(object sender, RoutedEventArgs e)
        {
            int clickedButtonIndex = 0;
            for (int i = 1; i < 3; ++i)
            {
                if (sender == insertPanelButtons[i])
                {
                    clickedButtonIndex = i;
                }
            }

            if (clickedButtonIndex == currentInsertPositionSelect || ((Button)sender).MinWidth == 0)
            {
                return;
            }

            insertPanelButtons[clickedButtonIndex].MinWidth = 0;
            insertPanelButtons[currentInsertPositionSelect].MinWidth = 1;

            Storyboard storyboard = new Storyboard();
            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;
            // Insert Selection Anim
            DoubleAnimation insertSelectAnim = new DoubleAnimation(currentInsertPositionSelect * 100, clickedButtonIndex * 100, new Duration(TimeSpan.FromMilliseconds(500 + 500 * Math.Abs(clickedButtonIndex - currentInsertPositionSelect))));
            insertSelectAnim.EasingFunction = nonLinearEasingFunction;
            Storyboard.SetTarget(insertSelectAnim, InsertPlaceMask);
            Storyboard.SetTargetProperty(insertSelectAnim, new PropertyPath("(Canvas.Left)"));
            storyboard.Children.Add(insertSelectAnim);

            storyboard.Begin();
            currentInsertPositionSelect = clickedButtonIndex;
        }

        private void InsertConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).MinWidth < 0.5)
            {
                return;
            }
            string targetFileName = Prefix();
            targetFileName += "_insert_";

            switch (currentInsertPositionSelect)
            {
                case 0:
                    targetFileName += "head";
                    break;
                case 1:
                    targetFileName += "middle";
                    break;
                case 2:
                    targetFileName += "rear";
                    break;
            }

            codes = LoadFile("ADL\\Code\\Insert\\" + currentNewListType.ToString() + '\\' + targetFileName + ".cstyle");

            instructions = LoadFile("ADL\\Assemble\\Insert\\" + currentNewListType.ToString() + '\\' + targetFileName + ".asm");
            scalarSet.Clear();

            // Embedded
            for (int i = 0; i < codes.Length; ++i)
            {
                codes[i] = string.Format(codes[i], InsertValueEditView.Text, InsertTargetIdxEditView.Text);
            }

            for (int i = 0; i < instructions.Length; ++i)
            {
                instructions[i] = string.Format(instructions[i], InsertValueEditView.Text, InsertTargetIdxEditView.Text);
            }

            SetCodeAreaText(codes);
            programCounter = 0;
            Storyboard storyboard = new Storyboard();
            HideNoticeLabelAnim(storyboard, 0);
            storyboard.Begin();

            ListBackup();
            PrepareStoryboards(codes, instructions);
            storyboardStatus = 1;
            storyboardList[0].Begin();
        }

        // Delete Panel
        private int currentDeletePositionSelect = 0;
        private readonly Button[] deletePanelButtons = new Button[3];
        private void DeletePanelButton_Click(object sender, RoutedEventArgs e)
        {
            int clickedButtonIndex = 0;
            for (int i = 1; i < 3; ++i)
            {
                if (sender == deletePanelButtons[i])
                {
                    clickedButtonIndex = i;
                }
            }

            if (clickedButtonIndex == currentDeletePositionSelect || ((Button)sender).MinWidth == 0)
            {
                return;
            }

            deletePanelButtons[clickedButtonIndex].MinWidth = 0;
            deletePanelButtons[currentDeletePositionSelect].MinWidth = 1;

            Storyboard storyboard = new Storyboard();
            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;
            // Insert Selection Anim
            DoubleAnimation deleteSelectAnim = new DoubleAnimation(currentDeletePositionSelect * 100, clickedButtonIndex * 100, new Duration(TimeSpan.FromMilliseconds(500 + 500 * Math.Abs(clickedButtonIndex - currentInsertPositionSelect))));
            deleteSelectAnim.EasingFunction = nonLinearEasingFunction;
            Storyboard.SetTarget(deleteSelectAnim, DeletePlaceMask);
            Storyboard.SetTargetProperty(deleteSelectAnim, new PropertyPath("(Canvas.Left)"));
            storyboard.Children.Add(deleteSelectAnim);

            storyboard.Begin();
            currentDeletePositionSelect = clickedButtonIndex;
        }

        private void DeleteConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).MinWidth < 0.5)
            {
                return;
            }
            string targetFileName = Prefix();
            targetFileName += "_delete_";

            switch (currentDeletePositionSelect)
            {
                case 0:
                    targetFileName += "head";
                    break;
                case 1:
                    targetFileName += "middle";
                    break;
                case 2:
                    targetFileName += "rear";
                    break;
            }

            codes = LoadFile("ADL\\Code\\Delete\\" + currentNewListType.ToString() + '\\' + targetFileName + ".cstyle");

            instructions = LoadFile("ADL\\Assemble\\Delete\\" + currentNewListType.ToString() + '\\' + targetFileName + ".asm");
            scalarSet.Clear();

            // Embedded
            for (int i = 0; i < codes.Length; ++i)
            {
                codes[i] = string.Format(codes[i], DeleteIdxEditView.Text);
            }

            for (int i = 0; i < instructions.Length; ++i)
            {
                instructions[i] = string.Format(instructions[i], DeleteIdxEditView.Text);
            }

            SetCodeAreaText(codes);
            programCounter = 0;
            Storyboard storyboard = new Storyboard();
            HideNoticeLabelAnim(storyboard, 0);
            storyboard.Begin();

            ListBackup();
            PrepareStoryboards(codes, instructions);
            storyboardStatus = 1;
            storyboardList[0].Begin();
        }


        // Update Panel
        private void UpdateValue_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).MinWidth < 0.5)
            {
                return;
            }
            string targetFileName = Prefix();
            switch (((Button)sender).Name)
            {
                case "UpdateIncSort":
                    {
                        targetFileName += "_sort_inc";
                        break;
                    }

                case "UpdateValue":
                    {
                        targetFileName += "_update";
                        break;
                    }

                case "UpdateDecSort":
                    {
                        targetFileName += "_sort_dec";
                        break;
                    }
            }

            codes = LoadFile("ADL\\Code\\Update\\" + currentNewListType.ToString() + '\\' + targetFileName + ".cstyle");

            instructions = LoadFile("ADL\\Assemble\\Update\\" + currentNewListType.ToString() + '\\' + targetFileName + ".asm");
            scalarSet.Clear();

            // Embedded
            for (int i = 0; i < codes.Length; ++i)
            {
                codes[i] = string.Format(codes[i], UpdateIdxEditView.Text, UpdateValueEditView.Text);
            }

            for (int i = 0; i < instructions.Length; ++i)
            {
                instructions[i] = string.Format(instructions[i], UpdateIdxEditView.Text, UpdateValueEditView.Text);
            }

            SetCodeAreaText(codes);
            programCounter = 0;
            Storyboard storyboard = new Storyboard();
            HideNoticeLabelAnim(storyboard, 0);
            storyboard.Begin();

            ListBackup();
            PrepareStoryboards(codes, instructions);
            storyboardStatus = 1;
            storyboardList[0].Begin();
        }

        // Query Panel
        private void QueryConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).MinWidth < 0.5)
            {
                return;
            }
            string targetFileName = Prefix();
            targetFileName += "_query";

            codes = LoadFile("ADL\\Code\\Query\\" + currentNewListType.ToString() + '\\' + targetFileName + ".cstyle");

            instructions = LoadFile("ADL\\Assemble\\Query\\" + currentNewListType.ToString() + '\\' + targetFileName + ".asm");
            scalarSet.Clear();

            // Embedded
            for (int i = 0; i < codes.Length; ++i)
            {
                codes[i] = string.Format(codes[i], QueryValueEditView.Text);
            }

            for (int i = 0; i < instructions.Length; ++i)
            {
                instructions[i] = string.Format(instructions[i], QueryValueEditView.Text);
            }

            SetCodeAreaText(codes);
            programCounter = 0;
            Storyboard storyboard = new Storyboard();
            HideNoticeLabelAnim(storyboard, 0);
            storyboard.Begin();

            ListBackup();
            PrepareStoryboards(codes, instructions);
            storyboardStatus = 1;
            storyboardList[0].Begin();
        }

        private List<int> backupList = null;
        private void ListBackup()
        {
            backupList = new List<int>();
            Node currentNode = root;
            while (currentNode != null)
            {
                if (currentNode == root && backupList.Count > 0)
                {
                    break;
                }
                backupList.Add(currentNode.value);
                currentNode = currentNode.nextPtr;
            }
        }

        private void ResumeFromBackup()
        {
            if (GeneralCanvas.Children.Count <= 1)
            {
                return;
            }
            generalVisualPtrSet.Clear();
            if (backupList.Count == 0)
            {
                generalVisualPtrSet.Clear();
                Storyboard storyboard1 = new Storyboard();
                ExecuteNextInstruction("gNew Root, 1285, 900", storyboard1, 0);
                generalVisualPtrSet.TryGetValue("Root", out rootPointer);
                if (currentTailSelect == 0)
                {
                    ExecuteNextInstruction("gNew Rear, 1285, 960", storyboard1, 0);
                    generalVisualPtrSet.TryGetValue("Rear", out rearPointer);
                }
                storyboard1.Begin();
                return;
            }
            bool isBidirection = currentNewListType == 2;
            root = new Node(backupList[0], 155, 155, 155, null);
            rootPointer = new VisualPointer("Root", root);

            Node currentPtr = root;
            for (int i = 1; i < backupList.Count; ++i)
            {
                currentPtr.CreateNextNode(backupList[i], isBidirection);
                currentPtr = currentPtr.nextPtr;
            }
            rearPointer = new VisualPointer("Rear", currentPtr);
            generalVisualPtrSet.Add("Root", rootPointer);
            if (currentTailSelect == 0)
            {
                generalVisualPtrSet.Add("Rear", rearPointer);
            }

            Storyboard storyboard = new Storyboard();
            if (currentNewListType == 1)
            {
                currentPtr.nextPtr = root;
                currentPtr.nextArrow = new Arrow();
                root.arrowsPointingToMe.Add(currentPtr.nextArrow);
                root.InitialDrawRecycle(GeneralCanvas, storyboard, backupList.Count, 0, generalVisualPtrSet);
            }
            else
            {
                root.InitialDrawLinear(GeneralCanvas, storyboard, isBidirection, 0, generalVisualPtrSet);
            }
            storyboard.Begin();

            EnableDisableAllPanelButtons(1);
        }

        private int storyboardStatus = 0;
        private List<Storyboard> storyboardList = new List<Storyboard>();
        private int storyboardListIdx = 0;
        private bool inException = false;
        private void PrepareStoryboards(string[] codes, string[] instructions)
        {
            storyboardList.Clear();
            scalarSet.Clear();
            programCounter = 0;
            storyboardListIdx = 0;
            inException = false;
            ForwardButton.MinWidth = 1;
            ForwardEndButton.MinWidth = 1;
            Storyboard newStoryboard = null;
            double completeTime = 0;

            while (programCounter >= 0)
            {
                if (Decode(instructions[programCounter])[0] == "aLine")
                {
                    if (newStoryboard != null)
                    {
                        newStoryboard.Completed += new EventHandler(BeginNextStoryboard);
                        storyboardList.Add(newStoryboard);
                    }
                    newStoryboard = new Storyboard();
                    completeTime = 0;
                }
                completeTime = ExecuteNextInstruction(instructions[programCounter], newStoryboard, completeTime);
                ++programCounter;
            }
            if (newStoryboard != null)
            {
                newStoryboard.Completed += new EventHandler(BeginNextStoryboard);
                storyboardList.Add(newStoryboard);
            }
            EnableDisableAllPanelButtons(0);
        }

        private void BeginNextStoryboard(object sender, EventArgs e)
        {
            if (storyboardStatus == 1 && storyboardListIdx < storyboardList.Count - 1)
            {
                ++storyboardListIdx;
                storyboardList[storyboardListIdx].Begin();
            }
            else if (storyboardListIdx == storyboardList.Count - 1)
            {
                ForwardButton.MinWidth = 0.4;
                ForwardEndButton.MinWidth = 0.4;
                if (!inException)
                {
                    EnableDisableAllPanelButtons(1);
                }
            }
        }


        private string[] LoadFile(string filePath)
        {
            List<string> tempList = new List<string>();
            try
            {
                FileStream fileInStream = new FileStream(filePath, FileMode.Open);
                long fileLength = fileInStream.Length;
                byte[] bufferByte = new byte[fileLength];
                fileInStream.Read(bufferByte, 0, (int)fileLength);
                fileInStream.Close();
                string buffer = System.Text.Encoding.ASCII.GetString(bufferByte);

                string tempStr = "";
                for (int i = 0; i < buffer.Length; ++i)
                {
                    if (buffer[i] != '\n' && buffer[i] != '\r')
                    {
                        tempStr += buffer[i];
                    }
                    else
                    {
                        if (tempStr.Length > 0)
                        {
                            tempList.Add(tempStr);
                            tempStr = "";
                        }
                    }
                }
                if (tempStr.Length > 0)
                {
                    tempList.Add(tempStr);
                }
            } catch (IOException e)
            {
                Console.WriteLine(e.ToString());
            }

            return tempList.ToArray();
        }

        private string Prefix()
        {
            string prefix = "";
            switch (currentNewListType)
            {
                case 0:
                    prefix += 'S';
                    break;
                case 1:
                    prefix += 'R';
                    break;
                case 2:
                    prefix += 'B';
                    break;
            }

            if (currentHeadSelect == 1)
            {
                prefix += '~';
            }
            prefix += 'H';

            if (currentTailSelect == 1)
            {
                prefix += '~';
            }
            prefix += "R";
            return prefix;
        }

        private double LineMaskMove(Storyboard storyboard, double prevCompleteTime, string[] codes, int targetLine)
        {
            if (currentLine == targetLine && CodeAreaMask.Width == codes[targetLine].Length * 9 + 20)
            {
                return prevCompleteTime;
            }
            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;

            double animTime = 0.5 + 0.1 * Math.Abs(targetLine - currentLine);
            DoubleAnimation lengthAnim = new DoubleAnimation(codes[targetLine].Length * 9 + 20, new Duration(TimeSpan.FromSeconds(animTime)));
            lengthAnim.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);
            lengthAnim.EasingFunction = nonLinearEasingFunction;
            Storyboard.SetTarget(lengthAnim, CodeAreaMask);
            Storyboard.SetTargetProperty(lengthAnim, new PropertyPath("Width"));
            storyboard.Children.Add(lengthAnim);

            DoubleAnimation shiftAnim = new DoubleAnimation(currentLine * 36 - 3, targetLine * 36 - 3, new Duration(TimeSpan.FromSeconds(animTime)));
            shiftAnim.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);
            shiftAnim.EasingFunction = nonLinearEasingFunction;
            Storyboard.SetTarget(shiftAnim, CodeAreaMask);
            Storyboard.SetTargetProperty(shiftAnim, new PropertyPath("(Canvas.Top)"));
            storyboard.Children.Add(shiftAnim);
            currentLine = targetLine;

            return prevCompleteTime + animTime;
        }

        private void SetCodeAreaText(string[] codes)
        {
            CodeAreaText.Text = "";
            foreach (string str in codes)
            {
                CodeAreaText.Text += str + '\n';
            }
        }

        private double SetNoticeLabelAnim(Storyboard storyboard, double prevCompleteTime, string content)
        {
            StringAnimationUsingKeyFrames changeAnim = new StringAnimationUsingKeyFrames();
            DiscreteStringKeyFrame changeBeforeFrame = new DiscreteStringKeyFrame((string)NoticeLabel.Content, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0)));
            DiscreteStringKeyFrame changeAfterFrame = new DiscreteStringKeyFrame(content, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.1)));
            changeAnim.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);
            changeAnim.KeyFrames.Add(changeBeforeFrame);
            changeAnim.KeyFrames.Add(changeAfterFrame);
            Storyboard.SetTarget(changeAnim, NoticeLabel);
            Storyboard.SetTargetProperty(changeAnim, new PropertyPath("Content"));

            storyboard.Children.Add(changeAnim);

            DoubleAnimation opacityAnim = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0.4)))
            {
                EasingFunction = new ExponentialEase()
                {
                    EasingMode = EasingMode.EaseOut
                },
                BeginTime = TimeSpan.FromSeconds(prevCompleteTime)
            };
            Storyboard.SetTarget(opacityAnim, NoticeLabel);
            Storyboard.SetTargetProperty(opacityAnim, new PropertyPath("Opacity"));
            storyboard.Children.Add(opacityAnim);

            return prevCompleteTime + 0.4;
        }

        private double HideNoticeLabelAnim(Storyboard storyboard, double prevCompleteTime)
        {
            if (!GeneralCanvas.Children.Contains(NoticeLabel))
            {
                GeneralCanvas.Children.Add(NoticeLabel);
            }
            DoubleAnimation opacityAnim = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.4)))
            {
                EasingFunction = new ExponentialEase()
                {
                    EasingMode = EasingMode.EaseOut
                },
                BeginTime = TimeSpan.FromSeconds(prevCompleteTime)
            };
            Storyboard.SetTarget(opacityAnim, NoticeLabel);
            Storyboard.SetTargetProperty(opacityAnim, new PropertyPath("Opacity"));
            storyboard.Children.Add(opacityAnim);

            return prevCompleteTime + 0.4;
        }

        private void EnableDisableAllPanelButtons(double targetEnable)
        {
            // New panel
            CreateEmptyButton.MinWidth = targetEnable;
            CreateRandomButton.MinWidth = targetEnable;
            CreateIncreaseButton.MinWidth = targetEnable;
            CreateDecreaseBuuton.MinWidth = targetEnable;

            // Insert panel
            InsertConfirmButton.MinWidth = targetEnable;

            // Delete panel
            DeleteConfirmButton.MinWidth = targetEnable;

            // Update panel
            UpdateIncSort.MinWidth = targetEnable;
            UpdateValue.MinWidth = targetEnable;
            UpdateDecSort.MinWidth = targetEnable;

            // Query panel
            QueryConfirm.MinWidth = targetEnable;
            LoadADLButton.MinWidth = targetEnable;
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
        private int currentLine = 0;

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

                        if (decodeResult.Length > 2)
                        {
                            double canvasLeft = double.Parse(decodeResult[2]);
                            double canvasTop = double.Parse(decodeResult[3]);

                            Canvas.SetLeft(visualPointer, canvasLeft);
                            Canvas.SetTop(visualPointer, canvasTop);
                            visualPointer.currentCanvasLeft = canvasLeft;
                            visualPointer.currentCanvasTop = canvasTop;
                        }
                        else
                        {
                            Canvas.SetLeft(visualPointer, 785 + 400);
                            Canvas.SetTop(visualPointer, 800);
                            visualPointer.currentCanvasLeft = 785 + 400;
                            visualPointer.currentCanvasTop = 800;
                        }
                        

                        GeneralCanvas.Children.Add(visualPointer);
                        completeTime = visualPointer.Show(storyboard, prevCompleteTime);
                        break;
                    }
                    
                case "gMove":
                    {
                        generalVisualPtrSet.TryGetValue(decodeResult[1], out VisualPointer dstPtr);
                        Node dstNode = dstPtr.pointingNode;

                        Node srcNode = null;
                        if (!decodeResult[2].Equals("null"))
                        {
                            generalVisualPtrSet.TryGetValue(decodeResult[2], out VisualPointer srcPtr);
                            srcNode = srcPtr.pointingNode;
                        }

                        dstPtr.pointingNode = srcNode;
                        // 不可见通用指针
                        if (dstPtr.Opacity < 0.1)
                        {
                            break;
                        }

                        if (srcNode == null)
                        {
                            completeTime = dstPtr.SetNullAnim(storyboard, prevCompleteTime, true);
                            completeTime = dstPtr.MoveToAnim(storyboard, prevCompleteTime, dstPtr.currentCanvasLeft, dstPtr.currentCanvasTop + 150, dstPtr.currentAngle);
                            break;
                        }
                        else
                        {
                            completeTime = dstPtr.SetNullAnim(storyboard, prevCompleteTime, false);
                        }
                        List<VisualPointer> srcRelatedPtrs = srcNode.GetRelatedPointers(generalVisualPtrSet);
                        double canvasLeftBias = srcNode.listElement.currentCanvasLeft + 40;
                        double canvasTopBias = srcNode.listElement.currentCanvasTop + 40;
                        double radius = 80;
                        foreach (VisualPointer visualPointer in srcRelatedPtrs)
                        {
                            if (visualPointer.Opacity > 0.1)
                            {
                                if (currentNewListType != 1)
                                {
                                    completeTime = visualPointer.MoveToAnim(storyboard, prevCompleteTime, canvasLeftBias - 50,
                                                                        canvasTopBias + radius - 20, 0);
                                }
                                else
                                {
                                    completeTime = visualPointer.MoveToAnim(storyboard, prevCompleteTime, canvasLeftBias + radius * Math.Sin(srcNode.vpAngle / 180 * Math.PI) - 50,
                                                                        canvasTopBias - radius * Math.Cos(srcNode.vpAngle / 180 * Math.PI) - 20, srcNode.vpAngle);
                                }
                                radius += 60;
                            }
                        }

                        if (dstNode == null)
                        {
                            break;
                        }
                        canvasLeftBias = dstNode.listElement.currentCanvasLeft + 40;
                        canvasTopBias = dstNode.listElement.currentCanvasTop + 40;
                        radius = 80;
                        List<VisualPointer> dstRelatedPtrs = dstNode.GetRelatedPointers(generalVisualPtrSet);
                        foreach (VisualPointer visualPointer in dstRelatedPtrs)
                        {
                            if (visualPointer.Opacity > 0.1)
                            {
                                if (currentNewListType != 1)
                                {
                                    completeTime = visualPointer.MoveToAnim(storyboard, prevCompleteTime, canvasLeftBias - 50,
                                                                        canvasTopBias + radius - 20, 0);
                                }
                                else
                                {
                                    completeTime = visualPointer.MoveToAnim(storyboard, prevCompleteTime, canvasLeftBias + radius * Math.Sin(dstNode.vpAngle / 180 * Math.PI) - 50,
                                                                        canvasTopBias - radius * Math.Cos(dstNode.vpAngle / 180 * Math.PI) - 20, dstNode.vpAngle);
                                }
                                radius += 60;
                            }
                        }
                        break;
                    }
                    
                case "gDelete":
                    {
                        generalVisualPtrSet.TryGetValue(decodeResult[1], out VisualPointer freeDstPtr);

                        // 不可见通用指针
                        if (freeDstPtr.Opacity < 0.1)
                        {
                            generalVisualPtrSet.Remove(decodeResult[1]);
                            break;
                        }
                        completeTime = freeDstPtr.Close(storyboard, prevCompleteTime);

                        if (freeDstPtr.pointingNode == null)
                        {
                            generalVisualPtrSet.Remove(decodeResult[1]);
                            break;
                        }
                        List<VisualPointer> freeDstRelatedPtrs = freeDstPtr.pointingNode.GetRelatedPointers(generalVisualPtrSet);


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

                case "gNewVPtr":
                    {
                        VisualPointer visualPointer = new VisualPointer(decodeResult[1], null);
                        generalVisualPtrSet.Add(decodeResult[1], visualPointer);
                        Canvas.SetLeft(visualPointer, 0);
                        Canvas.SetTop(visualPointer, 0);
                        visualPointer.currentCanvasLeft = 0;
                        visualPointer.currentCanvasTop = 0;
                        visualPointer.Opacity = 0;

                        GeneralCanvas.Children.Add(visualPointer);
                        break;
                    }

                case "gMoveNext":
                    {
                        generalVisualPtrSet.TryGetValue(decodeResult[1], out VisualPointer dstPtr);
                        generalVisualPtrSet.TryGetValue(decodeResult[2], out VisualPointer srcPtr);
                        
                        Node dstNode = dstPtr.pointingNode;
                        Node srcNode = srcPtr.pointingNode.nextPtr;
                        dstPtr.pointingNode = srcPtr.pointingNode.nextPtr;
                        if (dstPtr.Opacity < 0.1)
                        {
                            break;
                        }

                        if (srcNode == null)
                        {
                            completeTime = dstPtr.SetNullAnim(storyboard, prevCompleteTime, true);
                            completeTime = dstPtr.MoveToAnim(storyboard, prevCompleteTime, dstPtr.currentCanvasLeft, dstPtr.currentCanvasTop + 150, dstPtr.currentAngle);
                            break;
                        }
                        else
                        {
                            completeTime = dstPtr.SetNullAnim(storyboard, prevCompleteTime, false);
                        }
                        List<VisualPointer> srcRelated = srcNode.GetRelatedPointers(generalVisualPtrSet);

                        // srcNode part
                        double canvasLeftBias = srcNode.listElement.currentCanvasLeft + 40;
                        double canvasTopBias = srcNode.listElement.currentCanvasTop + 40;
                        double radius = 80;

                        foreach (VisualPointer visualPointer in srcRelated)
                        {
                            if (visualPointer.Opacity > 0.1)
                            {
                                if (currentNewListType != 1)
                                {
                                    completeTime = visualPointer.MoveToAnim(storyboard, prevCompleteTime, canvasLeftBias - 50,
                                                                        canvasTopBias + radius - 20, 0);
                                }
                                else
                                {
                                    completeTime = visualPointer.MoveToAnim(storyboard, prevCompleteTime, canvasLeftBias + radius * Math.Sin(srcNode.vpAngle / 180 * Math.PI) - 50,
                                                                        canvasTopBias - radius * Math.Cos(srcNode.vpAngle / 180 * Math.PI) - 20, srcNode.vpAngle);
                                }
                                radius += 60;
                            }
                        }

                        // dstNode part
                        if (dstNode == null)
                        {
                            break;
                        }
                        List<VisualPointer> dstRelated = dstNode.GetRelatedPointers(generalVisualPtrSet);
                        if (dstNode == null)
                        {
                            break;
                        }
                        canvasLeftBias = dstNode.listElement.currentCanvasLeft + 40;
                        canvasTopBias = dstNode.listElement.currentCanvasTop + 40;
                        radius = 80;

                        foreach (VisualPointer visualPointer in dstRelated)
                        {
                            if (visualPointer.Opacity > 0.1)
                            {
                                if (currentNewListType != 1)
                                {
                                    completeTime = visualPointer.MoveToAnim(storyboard, prevCompleteTime, canvasLeftBias - 50,
                                                                        canvasTopBias + radius - 20, 0);
                                }
                                else
                                {
                                    completeTime = visualPointer.MoveToAnim(storyboard, prevCompleteTime, canvasLeftBias + radius * Math.Sin(dstNode.vpAngle / 180 * Math.PI) - 50,
                                                                        canvasTopBias - radius * Math.Cos(dstNode.vpAngle / 180 * Math.PI) - 20, dstNode.vpAngle);
                                }
                                
                                radius += 60;
                            }
                        }
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

                case "nSetValue":
                    {
                        generalVisualPtrSet.TryGetValue(decodeResult[1], out VisualPointer dstPtr);

                        completeTime = dstPtr.pointingNode.listElement.SetPropertyAnim(storyboard, prevCompleteTime, int.Parse(decodeResult[2]));
                        dstPtr.pointingNode.value = int.Parse(decodeResult[2]);
                        break;
                    }

                case "nMoveAbs":
                    {
                        generalVisualPtrSet.TryGetValue(decodeResult[1], out VisualPointer dstPtr);

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
                            completeTime = Node.SetArrowAnim(storyboard, dstNode, dstNode.nextPtr, dstNode.nextArrow, prevCompleteTime);
                            //completeTime = dstNode.nextArrow.MoveBaseAnim(storyboard, prevCompleteTime, moveAbsLeft + 40, moveAbsTop + 40);
                        }

                        if (dstNode.prevPtr != null)
                        {
                            completeTime = Node.SetArrowAnim(storyboard, dstNode, dstNode.prevPtr, dstNode.prevArrow, prevCompleteTime);
                            //completeTime = dstNode.prevArrow.MoveBaseAnim(storyboard, prevCompleteTime, moveAbsLeft + 40, moveAbsTop + 40);
                        }

                        // 移动该结点相关的通用指针
                        double canvasLeftBias = dstNode.listElement.currentCanvasLeft + 40 - 50;
                        double canvasTopBias = dstNode.listElement.currentCanvasTop + 80 + 20;
                        List<VisualPointer> relatedPointers = dstNode.GetRelatedPointers(generalVisualPtrSet);
                        foreach (VisualPointer visualPointer in relatedPointers)
                        {
                            if (visualPointer.Opacity > 0.1)
                            {
                                visualPointer.MoveToAnim(storyboard, prevCompleteTime, canvasLeftBias, canvasTopBias, visualPointer.currentAngle);
                                canvasTopBias += 60;
                            }
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
                        generalVisualPtrSet.TryGetValue(decodeResult[1], out VisualPointer dstPtr);
                        generalVisualPtrSet.TryGetValue(decodeResult[2], out VisualPointer srcPtr);

                        double moveAbsLeft = double.Parse(decodeResult[3]) + srcPtr.pointingNode.listElement.currentCanvasLeft;
                        double moveAbsTop = double.Parse(decodeResult[4]) + srcPtr.pointingNode.listElement.currentCanvasTop;

                        // 移动结点
                        double deltaX = moveAbsLeft - dstPtr.pointingNode.listElement.currentCanvasLeft;
                        double deltaY = moveAbsTop - dstPtr.pointingNode.listElement.currentCanvasTop;
                        completeTime = dstPtr.pointingNode.listElement.Move(storyboard, prevCompleteTime, deltaX, deltaY);

                        // 移动该结点的前驱后继指针
                        Node dstNode = dstPtr.pointingNode;
                        if (dstNode.nextPtr != null)
                        {
                            completeTime = Node.SetArrowAnim(storyboard, dstNode, dstNode.nextPtr, dstNode.nextArrow, prevCompleteTime);
                            //completeTime = dstNode.nextArrow.MoveBaseAnim(storyboard, prevCompleteTime, moveAbsLeft + 40, moveAbsTop + 40);
                        }

                        if (dstNode.prevPtr != null)
                        {
                            completeTime = Node.SetArrowAnim(storyboard, dstNode, dstNode.prevPtr, dstNode.prevArrow, prevCompleteTime);
                            //completeTime = dstNode.prevArrow.MoveBaseAnim(storyboard, prevCompleteTime, moveAbsLeft + 40, moveAbsTop + 40);
                        }

                        // 移动该结点相关的通用指针
                        double canvasLeftBias = dstNode.listElement.currentCanvasLeft + 40 - 50;
                        double canvasTopBias = dstNode.listElement.currentCanvasTop + 80 + 20;
                        List<VisualPointer> relatedPointers = dstNode.GetRelatedPointers(generalVisualPtrSet);
                        foreach (VisualPointer visualPointer in relatedPointers)
                        {
                            if (visualPointer.Opacity > 0.1)
                            {
                                visualPointer.MoveToAnim(storyboard, prevCompleteTime, canvasLeftBias, canvasTopBias, visualPointer.currentAngle);
                                canvasTopBias += 60;
                            }
                        }
                        // 随结点移动指向该结点的指针
                        foreach (Arrow arrow in dstNode.arrowsPointingToMe)
                        {
                            completeTime = arrow.PointingAnim(storyboard, prevCompleteTime, moveAbsLeft + 40, moveAbsTop + 40);
                        }
                        break;
                    }

                case "nMoveRelOut":
                    {
                        generalVisualPtrSet.TryGetValue(decodeResult[1], out VisualPointer dstPtr);
                        generalVisualPtrSet.TryGetValue(decodeResult[2], out VisualPointer srcPtr);

                        double radius = double.Parse(decodeResult[3]);
                        double moveAbsLeft = srcPtr.pointingNode.listElement.currentCanvasLeft + radius * Math.Sin(srcPtr.pointingNode.vpAngle * Math.PI / 180);
                        double moveAbsTop = srcPtr.pointingNode.listElement.currentCanvasTop - radius * Math.Cos(srcPtr.pointingNode.vpAngle * Math.PI / 180);

                        // 移动结点
                        double deltaX = moveAbsLeft - dstPtr.pointingNode.listElement.currentCanvasLeft;
                        double deltaY = moveAbsTop - dstPtr.pointingNode.listElement.currentCanvasTop;
                        completeTime = dstPtr.pointingNode.listElement.Move(storyboard, prevCompleteTime, deltaX, deltaY);

                        // 移动该结点的前驱后继指针
                        Node dstNode = dstPtr.pointingNode;
                        if (dstNode.nextPtr != null)
                        {
                            completeTime = Node.SetArrowAnim(storyboard, dstNode, dstNode.nextPtr, dstNode.nextArrow, prevCompleteTime);
                            //completeTime = dstNode.nextArrow.MoveBaseAnim(storyboard, prevCompleteTime, moveAbsLeft + 40, moveAbsTop + 40);
                        }

                        if (dstNode.prevPtr != null)
                        {
                            completeTime = Node.SetArrowAnim(storyboard, dstNode, dstNode.prevPtr, dstNode.prevArrow, prevCompleteTime);
                            //completeTime = dstNode.prevArrow.MoveBaseAnim(storyboard, prevCompleteTime, moveAbsLeft + 40, moveAbsTop + 40);
                        }

                        // 移动该结点相关的通用指针
                        double canvasLeftBias = dstNode.listElement.currentCanvasLeft + 40;
                        double canvasTopBias = dstNode.listElement.currentCanvasTop + 40;
                        radius = 80;
                        List<VisualPointer> relatedPointers = dstNode.GetRelatedPointers(generalVisualPtrSet);
                        foreach (VisualPointer visualPointer in relatedPointers)
                        {
                            if (visualPointer.Opacity > 0.1)
                            {
                                if (dstNode.vpAngle < 0.1 && dstNode.vpAngle > -0.1)
                                {
                                    completeTime = visualPointer.MoveToAnim(storyboard, prevCompleteTime, canvasLeftBias - 50,
                                                                        canvasTopBias + radius - 20, 0);
                                }
                                else
                                {
                                    completeTime = visualPointer.MoveToAnim(storyboard, prevCompleteTime, canvasLeftBias + radius * Math.Sin(dstNode.vpAngle / 180 * Math.PI) - 50,
                                                                        canvasTopBias - radius * Math.Cos(dstNode.vpAngle / 180 * Math.PI) - 20, dstNode.vpAngle);
                                }
                                radius += 60;
                            }
                        }
                        // 随结点移动指向该结点的指针
                        foreach (Arrow arrow in dstNode.arrowsPointingToMe)
                        {
                            completeTime = arrow.PointingAnim(storyboard, prevCompleteTime, moveAbsLeft + 40, moveAbsTop + 40);
                        }
                        break;
                    }

                case "nDelete":
                    {
                        generalVisualPtrSet.TryGetValue(decodeResult[1], out VisualPointer dstPtr);

                        Node dstNode = dstPtr.pointingNode;

                        completeTime = dstNode.listElement.Close(storyboard, prevCompleteTime);

                        /*
                        // next pointer
                        if (dstNode.nextPtr != null)
                        {
                            dstNode.nextPtr.arrowsPointingToMe.Remove(dstNode.nextArrow);
                            completeTime = dstNode.nextArrow.Close(storyboard, prevCompleteTime);
                            dstNode.nextPtr = null;
                            dstNode.nextArrow = null;
                        }

                        // prev pointer
                        if (dstNode.prevPtr != null)
                        {
                            completeTime = dstNode.prevArrow.Close(storyboard, prevCompleteTime);
                            dstNode.prevPtr = null;
                            dstNode.prevArrow = null;
                        }
                        */
                        break;
                    }

                case "pSetNext":
                    {
                        generalVisualPtrSet.TryGetValue(decodeResult[1], out VisualPointer dstPtr);
                        generalVisualPtrSet.TryGetValue(decodeResult[2], out VisualPointer srcPtr);

                        Node dstNode = dstPtr.pointingNode;
                        // 移动或新建指向srcPtr指向结点的后继结点的箭头
                        if (srcPtr.pointingNode != null)
                        {
                            if (dstNode.nextPtr == null)
                            {
                                dstNode.nextArrow = new Arrow();
                                Canvas.SetLeft(dstNode.nextArrow, dstNode.listElement.currentCanvasLeft + 40);
                                Canvas.SetTop(dstNode.nextArrow, dstNode.listElement.currentCanvasTop + 40 - 17.5);
                                dstNode.nextArrow.currentCanvasLeft = dstNode.listElement.currentCanvasLeft + 40;
                                dstNode.nextArrow.currentCanvasTop = dstNode.listElement.currentCanvasTop + 40 - 17.5;

                                double targetAngle = Math.Atan2(srcPtr.pointingNode.listElement.currentCanvasTop - dstNode.listElement.currentCanvasTop, srcPtr.pointingNode.listElement.currentCanvasLeft - dstNode.listElement.currentCanvasLeft) / Math.PI * 180;
                                dstNode.nextArrow.Rotation.Angle = targetAngle;
                                dstNode.nextArrow.currentAngle = targetAngle;

                                double targetScaleRate = Math.Sqrt(Math.Pow(dstNode.listElement.currentCanvasTop - srcPtr.pointingNode.listElement.currentCanvasTop, 2) + Math.Pow(dstNode.listElement.currentCanvasLeft - srcPtr.pointingNode.listElement.currentCanvasLeft, 2)) / 190;
                                dstNode.nextArrow.ScaleTrans.ScaleX = targetScaleRate;
                                dstNode.nextArrow.currentScaleX = targetScaleRate;

                                GeneralCanvas.Children.Add(dstNode.nextArrow);
                                completeTime = dstNode.nextArrow.Expand(storyboard, prevCompleteTime);
                            }
                            else
                            {
                                completeTime = dstNode.nextArrow.PointingAnim(storyboard, prevCompleteTime, srcPtr.pointingNode.listElement.currentCanvasLeft + 40, srcPtr.pointingNode.listElement.currentCanvasTop + 40);
                                dstNode.nextPtr.arrowsPointingToMe.Remove(dstNode.nextArrow);
                            }
                            srcPtr.pointingNode.arrowsPointingToMe.Add(dstPtr.pointingNode.nextArrow);
                        }
                        else
                        {
                            // 将next指针指向null
                            if (dstNode.nextArrow != null)
                            {
                                completeTime = dstNode.nextArrow.Close(storyboard, prevCompleteTime);
                                dstNode.nextArrow = null;
                            }
                        }

                        dstNode.nextPtr = srcPtr.pointingNode;
                        break;
                    }

                case "pDeleteNext":
                    {
                        generalVisualPtrSet.TryGetValue(decodeResult[1], out VisualPointer dstPtr);

                        if (dstPtr.pointingNode.nextPtr != null)
                        {
                            dstPtr.pointingNode.nextPtr.arrowsPointingToMe.Remove(dstPtr.pointingNode.nextArrow);
                            completeTime = dstPtr.pointingNode.nextArrow.Close(storyboard, prevCompleteTime);
                            dstPtr.pointingNode.nextPtr = null;
                            dstPtr.pointingNode.nextArrow = null;
                        }
                        break;
                    }

                case "aStd":
                    {
                        root = rootPointer.pointingNode;
                        if (root == null)
                        {
                            completeTime = rootPointer.MoveToAnim(storyboard, prevCompleteTime, rootPointer.currentCanvasLeft, rootPointer.currentCanvasTop, 0);
                            break;
                        }
                        if (currentNewListType == 1)
                        {
                            // 循环链表
                            int elementNum = 0;
                            if (root != null)
                            {
                                Node currentNode = root;
                                elementNum = 1;
                                while (currentNode.nextPtr != root)
                                {
                                    ++elementNum;
                                    currentNode = currentNode.nextPtr;
                                }
                            }
                            

                            double singleHalfAngle = Math.PI / elementNum;
                            double radius = 95 / Math.Sin(singleHalfAngle);
                            double canvasLeftBias = 785 + 500 + radius;
                            double canvasTopBias = 200 + 400 + radius;

                            if (elementNum == 1)
                            {
                                canvasLeftBias = 785 + 500;
                                canvasTopBias = 200 + 400;

                                root.listElement.Move(storyboard, prevCompleteTime, -40 + canvasLeftBias - root.listElement.currentCanvasLeft, -40 + canvasTopBias - root.listElement.currentCanvasTop);
                                Node.SetArrowAnim(storyboard, root, root, root.nextArrow, prevCompleteTime);
                                completeTime = VisualPointer.MovePointersInNodeAnim(root, storyboard, generalVisualPtrSet, prevCompleteTime);
                                break;
                            }


                            Point[] point = new Point[elementNum];
                            for (int i = 0; i < elementNum; ++i)
                            {
                                point[i].X = radius * Math.Cos(singleHalfAngle * 2 * i);
                                point[i].Y = radius * Math.Sin(singleHalfAngle * 2 * i);
                            }

                            Node currentPtr = root.nextPtr;
                            Node prevCurrentPtr = root;
                            root.listElement.Move(storyboard, prevCompleteTime, point[0].X - 40 + canvasLeftBias - root.listElement.currentCanvasLeft, point[0].Y - 40 + canvasTopBias - root.listElement.currentCanvasTop);

                            completeTime = VisualPointer.RecycleMovePointersInNodeAnim(root, storyboard, generalVisualPtrSet, prevCompleteTime, canvasLeftBias, canvasTopBias);
                            for (int i = 1; i < elementNum; ++i)
                            {
                                completeTime = currentPtr.listElement.Move(storyboard, prevCompleteTime, 
                                                                            point[i].X - 40 + canvasLeftBias - currentPtr.listElement.currentCanvasLeft,
                                                                            point[i].Y - 40 + canvasTopBias - currentPtr.listElement.currentCanvasTop);

                                VisualPointer.RecycleMovePointersInNodeAnim(currentPtr, storyboard, generalVisualPtrSet, prevCompleteTime, canvasLeftBias, canvasTopBias);

                                completeTime = Node.SetArrowAnim(storyboard, prevCurrentPtr, currentPtr, prevCurrentPtr.nextArrow, prevCompleteTime);

                                prevCurrentPtr = currentPtr;
                                currentPtr = currentPtr.nextPtr;
                            }

                            completeTime = Node.SetArrowAnim(storyboard, prevCurrentPtr, root, prevCurrentPtr.nextArrow, prevCompleteTime);
                            
                        }
                        else
                        {
                            if (root.listElement == null)
                            {
                                break;
                            }
                            double canvasLeft = 785 + 500;
                            double canvasTop = 200 + 400;
                            Node currentPtr = root;

                            currentPtr.listElement.Move(storyboard, prevCompleteTime, canvasLeft - currentPtr.listElement.currentCanvasLeft, canvasTop - currentPtr.listElement.currentCanvasTop);

                            completeTime = VisualPointer.MovePointersInNodeAnim(currentPtr, storyboard, generalVisualPtrSet, prevCompleteTime);
                            canvasLeft += 190;

                            while (currentPtr.nextPtr != null)
                            {
                                // 循环链表尾结点处理
                                if (currentPtr.nextPtr == root)
                                {
                                    Node.SetArrowAnim(storyboard, currentPtr, currentPtr.nextPtr, currentPtr.nextArrow, prevCompleteTime);
                                    break;
                                }
                                currentPtr.nextPtr.listElement.Move(storyboard, prevCompleteTime, canvasLeft - currentPtr.nextPtr.listElement.currentCanvasLeft, canvasTop - currentPtr.nextPtr.listElement.currentCanvasTop);

                                VisualPointer.MovePointersInNodeAnim(currentPtr.nextPtr, storyboard, generalVisualPtrSet, prevCompleteTime);

                                completeTime = Node.SetArrowAnim(storyboard, currentPtr, currentPtr.nextPtr, currentPtr.nextArrow, prevCompleteTime);


                                if (currentPtr.prevPtr != null)
                                {
                                    completeTime = Node.SetArrowAnim(storyboard, currentPtr, currentPtr.prevPtr, currentPtr.prevArrow, prevCompleteTime);
                                }
                                canvasLeft += 190;
                                currentPtr = currentPtr.nextPtr;
                            }

                            if (currentPtr.prevPtr != null)
                            {
                                completeTime = Node.SetArrowAnim(storyboard, currentPtr, currentPtr.prevPtr, currentPtr.prevArrow, prevCompleteTime);
                            }

                        }
                        break;
                    }

                case "aLine":
                    {
                        int targetLine = int.Parse(decodeResult[1]);

                        completeTime = LineMaskMove(storyboard, prevCompleteTime, codes, targetLine);
                        break;
                    }

                case "gBeq":
                    {
                        Node srcNode = null;
                        Node dstNode = null;
                        if (!decodeResult[1].Equals("null"))
                        {
                            generalVisualPtrSet.TryGetValue(decodeResult[1], out VisualPointer dstPtr);
                            dstNode = dstPtr.pointingNode;
                        }

                        if (!decodeResult[2].Equals("null"))
                        {
                            generalVisualPtrSet.TryGetValue(decodeResult[2], out VisualPointer srcPtr);
                            srcNode = srcPtr.pointingNode;
                        }

                        if (dstNode == srcNode)
                        {
                            programCounter += int.Parse(decodeResult[3]) - 1;
                        }
                        break;
                    }

                case "gBne":
                    {
                        Node srcNode = null;
                        Node dstNode = null;
                        if (!decodeResult[1].Equals("null"))
                        {
                            generalVisualPtrSet.TryGetValue(decodeResult[1], out VisualPointer dstPtr);
                            dstNode = dstPtr.pointingNode;
                        }
                        
                        if (!decodeResult[2].Equals("null"))
                        {
                            generalVisualPtrSet.TryGetValue(decodeResult[2], out VisualPointer srcPtr);
                            srcNode = srcPtr.pointingNode;
                        }

                        if (dstNode != srcNode)
                        {
                            programCounter += int.Parse(decodeResult[3]) - 1;
                        }
                        break;
                    }

                case "vBeq":
                    {
                        generalVisualPtrSet.TryGetValue(decodeResult[1], out VisualPointer dstPtr);
                        if (dstPtr.pointingNode.value == int.Parse(decodeResult[2]))
                        {
                            programCounter += int.Parse(decodeResult[3]) - 1;
                        }
                        break;
                    }

                case "vBge":
                    {
                        generalVisualPtrSet.TryGetValue(decodeResult[1], out VisualPointer dstPtr);
                        generalVisualPtrSet.TryGetValue(decodeResult[2], out VisualPointer srcPtr);
                        if (dstPtr.pointingNode.value >= srcPtr.pointingNode.value)
                        {
                            programCounter += int.Parse(decodeResult[3]) - 1;
                        }
                        break;
                    }

                case "vBle":
                    {
                        generalVisualPtrSet.TryGetValue(decodeResult[1], out VisualPointer dstPtr);
                        generalVisualPtrSet.TryGetValue(decodeResult[2], out VisualPointer srcPtr);
                        if (dstPtr.pointingNode.value <= srcPtr.pointingNode.value)
                        {
                            programCounter += int.Parse(decodeResult[3]) - 1;
                        }
                        break;
                    }

                case "sInit":
                    {
                        scalarSet.Add(decodeResult[1], int.Parse(decodeResult[2]));
                        break;
                    }

                case "sMove":
                    {
                        scalarSet.TryGetValue(decodeResult[2], out int srcSca);

                        scalarSet[decodeResult[1]] = srcSca;
                        break;
                    }

                case "sInc":
                    {
                        scalarSet.TryGetValue(decodeResult[1], out int dstSca);
                        scalarSet[decodeResult[1]] = dstSca + int.Parse(decodeResult[2]);
                        break;
                    }

                case "sBge":
                    {
                        scalarSet.TryGetValue(decodeResult[1], out int scalar);
                        int opr2 = int.Parse(decodeResult[2]);
                        if (scalar >= opr2)
                        {
                            programCounter += int.Parse(decodeResult[3]) - 1;
                        }
                        break;
                    }

                case "sBle":
                    {
                        scalarSet.TryGetValue(decodeResult[1], out int scalar);
                        int opr2 = int.Parse(decodeResult[2]);
                        if (scalar <= opr2)
                        {
                            programCounter += int.Parse(decodeResult[3]) - 1;
                        }
                        break;
                    }

                case "Jmp":
                    {
                        programCounter += int.Parse(decodeResult[1]) - 1;
                        break;
                    }

                case "Halt":
                    {
                        programCounter = -2;
                        break;
                    }

                case "Exception":
                    {
                        string result = "Exception : " + decodeResult[1].Replace("_", "__");
                        completeTime = SetNoticeLabelAnim(storyboard, prevCompleteTime, result);
                        programCounter = -3;
                        inException = true;
                        break;
                    }

                case "Yield":
                    {
                        string result = "Result : " + scalarSet[decodeResult[1]].ToString();
                        completeTime = SetNoticeLabelAnim(storyboard, prevCompleteTime, result);
                        programCounter = -4;
                        break;
                    }

                default:
                    {
                        programCounter = -5;
                        completeTime = SetNoticeLabelAnim(storyboard, prevCompleteTime, "Invalid Instruction!");
                        break;
                    }
            }
            return completeTime;
        }

        private void ForwardEndButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).MinWidth < 0.5)
            {
                return;
            }
            if (storyboardStatus == 0)
            {
                if (storyboardListIdx < storyboardList.Count - 1)
                {
                    storyboardStatus = 1;
                    ++storyboardListIdx;
                    storyboardList[storyboardListIdx].Begin();
                }
            }
            else if (storyboardStatus == 1)
            {
                storyboardStatus = 0;
            }
            else
            {
                storyboardStatus = 1;
                if (codes.Length > 0)
                {
                    // Normal status
                    PrepareStoryboards(codes, instructions);
                    storyboardList[0].Begin();
                }
                else
                {
                    // Running ADL
                    storyboardList.Clear();
                    programCounter = 0;
                    storyboardListIdx = 0;
                    ForwardButton.MinWidth = 1;
                    ForwardEndButton.MinWidth = 1;
                    while (programCounter >= 0)
                    {
                        Storyboard storyboard1 = new Storyboard();
                        double completeTime = LineMaskMove(storyboard1, 0, instructions, programCounter);
                        ExecuteNextInstruction(instructions[programCounter], storyboard1, completeTime);
                        storyboard1.Completed += new EventHandler(BeginNextStoryboard);
                        storyboardList.Add(storyboard1);
                        ++programCounter;
                    }
                    if (storyboardList.Count > 0)
                    {
                        storyboardList[0].Begin();
                    }
                }
            }
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).MinWidth < 0.5)
            {
                return;
            }
            if (storyboardStatus == 0)
            {
                if (storyboardListIdx < storyboardList.Count - 1)
                {
                    ++storyboardListIdx;
                    storyboardList[storyboardListIdx].Begin();
                }
            }
            else if (storyboardStatus == -1)
            {
                storyboardStatus = 0;
                if (codes.Length > 0)
                {
                    // Normal status
                    PrepareStoryboards(codes, instructions);
                    storyboardList[0].Begin();
                }
                else
                {
                    // Running ADL
                    storyboardList.Clear();
                    programCounter = 0;
                    storyboardListIdx = 0;
                    ForwardButton.MinWidth = 1;
                    ForwardEndButton.MinWidth = 1;
                    while (programCounter >= 0)
                    {
                        Storyboard storyboard1 = new Storyboard();
                        double completeTime = LineMaskMove(storyboard1, 0, instructions, programCounter);
                        ExecuteNextInstruction(instructions[programCounter], storyboard1, completeTime);
                        storyboard1.Completed += new EventHandler(BeginNextStoryboard);
                        storyboardList.Add(storyboard1);
                        ++programCounter;
                    }
                    if (storyboardList.Count > 0)
                    {
                        storyboardList[0].Begin();
                    }
                }
            }
        }

        private void BackToStart(object sender, RoutedEventArgs e)
        {
            if (storyboardStatus == 1 && storyboardListIdx < storyboardList.Count)
            {
                storyboardList[storyboardListIdx].Stop();
            }
            storyboardStatus = -1;
            
            Storyboard storyboard = new Storyboard();
            foreach (UIElement element in GeneralCanvas.Children)
            {
                if (element.Opacity > 0.1)
                {
                    Type type = element.GetType();
                    string typeStr = type.Name;

                    switch (typeStr)
                    {
                        case "Arrow":
                            {
                                ((Arrow)element).Close(storyboard, 0);
                                break;
                            }

                        case "ListElement":
                            {
                                ((ListElement)element).Close(storyboard, 0);
                                break;
                            }

                        case "VisualPointer":
                            {
                                ((VisualPointer)element).Close(storyboard, 0);
                                break;
                            }
                    }
                }
            }

            storyboard.Completed += (sArg, eArg) =>
            {
                GeneralCanvas.Children.Clear();
                NoticeLabel.Opacity = 0;
                GeneralCanvas.Children.Add(NoticeLabel);

                ResumeFromBackup();
                storyboardListIdx = -1;
                ForwardButton.MinWidth = 1;
                ForwardEndButton.MinWidth = 1;
            };

            storyboard.Begin();
        }

        private void LoadADLButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).MinWidth < 0.5)
            {
                return;
            }

            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            bool? selectResult = dialog.ShowDialog();
            if (selectResult == true)
            {
                // 用户已选择文件
                instructions = LoadFile(dialog.FileName);
                codes = new string[0];

                scalarSet.Clear();
                storyboardList.Clear();
                EnableDisableAllPanelButtons(0);
                ForwardButton.MinWidth = 1;
                ForwardEndButton.MinWidth = 1;
                SetCodeAreaText(instructions);
                programCounter = 0;
                storyboardStatus = 1;
                storyboardListIdx = 0;
                Storyboard storyboard = new Storyboard();
                HideNoticeLabelAnim(storyboard, 0);
                storyboard.Begin();

                ListBackup();

                while (programCounter >= 0)
                {
                    Storyboard storyboard1 = new Storyboard();
                    double completeTime = LineMaskMove(storyboard1, 0, instructions, programCounter);
                    ExecuteNextInstruction(instructions[programCounter], storyboard1, completeTime);
                    storyboard1.Completed += new EventHandler(BeginNextStoryboard);
                    storyboardList.Add(storyboard1);
                    ++programCounter;
                }
                if (storyboardList.Count > 0)
                {
                    storyboardList[0].Begin();
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            currentScaleRate = 1;
            prevHoriChange = 0;
            prevVerChange = 0;
            Canvas.SetLeft(ViewboxGeneralCanvas, -685);
            Canvas.SetTop(ViewboxGeneralCanvas, -400);
            GeneralCanvasScaleTransform.ScaleX = 1;
            GeneralCanvasScaleTransform.ScaleY = 1;

            /*
            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16)
            {
                EasingMode = EasingMode.EaseOut
            };

            Storyboard storyboard = new Storyboard();
            double durationTime = Math.Sqrt(Math.Pow(Canvas.GetLeft(ViewboxGeneralCanvas) + 685, 2) + Math.Pow(Canvas.GetTop(ViewboxGeneralCanvas) + 400, 2)) / 1000;
            DoubleAnimation leftAnim = new DoubleAnimation(-685, new Duration(TimeSpan.FromSeconds(durationTime)));
            leftAnim.EasingFunction = nonLinearEasingFunction;
            Storyboard.SetTarget(leftAnim, ViewboxGeneralCanvas);
            Storyboard.SetTargetProperty(leftAnim, new PropertyPath("(Canvas.Left)"));
            storyboard.Children.Add(leftAnim);

            DoubleAnimation topAnim = new DoubleAnimation(-400, new Duration(TimeSpan.FromSeconds(durationTime)));
            topAnim.EasingFunction = nonLinearEasingFunction;
            Storyboard.SetTarget(topAnim, ViewboxGeneralCanvas);
            Storyboard.SetTargetProperty(topAnim, new PropertyPath("(Canvas.Top)"));
            storyboard.Children.Add(topAnim);

            DoubleAnimation scaleXAnim = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(durationTime)));
            scaleXAnim.EasingFunction = nonLinearEasingFunction;
            Storyboard.SetTarget(scaleXAnim, ViewboxGeneralCanvas);
            Storyboard.SetTargetProperty(scaleXAnim, new PropertyPath("RenderTransform.ScaleX"));
            storyboard.Children.Add(scaleXAnim);

            DoubleAnimation scaleYAnim = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(durationTime)));
            scaleYAnim.EasingFunction = nonLinearEasingFunction;
            Storyboard.SetTarget(scaleYAnim, ViewboxGeneralCanvas);
            Storyboard.SetTargetProperty(scaleYAnim, new PropertyPath("RenderTransform.ScaleY"));
            storyboard.Children.Add(scaleYAnim);

            storyboard.Completed += (sArg, eArg) =>
            {
                Canvas.SetLeft(ViewboxGeneralCanvas, -685);
                Canvas.SetTop(ViewboxGeneralCanvas, -400);
                GeneralCanvasScaleTransform.ScaleX = 1;
                GeneralCanvasScaleTransform.ScaleY = 1;
            };
            storyboard.Begin();
            */
        }
    }
}

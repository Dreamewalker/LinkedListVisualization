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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LinkedListVisualization.Widget
{
    /// <summary>
    /// ListElement.xaml 的交互逻辑
    /// </summary>
    public partial class ListElement : Viewbox
    {
        public ListElement()
        {
            InitializeComponent();
            Ring.Opacity = 0;
            Content.Opacity = 0;
        }

        public ListElement(int number, byte r, byte g, byte b)
        {
            InitializeComponent();
            Ring.Opacity = 0;
            Content.Opacity = 0;
            SetProperty(number, r, g, b);
        }

        public double Show(Storyboard storyboard, double prevCompleteTime)
        {
            DoubleAnimation ringDoubleAnimation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(700)))
            {
                BeginTime = TimeSpan.FromSeconds(prevCompleteTime)
            };
            DoubleAnimation contentDoubleAnimation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(700)))
            {
                BeginTime = TimeSpan.FromSeconds(prevCompleteTime)
            };

            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;
            ringDoubleAnimation.EasingFunction = nonLinearEasingFunction;
            contentDoubleAnimation.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(ringDoubleAnimation, Ring);
            Storyboard.SetTargetProperty(ringDoubleAnimation, new PropertyPath("Opacity"));

            Storyboard.SetTarget(contentDoubleAnimation, Content);
            Storyboard.SetTargetProperty(contentDoubleAnimation, new PropertyPath("Opacity"));

            storyboard.Children.Add(ringDoubleAnimation);
            storyboard.Children.Add(contentDoubleAnimation);
            return prevCompleteTime + 0.7;
        }

        public double Close(Storyboard storyboard, double prevCompleteTime)
        {
            DoubleAnimation ringDoubleAnimation = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(700)))
            {
                BeginTime = TimeSpan.FromSeconds(prevCompleteTime)
            };
            DoubleAnimation contentDoubleAnimation = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(700)))
            {
                BeginTime = TimeSpan.FromSeconds(prevCompleteTime)
            };

            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;
            ringDoubleAnimation.EasingFunction = nonLinearEasingFunction;
            contentDoubleAnimation.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(ringDoubleAnimation, Ring);
            Storyboard.SetTargetProperty(ringDoubleAnimation, new PropertyPath("Opacity"));

            Storyboard.SetTarget(contentDoubleAnimation, Content);
            Storyboard.SetTargetProperty(contentDoubleAnimation, new PropertyPath("Opacity"));

            storyboard.Children.Add(ringDoubleAnimation);
            storyboard.Children.Add(contentDoubleAnimation);
            return prevCompleteTime + 0.7;
        }

        public void SetProperty(int number, byte r, byte g, byte b)
        {
            Content.Content = number;
            SolidColorBrush solidColorBrush = new SolidColorBrush(Color.FromRgb(r, g, b));
            Content.Foreground = solidColorBrush;
            Ring.Fill = solidColorBrush;
        }

        public double Move(Storyboard storyboard, double prevCompleteTime, double deltaX, double deltaY)
        {
            DoubleAnimation xDoubleAnimation = new DoubleAnimation(Canvas.GetLeft(this), Canvas.GetLeft(this) + deltaX, new Duration(TimeSpan.FromMilliseconds(1500)));
            xDoubleAnimation.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);

            DoubleAnimation yDoubleAnimation = new DoubleAnimation(Canvas.GetTop(this), Canvas.GetTop(this) + deltaX, new Duration(TimeSpan.FromMilliseconds(1500)));
            yDoubleAnimation.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);

            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;

            xDoubleAnimation.EasingFunction = nonLinearEasingFunction;
            yDoubleAnimation.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(xDoubleAnimation, this);
            Storyboard.SetTargetProperty(xDoubleAnimation, new PropertyPath("(Canvas.Left)"));
            Storyboard.SetTarget(yDoubleAnimation, this);
            Storyboard.SetTargetProperty(yDoubleAnimation, new PropertyPath("(Canvas.Top)"));

            storyboard.Children.Add(xDoubleAnimation);
            storyboard.Children.Add(yDoubleAnimation);
            return prevCompleteTime + 1.5;
        }
    }
}

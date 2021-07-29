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
    /// Arrow.xaml 的交互逻辑
    /// </summary>
    public partial class Arrow : Viewbox
    {
        public Arrow()
        {
            InitializeComponent();
            ArrowLine.Opacity = 0;
            ArrowUp.Opacity = 0;
            ArrowDown.Opacity = 0;
        }

        public double Expand(Storyboard storyboard, double prevCompleteTime)
        {

            double thisCompleteTime = ShowAmin(storyboard, prevCompleteTime);
            return StartRot(storyboard, thisCompleteTime);
        }

        public double Close(Storyboard storyboard, double prevCompleteTime)
        {

            double thisCompleteTime = CloseRotAnim(storyboard, prevCompleteTime);
            return EndRot(storyboard, thisCompleteTime);
        }

        private double RotateAmin(Storyboard storyboard, double prevCompleteTime, Viewbox viewbox, double initAngle, double targetAngle, int opacityTarget)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(initAngle, targetAngle, new Duration(TimeSpan.FromMilliseconds(500)));
            doubleAnimation.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);
            RotateTransform rotateTransform = new RotateTransform();
            viewbox.RenderTransform = rotateTransform;

            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;
            doubleAnimation.EasingFunction = nonLinearEasingFunction;
            Storyboard.SetTarget(doubleAnimation, viewbox);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("RenderTransform.Angle"));
            storyboard.Children.Add(doubleAnimation);

            DoubleAnimation doubleAnimationOpcacity = new DoubleAnimation(1 - opacityTarget, opacityTarget, new Duration(TimeSpan.FromMilliseconds(500)));
            doubleAnimationOpcacity.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);
            doubleAnimationOpcacity.EasingFunction = nonLinearEasingFunction;
            Storyboard.SetTarget(doubleAnimationOpcacity, viewbox);
            Storyboard.SetTargetProperty(doubleAnimationOpcacity, new PropertyPath("Opacity"));
            storyboard.Children.Add(doubleAnimationOpcacity);

            return prevCompleteTime + 0.5;
        }

        private double ShowAmin(Storyboard storyboard, double prevCompleteTime)
        {
            DoubleAnimation doubleAnimationOpcacity = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(500)));
            doubleAnimationOpcacity.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);
            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;
            doubleAnimationOpcacity.EasingFunction = nonLinearEasingFunction;
            Storyboard.SetTarget(doubleAnimationOpcacity, ArrowLine);
            Storyboard.SetTargetProperty(doubleAnimationOpcacity, new PropertyPath("Opacity"));
            storyboard.Children.Add(doubleAnimationOpcacity);
            return prevCompleteTime + 0.5;
        }

        private double CloseRotAnim(Storyboard storyboard, double prevCompleteTime)
        {
            RotateAmin(storyboard, prevCompleteTime, ArrowUp, 240, 270, 0);
            return RotateAmin(storyboard, prevCompleteTime, ArrowDown, 120, 90, 0);
        }

        private double StartRot(Storyboard storyboard, double prevCompleteTime)
        {
            RotateAmin(storyboard, prevCompleteTime, ArrowUp, 270, 240, 1);
            return RotateAmin(storyboard, prevCompleteTime, ArrowDown, 90, 120, 1);
        }

        private double EndRot(Storyboard storyboard, double prevCompleteTime)
        {
            DoubleAnimation doubleAnimationOpcacity = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(500)));
            doubleAnimationOpcacity.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);
            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;
            doubleAnimationOpcacity.EasingFunction = nonLinearEasingFunction;
            Storyboard.SetTarget(doubleAnimationOpcacity, ArrowLine);
            Storyboard.SetTargetProperty(doubleAnimationOpcacity, new PropertyPath("Opacity"));
            storyboard.Children.Add(doubleAnimationOpcacity);
            return prevCompleteTime + 0.5;
        }

    }
}

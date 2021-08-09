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

        public double currentCanvasLeft = 0;
        public double currentCanvasTop = 0;
        public double currentScaleX = 1;
        public double currentAngle = 0;

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

        public double Rotate(Storyboard storyboard, double prevCompleteTime, double angle)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(currentAngle, currentAngle + angle, new Duration(TimeSpan.FromMilliseconds(1000)));
            doubleAnimation.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);
            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;
            doubleAnimation.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(doubleAnimation, Rotation);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Angle"));
            storyboard.Children.Add(doubleAnimation);
            currentAngle = currentAngle + angle;
            return prevCompleteTime + 1;
        }

        public double Move(Storyboard storyboard, double prevCompleteTime, double deltaX, double deltaY)
        {
            DoubleAnimation xDoubleAnimation = new DoubleAnimation(currentCanvasLeft, currentCanvasLeft + deltaX, new Duration(TimeSpan.FromMilliseconds(1500)));
            xDoubleAnimation.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);

            DoubleAnimation yDoubleAnimation = new DoubleAnimation(currentCanvasTop, currentCanvasTop + deltaY, new Duration(TimeSpan.FromMilliseconds(1500)));
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

            currentCanvasLeft += deltaX;
            currentCanvasTop += deltaY;

            return prevCompleteTime + 1.5;
        }

        public double PointingAnim(Storyboard storyboard, double prevCompleteTime, double targetLeft, double targetTop)
        {
            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;

            double targetLength = Math.Sqrt(Math.Pow(targetLeft - currentCanvasLeft, 2) + Math.Pow(targetTop - currentCanvasTop - 17.5, 2));
            double targetScaleRate = targetLength / 190;

            double targetAngle = Math.Atan2(targetTop - currentCanvasTop - 17.5, targetLeft - currentCanvasLeft) / Math.PI * 180;

            /*
            DoubleAnimationUsingKeyFrames scaleAnim = new DoubleAnimationUsingKeyFrames();
            scaleAnim.Duration = new Duration(TimeSpan.FromSeconds(1.5));
            EasingDoubleKeyFrame scaleBeginFrame = new EasingDoubleKeyFrame(currentScaleX, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0)), nonLinearEasingFunction);
            EasingDoubleKeyFrame scaleEndFrame = new EasingDoubleKeyFrame(targetScaleRate, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1.5)), nonLinearEasingFunction);
            scaleAnim.KeyFrames.Add(scaleBeginFrame);
            scaleAnim.KeyFrames.Add(scaleEndFrame);
            */
            /*
            RotateTransform rotateTransform = new RotateTransform(0, 0, 17.5);
            ScaleTransform scaleTransform = new ScaleTransform(1, 1, 0, 17.5);
            TransformGroup group = new TransformGroup();
            group.Children.Add(scaleTransform);
            group.Children.Add(rotateTransform);
            this.RenderTransform = group;
            */

            DoubleAnimation scaleAnim = new DoubleAnimation(currentScaleX, targetScaleRate, new Duration(TimeSpan.FromSeconds(1.5)));
            scaleAnim.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);
            scaleAnim.EasingFunction = nonLinearEasingFunction;
            Storyboard.SetTarget(scaleAnim, this);
            Storyboard.SetTargetProperty(scaleAnim, new PropertyPath("RenderTransform.Children[0].ScaleX"));
            storyboard.Children.Add(scaleAnim);
            currentScaleX = targetScaleRate;

            /*
            DoubleAnimationUsingKeyFrames angleAnim = new DoubleAnimationUsingKeyFrames();
            angleAnim.Duration = new Duration(TimeSpan.FromSeconds(1.5));
            EasingDoubleKeyFrame angleBeginFrame = new EasingDoubleKeyFrame(currentAngle, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0)), nonLinearEasingFunction);
            EasingDoubleKeyFrame angleEndFrame = new EasingDoubleKeyFrame(targetAngle, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1.5)), nonLinearEasingFunction);
            angleAnim.KeyFrames.Add(angleBeginFrame);
            angleAnim.KeyFrames.Add(angleEndFrame);
            */
            DoubleAnimation angleAnim = new DoubleAnimation(currentAngle, targetAngle, new Duration(TimeSpan.FromSeconds(1.5)));
            angleAnim.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);
            angleAnim.EasingFunction = nonLinearEasingFunction;
            Storyboard.SetTarget(angleAnim, this);
            Storyboard.SetTargetProperty(angleAnim, new PropertyPath("RenderTransform.Children[1].Angle"));
            storyboard.Children.Add(angleAnim);
            currentAngle = targetAngle;

            //ScaleTrans.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
            //Rotation.BeginAnimation(RotateTransform.AngleProperty, angleAnim);
            return prevCompleteTime + 1.5;
        }

        public double MoveBaseAnim(Storyboard storyboard, double prevCompleteTime, double targetLeft, double targetTop)
        {
            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;

            double currentEndLeft = currentCanvasLeft + Math.Cos(currentAngle / 180 * Math.PI) * 190 * currentScaleX;
            double currentEndTop = currentCanvasTop + 17.5 - Math.Sin(currentAngle / 180 * Math.PI) * 190 * currentScaleX;

            double targetLength = Math.Sqrt(Math.Pow(targetLeft - currentEndLeft, 2) + Math.Pow(targetTop - currentEndTop, 2));
            double targetScaleRate = targetLength / 190;

            double targetAngle = Math.Atan2(currentEndTop - targetTop, currentEndLeft - targetLeft) * 180 / Math.PI;

            DoubleAnimation scaleAnim = new DoubleAnimation(targetScaleRate, new Duration(TimeSpan.FromSeconds(1.5)));
            scaleAnim.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);
            scaleAnim.EasingFunction = nonLinearEasingFunction;
            Storyboard.SetTarget(scaleAnim, this);
            Storyboard.SetTargetProperty(scaleAnim, new PropertyPath("RenderTransform.Children[0].ScaleX"));
            storyboard.Children.Add(scaleAnim);
            currentScaleX = targetScaleRate;

            DoubleAnimation angleAnim = new DoubleAnimation(targetAngle, new Duration(TimeSpan.FromSeconds(1.5)));
            angleAnim.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);
            angleAnim.EasingFunction = nonLinearEasingFunction;
            Storyboard.SetTarget(angleAnim, this);
            Storyboard.SetTargetProperty(angleAnim, new PropertyPath("RenderTransform.Children[1].Angle"));
            storyboard.Children.Add(angleAnim);
            currentAngle = targetAngle;

            DoubleAnimation baseXAnim = new DoubleAnimation(targetLeft, new Duration(TimeSpan.FromSeconds(1.5)));
            baseXAnim.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);
            baseXAnim.EasingFunction = nonLinearEasingFunction;
            Storyboard.SetTarget(baseXAnim, this);
            Storyboard.SetTargetProperty(baseXAnim, new PropertyPath("(Canvas.Left)"));
            storyboard.Children.Add(baseXAnim);
            currentCanvasLeft = targetLeft;

            DoubleAnimation baseYAnim = new DoubleAnimation(targetTop, new Duration(TimeSpan.FromSeconds(1.5)));
            baseYAnim.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);
            baseYAnim.EasingFunction = nonLinearEasingFunction;
            Storyboard.SetTarget(baseYAnim, this);
            Storyboard.SetTargetProperty(baseYAnim, new PropertyPath("(Canvas.Top)"));
            storyboard.Children.Add(baseYAnim);
            currentCanvasTop = targetTop;

            //ScaleTrans.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnim);
            //Rotation.BeginAnimation(RotateTransform.AngleProperty, angleAnim);
            return prevCompleteTime + 1.5;
        }

        private double RotateAmin(Storyboard storyboard, double prevCompleteTime, Viewbox viewbox, double initAngle, double targetAngle, int opacityTarget)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(initAngle, targetAngle, new Duration(TimeSpan.FromMilliseconds(500)));
            doubleAnimation.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);

            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;
            doubleAnimation.EasingFunction = nonLinearEasingFunction;
            Storyboard.SetTarget(doubleAnimation, viewbox);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("RenderTransform.Children[0].Angle"));
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
            RotateAmin(storyboard, prevCompleteTime, ArrowUp, -60, -90, 0);
            return RotateAmin(storyboard, prevCompleteTime, ArrowDown, 60, 90, 0);
        }

        private double StartRot(Storyboard storyboard, double prevCompleteTime)
        {
            RotateAmin(storyboard, prevCompleteTime, ArrowUp, -90, -60, 1);
            return RotateAmin(storyboard, prevCompleteTime, ArrowDown, 90, 60, 1);
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

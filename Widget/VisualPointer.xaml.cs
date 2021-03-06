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
    /// VisualPointer.xaml 的交互逻辑
    /// </summary>
    public partial class VisualPointer : Viewbox
    {
        // 非线性动画Easing function
        private static NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16)
        {
            EasingMode = EasingMode.EaseIn
        };

        public Node pointingNode = null;
        public double currentCanvasLeft = 0;
        public double currentCanvasTop = 0;
        public double currentAngle = 0;
        public VisualPointer(String pointerName, Node pointingNode)
        {
            InitializeComponent();
            PointerType.Content = pointerName;
            this.pointingNode = pointingNode;
            PointerType.Opacity = 0;
            BackgroundPath.Opacity = 0;
        }

        public double Show(Storyboard storyboard, double prevCompleteTime)
        {
            ChangeElementOpacityAnim(storyboard, prevCompleteTime, BackgroundPath, pointingNode == null ? 0.11 : 0.4);
            return ChangeElementOpacityAnim(storyboard, prevCompleteTime, PointerType, 1);
        }

        public double Close(Storyboard storyboard, double prevCompleteTime)
        {
            ChangeElementOpacityAnim(storyboard, prevCompleteTime, BackgroundPath, 0);
            return ChangeElementOpacityAnim(storyboard, prevCompleteTime, PointerType, 0);
        }

        public double SetNullAnim(Storyboard storyboard, double prevCompleteTime, bool isNull)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(isNull ? 0.11 : 0.4, new Duration(TimeSpan.FromSeconds(0.4)))
            {
                EasingFunction = new ExponentialEase()
                {
                    EasingMode = EasingMode.EaseOut
                },
                BeginTime = TimeSpan.FromSeconds(prevCompleteTime)
            };

            Storyboard.SetTarget(doubleAnimation, BackgroundPath);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Opacity"));
            storyboard.Children.Add(doubleAnimation);
            return prevCompleteTime + 0.4;
        }

        private double ChangeElementOpacityAnim(Storyboard storyboard, double prevCompleteTime, UIElement element, double opacityTarget)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(element.Opacity, opacityTarget, new Duration(TimeSpan.FromMilliseconds(700)));
            doubleAnimation.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);
            doubleAnimation.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(doubleAnimation, element);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Opacity"));
            storyboard.Children.Add(doubleAnimation);
            return prevCompleteTime + 0.7;
        }

        public double MoveToAnim(Storyboard storyboard, double prevCompleteTime, double targetLeft, double targetTop, double targetAngle)
        {

            /*
            DoubleAnimationUsingKeyFrames xAnim = new DoubleAnimationUsingKeyFrames();
            EasingDoubleKeyFrame xAnimBegin = new EasingDoubleKeyFrame(currentCanvasLeft, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)), nonLinearEasingFunction);
            EasingDoubleKeyFrame xAnimEnd = new EasingDoubleKeyFrame(targetLeft, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1500)), nonLinearEasingFunction);
            xAnim.KeyFrames.Add(xAnimBegin);
            xAnim.KeyFrames.Add(xAnimEnd);
            */

            DoubleAnimation xAnim = new DoubleAnimation(currentCanvasLeft, targetLeft, new Duration(TimeSpan.FromMilliseconds(1500)));
            xAnim.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);
            xAnim.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(xAnim, this);
            Storyboard.SetTargetProperty(xAnim, new PropertyPath("(Canvas.Left)"));
            storyboard.Children.Add(xAnim);
            currentCanvasLeft = targetLeft;

            /*
            DoubleAnimationUsingKeyFrames yAnim = new DoubleAnimationUsingKeyFrames();
            EasingDoubleKeyFrame yAnimBegin = new EasingDoubleKeyFrame(currentCanvasTop, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)), nonLinearEasingFunction);
            EasingDoubleKeyFrame yAnimEnd = new EasingDoubleKeyFrame(targetTop, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1500)), nonLinearEasingFunction);
            yAnim.KeyFrames.Add(yAnimBegin);
            yAnim.KeyFrames.Add(yAnimEnd);
            */

            DoubleAnimation yAnim = new DoubleAnimation(currentCanvasTop, targetTop, new Duration(TimeSpan.FromMilliseconds(1500)));
            yAnim.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);
            yAnim.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(yAnim, this);
            Storyboard.SetTargetProperty(yAnim, new PropertyPath("(Canvas.Top)"));
            storyboard.Children.Add(yAnim);
            currentCanvasTop = targetTop;


            DoubleAnimation angleAnim = new DoubleAnimation(targetAngle, new Duration(TimeSpan.FromMilliseconds(1500)));
            angleAnim.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);
            angleAnim.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(angleAnim, this);
            Storyboard.SetTargetProperty(angleAnim, new PropertyPath("RenderTransform.Angle"));
            storyboard.Children.Add(angleAnim);
            currentAngle = targetAngle;

            return prevCompleteTime + 1.5;
        }

        public static double ShowPointersInNodeAnim(Node node, Canvas canvas, Storyboard storyboard, Dictionary<string, VisualPointer> generalVisualPointers, double prevCompleteTime)
        {
            double posX = node.listElement.currentCanvasLeft + 40 - 50;
            double posY = node.listElement.currentCanvasTop + 80 + 20;

            List<VisualPointer> relatedList = node.GetRelatedPointers(generalVisualPointers);
            foreach (VisualPointer visualPointer in relatedList)
            {
                Canvas.SetLeft(visualPointer, posX);
                visualPointer.currentCanvasLeft = posX;
                Canvas.SetTop(visualPointer, posY);
                visualPointer.currentCanvasTop = posY;
                visualPointer.Show(storyboard, prevCompleteTime);
                canvas.Children.Add(visualPointer);
                posY += 60;
            }

            return prevCompleteTime + 0.7;
        }

        public static double RecycleShowPointersInNodeAnim(Node node, Canvas canvas, Storyboard storyboard, Dictionary<string, VisualPointer> generalVisualPointers, double prevCompleteTime, double centerX, double centerY)
        {
            double posX = node.listElement.currentCanvasLeft + 40;
            double posY = node.listElement.currentCanvasTop + 40;

            double radius = Math.Sqrt(Math.Pow(posX - centerX, 2) + Math.Pow(posY - centerY, 2));

            double pointerCenterR = radius + 80;
            double angle = Math.Atan2(posY - centerY, posX - centerX) / Math.PI * 180 + 90;

            List<VisualPointer> relatedList = node.GetRelatedPointers(generalVisualPointers);
            foreach (VisualPointer visualPointer in relatedList)
            {
                Canvas.SetLeft(visualPointer, centerX + (posX - centerX) / radius * pointerCenterR - 50);
                Canvas.SetTop(visualPointer, centerY + (posY - centerY) / radius * pointerCenterR - 20);

                visualPointer.currentCanvasLeft = centerX + (posX - centerX) / radius * pointerCenterR - 50;
                visualPointer.currentCanvasTop = centerY + (posY - centerY) / radius * pointerCenterR - 20;

                visualPointer.Rotation.Angle = angle;
                visualPointer.currentAngle = angle;
                visualPointer.Show(storyboard, prevCompleteTime);
                canvas.Children.Add(visualPointer);
                pointerCenterR += 60;
            }
            node.vpAngle = angle;
            return prevCompleteTime + 0.7;
        }

        public static double MovePointersInNodeAnim(Node node, Storyboard storyboard, Dictionary<string, VisualPointer> generalVisualPointers, double prevCompleteTime)
        {
            double posX = node.listElement.currentCanvasLeft + 40 - 50;
            double posY = node.listElement.currentCanvasTop + 80 + 20;
            double completeTime = 0;

            List<VisualPointer> relatedList = node.GetRelatedPointers(generalVisualPointers);
            foreach (VisualPointer visualPointer in relatedList)
            {
                if (visualPointer.Opacity > 0.1)
                {
                    completeTime = visualPointer.MoveToAnim(storyboard, prevCompleteTime, posX, posY, 0);

                    posY += 60;
                }
            }

            return completeTime;
        }

        public static double RecycleMovePointersInNodeAnim(Node node, Storyboard storyboard, Dictionary<string, VisualPointer> generalVisualPointers, double prevCompleteTime, double centerX, double centerY)
        {
            double posX = node.listElement.currentCanvasLeft + 40;
            double posY = node.listElement.currentCanvasTop + 40;

            double radius = Math.Sqrt(Math.Pow(posX - centerX, 2) + Math.Pow(posY - centerY, 2));

            double pointerCenterR = radius + 80;
            double completeTime = 0;
            double angle = Math.Atan2(posY - centerY, posX - centerX) / Math.PI * 180 + 90;
            node.vpAngle = angle;

            List<VisualPointer> relatedList = node.GetRelatedPointers(generalVisualPointers);
            foreach (VisualPointer visualPointer in relatedList)
            {

                completeTime = visualPointer.MoveToAnim(storyboard, prevCompleteTime,
                                                        centerX + (posX - centerX) / radius * pointerCenterR - 50,
                                                        centerY + (posY - centerY) / radius * pointerCenterR - 20, angle);
                
                pointerCenterR += 60;
            }

            return completeTime;
        }

    }
}

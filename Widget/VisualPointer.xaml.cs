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
        public VisualPointer(String pointerName)
        {
            InitializeComponent();
            PointerType.Content = pointerName;
            PointerType.Opacity = 0;
            BackgroundPath.Opacity = 0;
        }

        public double Show(Storyboard storyboard, double prevCompleteTime)
        {
            ChangeElementOpacityAnim(storyboard, prevCompleteTime, BackgroundPath, 0.4);
            return ChangeElementOpacityAnim(storyboard, prevCompleteTime, PointerType, 1);
        }

        public double Close(Storyboard storyboard, double prevCompleteTime)
        {
            ChangeElementOpacityAnim(storyboard, prevCompleteTime, BackgroundPath, 0);
            return ChangeElementOpacityAnim(storyboard, prevCompleteTime, PointerType, 0);
        }

        private double ChangeElementOpacityAnim(Storyboard storyboard, double prevCompleteTime, UIElement element, double opacityTarget)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(element.Opacity, opacityTarget, new Duration(TimeSpan.FromMilliseconds(700)));
            doubleAnimation.BeginTime = TimeSpan.FromSeconds(prevCompleteTime);
            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;
            doubleAnimation.EasingFunction = nonLinearEasingFunction;

            Storyboard.SetTarget(doubleAnimation, element);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Opacity"));
            storyboard.Children.Add(doubleAnimation);
            return prevCompleteTime + 0.7;
        }

        public static double ShowPointersInNodeAnim(Node node, Canvas canvas, Storyboard storyboard, double prevCompleteTime)
        {
            double posX = Canvas.GetLeft(node.listElement) + 40 - 50;
            double posY = Canvas.GetTop(node.listElement) + 80 + 20;

            foreach (VisualPointer visualPointer in node.relatedPointers.Values)
            {
                Canvas.SetLeft(visualPointer, posX);
                Canvas.SetTop(visualPointer, posY);
                visualPointer.Show(storyboard, prevCompleteTime);
                canvas.Children.Add(visualPointer);
                posY += 60;
            }

            return prevCompleteTime + 0.7;
        }

        public static double RecycleShowPointersInNodeAnim(Node node, Canvas canvas, Storyboard storyboard, double prevCompleteTime, double centerX, double centerY)
        {
            double posX = Canvas.GetLeft(node.listElement) + 40;
            double posY = Canvas.GetTop(node.listElement) + 40;

            double radius = Math.Sqrt(Math.Pow(posX - centerX, 2) + Math.Pow(posY - centerY, 2));

            double pointerCenterR = radius + 80;

            foreach (VisualPointer visualPointer in node.relatedPointers.Values)
            {
                Canvas.SetLeft(visualPointer, centerX + (posX - centerX) / radius * pointerCenterR - 50);
                Canvas.SetTop(visualPointer, centerY + (posY - centerY) / radius * pointerCenterR - 20);

                double angle = Math.Atan2(posY - centerY, posX - centerX) / Math.PI * 180;
                visualPointer.Rotation.Angle = angle + 90;
                visualPointer.Show(storyboard, prevCompleteTime);
                canvas.Children.Add(visualPointer);
                pointerCenterR += 60;
            }

            return prevCompleteTime + 0.7;
        }
    }
}

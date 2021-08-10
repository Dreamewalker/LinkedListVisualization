using LinkedListVisualization.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LinkedListVisualization
{
    public class Node
    {
        public int value;
        public Node prevPtr = null;
        public Node nextPtr = null;
        public ListElement listElement = null;
        public Arrow prevArrow = null;
        public Arrow nextArrow = null;
        public double vpAngle = 0;
        // public Dictionary<string, VisualPointer> relatedPointers = new Dictionary<string, VisualPointer>();
        public List<Arrow> arrowsPointingToMe = new List<Arrow>(); // 仅记录指向该结点的后继指针

        public Node(int nodeValue, byte colorR, byte colorG, byte colorB, Node prevNode)
        {
            value = nodeValue;
            listElement = new ListElement(nodeValue, colorR, colorG, colorB);

            if (prevNode != null)
            {
                prevPtr = prevNode;
                prevArrow = new Arrow();
            }
        }
        public double InitialDrawLinear(Canvas canvas, Storyboard storyboard, bool isBidirection, double prevCompleteTime, Dictionary<string, VisualPointer> generalVisualPointers)
        {
            if (listElement == null)
            {
                return 0;
            }
            double finishTime = prevCompleteTime;
            double canvasLeft = 785 + 500;
            double canvasTop = 200 + 400;
            Node currentPtr = this;

            Canvas.SetLeft(currentPtr.listElement, canvasLeft);
            Canvas.SetTop(currentPtr.listElement, canvasTop);
            currentPtr.listElement.currentCanvasLeft = canvasLeft;
            currentPtr.listElement.currentCanvasTop = canvasTop;

            canvas.Children.Add(currentPtr.listElement);
            VisualPointer.ShowPointersInNodeAnim(currentPtr, canvas, storyboard, generalVisualPointers, prevCompleteTime);
            finishTime = currentPtr.listElement.Show(storyboard, prevCompleteTime);
            canvasLeft += 190;

            while (currentPtr.nextPtr != null)
            {
                // 循环链表尾结点处理
                if (currentPtr.nextPtr == this)
                {
                    Node.SetArrowNoAnim(currentPtr, currentPtr.nextPtr, currentPtr.nextArrow);
                    canvas.Children.Add(currentPtr.nextArrow);
                    finishTime = currentPtr.nextArrow.Expand(storyboard, prevCompleteTime);
                    break;
                }
                Canvas.SetLeft(currentPtr.nextPtr.listElement, canvasLeft);
                Canvas.SetTop(currentPtr.nextPtr.listElement, canvasTop);
                currentPtr.nextPtr.listElement.currentCanvasLeft = canvasLeft;
                currentPtr.nextPtr.listElement.currentCanvasTop = canvasTop;

                canvas.Children.Add(currentPtr.nextPtr.listElement);
                VisualPointer.ShowPointersInNodeAnim(currentPtr.nextPtr, canvas, storyboard, generalVisualPointers, prevCompleteTime);
                finishTime = currentPtr.nextPtr.listElement.Show(storyboard, prevCompleteTime);

                Node.SetArrowNoAnim(currentPtr, currentPtr.nextPtr, currentPtr.nextArrow);
                canvas.Children.Add(currentPtr.nextArrow);
                finishTime = currentPtr.nextArrow.Expand(storyboard, prevCompleteTime);


                if (currentPtr.prevPtr != null)
                {
                    Node.SetArrowNoAnim(currentPtr, currentPtr.prevPtr, currentPtr.prevArrow);
                    // Canvas.SetLeft(currentPtr.prevArrow, canvasLeft + 80 - 160);
                    // Canvas.SetTop(currentPtr.prevArrow, canvasTop + 20);
                    canvas.Children.Add(currentPtr.prevArrow);
                    finishTime = currentPtr.prevArrow.Expand(storyboard, prevCompleteTime);
                }
                canvasLeft += 190;
                currentPtr = currentPtr.nextPtr;
            }

            if (currentPtr.prevPtr != null)
            {
                Node.SetArrowNoAnim(currentPtr, currentPtr.prevPtr, currentPtr.prevArrow);
                // Canvas.SetLeft(currentPtr.prevArrow, canvasLeft + 80 - 160);
                // Canvas.SetTop(currentPtr.prevArrow, canvasTop + 20);
                canvas.Children.Add(currentPtr.prevArrow);
                finishTime = currentPtr.prevArrow.Expand(storyboard, prevCompleteTime);
            }

            return finishTime;
        }

        public double InitialDrawRecycle(Canvas canvas, Storyboard storyboard, int elementNum, double prevCompleteTime, Dictionary<string, VisualPointer> generalVisualPointers)
        {
            double singleHalfAngle = Math.PI / elementNum;
            double radius = 95 / Math.Sin(singleHalfAngle);
            double canvasLeftBias = 785 + 500 + radius;
            double canvasTopBias = 200 + 400 + radius;

            if (elementNum == 1)
            {
                canvasLeftBias = 785 + 500;
                canvasTopBias = 200 + 400;

                Canvas.SetLeft(this.listElement, -40 + canvasLeftBias);
                Canvas.SetTop(this.listElement, -40 + canvasTopBias);
                listElement.currentCanvasLeft = -40 + canvasLeftBias;
                listElement.currentCanvasTop = -40 + canvasTopBias;

                VisualPointer.ShowPointersInNodeAnim(this, canvas, storyboard, generalVisualPointers, prevCompleteTime);
                canvas.Children.Add(this.listElement);

                Node.SetArrowNoAnim(this, this, this.nextArrow);
                nextArrow.Expand(storyboard, prevCompleteTime);
                canvas.Children.Add(nextArrow);
                return this.listElement.Show(storyboard, prevCompleteTime);
            }

            double finishTime;

            Point[] point = new Point[elementNum];
            for (int i = 0; i < elementNum; ++i)
            {
                point[i].X = radius * Math.Cos(singleHalfAngle * 2 * i);
                point[i].Y = radius * Math.Sin(singleHalfAngle * 2 * i);
            }

            Node currentPtr = this.nextPtr;
            Node prevCurrentPtr = this;
            Canvas.SetLeft(this.listElement, point[0].X - 40 + canvasLeftBias);
            Canvas.SetTop(this.listElement, point[0].Y - 40 + canvasTopBias);
            listElement.currentCanvasLeft = point[0].X - 40 + canvasLeftBias;
            listElement.currentCanvasTop = point[0].Y - 40 + canvasTopBias;

            VisualPointer.RecycleShowPointersInNodeAnim(this, canvas, storyboard, generalVisualPointers, prevCompleteTime, canvasLeftBias, canvasTopBias);
            canvas.Children.Add(this.listElement);
            finishTime = this.listElement.Show(storyboard, prevCompleteTime);
            for (int i = 1; i < elementNum; ++i)
            {
                Canvas.SetLeft(currentPtr.listElement, point[i].X - 40 + canvasLeftBias);
                Canvas.SetTop(currentPtr.listElement, point[i].Y - 40 + canvasTopBias);
                currentPtr.listElement.currentCanvasLeft = point[i].X - 40 + canvasLeftBias;
                currentPtr.listElement.currentCanvasTop = point[i].Y - 40 + canvasTopBias;

                canvas.Children.Add(currentPtr.listElement);
                VisualPointer.RecycleShowPointersInNodeAnim(currentPtr, canvas, storyboard, generalVisualPointers, prevCompleteTime, canvasLeftBias, canvasTopBias);
                currentPtr.listElement.Show(storyboard, prevCompleteTime);

                Node.SetArrowNoAnim(prevCurrentPtr, currentPtr, prevCurrentPtr.nextArrow);
                canvas.Children.Add(prevCurrentPtr.nextArrow);
                prevCurrentPtr.nextArrow.Expand(storyboard, prevCompleteTime);

                prevCurrentPtr = currentPtr;
                currentPtr = currentPtr.nextPtr;
            }

            Node.SetArrowNoAnim(prevCurrentPtr, this, prevCurrentPtr.nextArrow);
            canvas.Children.Add(prevCurrentPtr.nextArrow);
            finishTime = prevCurrentPtr.nextArrow.Expand(storyboard, prevCompleteTime);

            return finishTime;
        }


        public double CloseAnim(Storyboard storyboard, double prevCompleteTime)
        {
            if (listElement == null)
            {
                return prevCompleteTime;
            }
            double finishTime = prevCompleteTime;
            Node currentPtr = this;
            do
            {
                finishTime = currentPtr.listElement.Close(storyboard, prevCompleteTime);


                if (currentPtr.nextPtr != null)
                {
                    finishTime = currentPtr.nextArrow.Close(storyboard, prevCompleteTime);
                }

                if (currentPtr.prevPtr != null)
                {
                    finishTime = currentPtr.prevArrow.Close(storyboard, prevCompleteTime);
                }
                currentPtr = currentPtr.nextPtr;
            } while (currentPtr != null && currentPtr != this);

            return finishTime;
        }

        public void RemoveFromCanvas(Canvas canvas)
        {
            if (listElement == null)
            {
                return;
            }
            Node currentPtr = this;
            do
            {
                canvas.Children.Remove(currentPtr.listElement);


                if (currentPtr.nextPtr != null)
                {
                    canvas.Children.Remove(currentPtr.nextArrow);
                }

                if (currentPtr.prevPtr != null)
                {
                    canvas.Children.Remove(currentPtr.prevArrow);
                }
                currentPtr = currentPtr.nextPtr;
            } while (currentPtr != null && currentPtr != this);
        }

        public void CreateNextNode(int nodeValue, bool isBidirection)
        {
            nextPtr = new Node(nodeValue, 155, 155, 155, isBidirection ? this : null);
            nextArrow = new Arrow();
            nextArrow.Rotation.Angle = 0;
            nextArrow.currentAngle = 0;
            nextPtr.arrowsPointingToMe.Add(nextArrow);
        }

        public static void SetArrowNoAnim(Node srcNode, Node dstNode, Arrow arrowToBeSet)
        {
            double srcX = srcNode.listElement.currentCanvasLeft;
            double srcY = srcNode.listElement.currentCanvasTop;

            double dstX = dstNode.listElement.currentCanvasLeft;
            double dstY = dstNode.listElement.currentCanvasTop;

            double scaleRate = Math.Sqrt(Math.Pow(srcX - dstX, 2) + Math.Pow(srcY - dstY, 2)) / 190.0;

            arrowToBeSet.ScaleTrans.ScaleX = scaleRate;
            arrowToBeSet.currentScaleX = scaleRate;

            double tanValue = (srcY - dstY) / (srcX - dstX);
            double rotAngle = Math.Atan2(dstY - srcY, dstX - srcX) / 2 / Math.PI * 360;

            arrowToBeSet.Rotation.Angle = rotAngle;
            arrowToBeSet.currentAngle = rotAngle;

            Canvas.SetLeft(arrowToBeSet, srcX + 40);
            Canvas.SetTop(arrowToBeSet, srcY + 40 - 17.5);
            arrowToBeSet.currentCanvasLeft = srcX + 40;
            arrowToBeSet.currentCanvasTop = srcY + 40 - 17.5;
        }

        public static double SetArrowAnim(Storyboard storyboard, Node srcNode, Node dstNode, Arrow arrowToBeRotate, double prevFinishTime)
        {
            double srcX = srcNode.listElement.currentCanvasLeft + 40;
            double srcY = srcNode.listElement.currentCanvasTop + 40;

            double dstX = dstNode.listElement.currentCanvasLeft + 40;
            double dstY = dstNode.listElement.currentCanvasTop + 40;

            double scaleRate = Math.Sqrt(Math.Pow(srcX - dstX, 2) + Math.Pow(srcY - dstY, 2)) / 190.0;
            double targetAngle = Math.Atan2(dstY - srcY, dstX - srcX) / Math.PI * 180;

            NonLinearEasingFunction nonLinearEasingFunction = new NonLinearEasingFunction(16);
            nonLinearEasingFunction.EasingMode = EasingMode.EaseIn;

            DoubleAnimation leftAnim = new DoubleAnimation(srcX, new Duration(TimeSpan.FromMilliseconds(1500)));
            leftAnim.EasingFunction = nonLinearEasingFunction;
            leftAnim.BeginTime = TimeSpan.FromSeconds(prevFinishTime);
            Storyboard.SetTarget(leftAnim, arrowToBeRotate);
            Storyboard.SetTargetProperty(leftAnim, new PropertyPath("(Canvas.Left)"));
            storyboard.Children.Add(leftAnim);

            DoubleAnimation topAnim = new DoubleAnimation(srcY - 17.5, new Duration(TimeSpan.FromMilliseconds(1500)));
            topAnim.EasingFunction = nonLinearEasingFunction;
            topAnim.BeginTime = TimeSpan.FromSeconds(prevFinishTime);
            Storyboard.SetTarget(topAnim, arrowToBeRotate);
            Storyboard.SetTargetProperty(topAnim, new PropertyPath("(Canvas.Top)"));
            storyboard.Children.Add(topAnim);

            arrowToBeRotate.currentCanvasLeft = srcX;
            arrowToBeRotate.currentCanvasTop = srcY - 17.5;

            /*
            RotateTransform rotateTransform = new RotateTransform(arrowToBeRotate.currentAngle, 0, 17.5);
            ScaleTransform scaleTransform = new ScaleTransform(arrowToBeRotate.currentScaleX, 1, 0, 17.5);
            TransformGroup group = new TransformGroup();
            group.Children.Add(scaleTransform);
            group.Children.Add(rotateTransform);
            arrowToBeRotate.RenderTransform = group;
            */

            DoubleAnimation scaleXAnim = new DoubleAnimation(arrowToBeRotate.currentScaleX, scaleRate, new Duration(TimeSpan.FromMilliseconds(1500)));
            scaleXAnim.EasingFunction = nonLinearEasingFunction;
            scaleXAnim.BeginTime = TimeSpan.FromSeconds(prevFinishTime);
            Storyboard.SetTarget(scaleXAnim, arrowToBeRotate);
            Storyboard.SetTargetProperty(scaleXAnim, new PropertyPath("RenderTransform.Children[0].ScaleX"));
            storyboard.Children.Add(scaleXAnim);
            //scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleXAnim);
            arrowToBeRotate.currentScaleX = scaleRate;

            /*
            DoubleAnimation scaleYAnim = new DoubleAnimation(arrowToBeRotate.ScaleTrans.ScaleY, scaleRate, new Duration(TimeSpan.FromMilliseconds(1500)));
            scaleYAnim.EasingFunction = nonLinearEasingFunction;
            scaleYAnim.BeginTime = TimeSpan.FromSeconds(prevFinishTime);

            Storyboard.SetTarget(scaleYAnim, arrowToBeRotate.ScaleTrans);
            Storyboard.SetTargetProperty(scaleYAnim, new PropertyPath("ScaleY"));
            storyboard.Children.Add(scaleYAnim);
            */

            DoubleAnimation angleAnimation = new DoubleAnimation(arrowToBeRotate.currentAngle, targetAngle, new Duration(TimeSpan.FromMilliseconds(1500)));
            angleAnimation.EasingFunction = nonLinearEasingFunction;
            angleAnimation.BeginTime = TimeSpan.FromSeconds(prevFinishTime);
            Storyboard.SetTarget(angleAnimation, arrowToBeRotate);
            Storyboard.SetTargetProperty(angleAnimation, new PropertyPath("RenderTransform.Children[1].Angle"));
            storyboard.Children.Add(angleAnimation);
            //rotateTransform.BeginAnimation(RotateTransform.AngleProperty, angleAnimation);
            arrowToBeRotate.currentAngle = targetAngle;

            return prevFinishTime + 1.5;
        }

        public List<VisualPointer> GetRelatedPointers(Dictionary<string, VisualPointer> generalVisualPointers)
        {
            List<VisualPointer> resultList = new List<VisualPointer>();
            foreach (VisualPointer visualPointer in generalVisualPointers.Values)
            {
                if (visualPointer.pointingNode == this)
                {
                    resultList.Add(visualPointer);
                }
            }
            return resultList;
        }
    }
}

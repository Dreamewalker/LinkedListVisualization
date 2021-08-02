using LinkedListVisualization.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace LinkedListVisualization
{
    class Node
    {
        public int value;
        public Node prevPtr = null;
        public Node nextPtr = null;
        public ListElement listElement = null;
        public Arrow prevArrow = null;
        public Arrow nextArrow = null;

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
        public double InitialDraw(Canvas canvas, Storyboard storyboard, bool isBidirection)
        {
            if (listElement == null)
            {
                return 0;
            }
            double finishTime = 0;
            double canvasLeft = 785 + 500;
            double canvasTop = 200 + 400;
            Node currentPtr = this;

            Canvas.SetLeft(currentPtr.listElement, canvasLeft);
            Canvas.SetTop(currentPtr.listElement, canvasTop);
            canvas.Children.Add(currentPtr.listElement);
            finishTime = currentPtr.listElement.Show(storyboard, 0);
            canvasLeft += 190;

            while (currentPtr.nextPtr != null)
            {
                Canvas.SetLeft(currentPtr.nextPtr.listElement, canvasLeft);
                Canvas.SetTop(currentPtr.nextPtr.listElement, canvasTop);
                canvas.Children.Add(currentPtr.nextPtr.listElement);
                finishTime = currentPtr.nextPtr.listElement.Show(storyboard, 0);

                Node.SetArrowNoAnim(currentPtr, currentPtr.nextPtr, currentPtr.nextArrow);
                canvas.Children.Add(currentPtr.nextArrow);
                finishTime = currentPtr.nextArrow.Expand(storyboard, 0);


                if (currentPtr.prevPtr != null)
                {
                    Node.SetArrowNoAnim(currentPtr, currentPtr.prevPtr, currentPtr.prevArrow);
                    // Canvas.SetLeft(currentPtr.prevArrow, canvasLeft + 80 - 160);
                    // Canvas.SetTop(currentPtr.prevArrow, canvasTop + 20);
                    canvas.Children.Add(currentPtr.prevArrow);
                    finishTime = currentPtr.prevArrow.Expand(storyboard, 0);
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
                finishTime = currentPtr.prevArrow.Expand(storyboard, 0);
            }

            /*
            do
            {
                Canvas.SetLeft(currentPtr.listElement, canvasLeft);
                Canvas.SetTop(currentPtr.listElement, canvasTop);
                canvas.Children.Add(currentPtr.listElement);
                finishTime = currentPtr.listElement.Show(storyboard, 0);


                if (currentPtr.nextPtr != null)
                {
                    Node.SetArrowNoAnim(currentPtr, currentPtr.nextPtr, currentPtr.nextArrow);
                    
                    if (isBidirection)
                    {
                        Canvas.SetLeft(currentPtr.nextArrow, canvasLeft + 40);
                    }
                    else
                    {
                        Canvas.SetLeft(currentPtr.nextArrow, canvasLeft + 30);
                    }
                    Canvas.SetTop(currentPtr.nextArrow, canvasTop + 20);
                    
                    canvas.Children.Add(currentPtr.nextArrow);
                    finishTime = currentPtr.nextArrow.Expand(storyboard, 0);
                }

                if (currentPtr.prevPtr != null)
                {
                    Node.SetArrowNoAnim(currentPtr, currentPtr.prevPtr, currentPtr.prevArrow);
                    // Canvas.SetLeft(currentPtr.prevArrow, canvasLeft + 80 - 160);
                    // Canvas.SetTop(currentPtr.prevArrow, canvasTop + 20);
                    canvas.Children.Add(currentPtr.prevArrow);
                    finishTime = currentPtr.prevArrow.Expand(storyboard, 0);
                }
                canvasLeft += 190;
                currentPtr = currentPtr.nextPtr;
            } while (currentPtr != null);
            */

            return finishTime;
        }

        public double CloseAnim(Storyboard storyboard)
        {
            if (listElement == null)
            {
                return 0;
            }
            double finishTime = 0;
            Node currentPtr = this;
            do
            {
                finishTime = currentPtr.listElement.Close(storyboard, 0);


                if (currentPtr.nextPtr != null)
                {
                    finishTime = currentPtr.nextArrow.Close(storyboard, 0);
                }

                if (currentPtr.prevPtr != null)
                {
                    finishTime = currentPtr.prevArrow.Close(storyboard, 0);
                }
                currentPtr = currentPtr.nextPtr;
            } while (currentPtr != null);

            return finishTime;
        }

        public void CreateNextNode(int nodeValue, bool isBidirection)
        {
            nextPtr = new Node(nodeValue, 155, 155, 155, isBidirection ? this : null);
            nextArrow = new Arrow();
            nextArrow.Rotation.Angle = 180;
        }

        public static void SetArrowNoAnim(Node srcNode, Node dstNode, Arrow arrowToBeSet)
        {
            double srcX = Canvas.GetLeft(srcNode.listElement);
            double srcY = Canvas.GetTop(srcNode.listElement);

            double dstX = Canvas.GetLeft(dstNode.listElement);
            double dstY = Canvas.GetTop(dstNode.listElement);

            double scaleRate = Math.Sqrt(Math.Pow(srcX - dstX, 2) + Math.Pow(srcY - dstY, 2)) / 190.0;

            arrowToBeSet.ScaleTrans.ScaleX = scaleRate;
            arrowToBeSet.ScaleTrans.ScaleY = scaleRate;

            double tanValue = (srcY - dstY) / (srcX - dstX);
            double rotAngle = Math.Atan2(dstY - srcY, dstX - srcX) / 2 / Math.PI * 360 - 180;

            arrowToBeSet.Rotation.Angle = rotAngle;

            Canvas.SetLeft(arrowToBeSet, srcX + 40 - 190 * scaleRate);
            Canvas.SetTop(arrowToBeSet, srcY + 40 - 35 * scaleRate / 2);
        }
    }
}

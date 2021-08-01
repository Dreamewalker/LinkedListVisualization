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
        public double InitialDraw(Canvas canvas, Storyboard storyboard)
        {
            if (listElement == null)
            {
                return 0;
            }
            double finishTime = 0;
            double canvasLeft = 785 + 500;
            double canvasTop = 200 + 400;
            Node currentPtr = this;
            do
            {
                Canvas.SetLeft(currentPtr.listElement, canvasLeft);
                Canvas.SetTop(currentPtr.listElement, canvasTop);
                canvas.Children.Add(currentPtr.listElement);
                finishTime = currentPtr.listElement.Show(storyboard, 0);


                if (currentPtr.nextPtr != null)
                {
                    Canvas.SetLeft(currentPtr.nextArrow, canvasLeft + 90);
                    Canvas.SetTop(currentPtr.nextArrow, canvasTop + 20);
                    canvas.Children.Add(currentPtr.nextArrow);
                    finishTime = currentPtr.nextArrow.Expand(storyboard, 0);
                }
                canvasLeft += 160;
                currentPtr = currentPtr.nextPtr;
            } while (currentPtr != null);

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
                currentPtr = currentPtr.nextPtr;
            } while (currentPtr != null);

            return finishTime;
        }

        public void CreateNextNode(int nodeValue, bool isBidirection)
        {
            nextPtr = new Node(nodeValue, 155, 155, 155, isBidirection ? this : null);
            nextArrow = new Arrow();
        }
    }
}

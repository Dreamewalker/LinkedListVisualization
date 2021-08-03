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
        public VisualPointer(Canvas canvas, Node pointerTarget)
        {
            InitializeComponent();

            double targetX = Canvas.GetLeft(pointerTarget.listElement);
            double targetY = Canvas.GetTop(pointerTarget.listElement);

            Canvas.SetLeft(this, targetX + 40 - 45);
            Canvas.SetTop(this, targetY + 40);
            canvas.Children.Add(this);
        }

    }
}

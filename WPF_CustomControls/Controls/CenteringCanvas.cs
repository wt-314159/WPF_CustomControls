using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPF_CustomControls.Controls
{
    public class CenteringCanvas : Canvas
    {
        public CenteringCanvas() : base() 
        { }

        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            foreach (UIElement child in base.InternalChildren)
            {
                if (child == null)
                    continue;

                double xOffset = 0.0;
                var left = GetLeft(child);
                if (!double.IsNaN(left))
                {
                    xOffset = -left;
                }
                else
                {
                    var right = GetRight(child);
                    if (!double.IsNaN(right))
                    {
                        xOffset = right;
                    }
                }
                double x = ((arrangeSize.Width - child.DesiredSize.Width) / 2) + xOffset;

                double yOffset = 0.0;
                var top = GetTop(child);
                if (!double.IsNaN(top))
                {
                    yOffset = -top;
                }
                else
                {
                    var bottom = GetBottom(child);
                    if (!double.IsNaN(bottom))
                    {
                        yOffset = bottom;
                    }
                }
                double y = ((arrangeSize.Height - child.DesiredSize.Height) / 2) + yOffset;

                child.Arrange(new Rect(new Point(x, y), child.DesiredSize));
            }
            return arrangeSize;
        }
    }
}

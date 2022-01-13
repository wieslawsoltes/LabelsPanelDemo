using System;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace LabelsPanelDemo.Controls
{
    public class LabelsPanel : VirtualizingStackPanel
    {
        public static readonly StyledProperty<Control?> EllipsisControlProperty =
            AvaloniaProperty.Register<LabelsPanel, Control?>(nameof(EllipsisControl));

        //public static readonly StyledProperty<double> SpacingProperty = 
        //    AvaloniaProperty.Register<CellPanel, double>(nameof(Spacing));

        public Control? EllipsisControl
        {
            get => GetValue(EllipsisControlProperty);
            set => SetValue(EllipsisControlProperty, value);
        }

        //public double Spacing
        //{
        //    get => GetValue(SpacingProperty);
        //    set => SetValue(SpacingProperty, value);
        //}

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            if (EllipsisControl is { } ellipsisControl)
            {
                ((ISetLogicalParent)ellipsisControl).SetParent(this);
                VisualChildren.Add(ellipsisControl);
                LogicalChildren.Add(ellipsisControl);
            }

            base.OnAttachedToVisualTree(e);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            // Debug.WriteLine($"[MeasureOverride] availableSize='{availableSize}', Children='{Children.Count}'");

            var ellipsis = 0.0;
            if (EllipsisControl is { })
            {
                EllipsisControl.Measure(availableSize);
                ellipsis = EllipsisControl.DesiredSize.Width;
            }
            /*
            var width = 0.0;
            var height = 0.0;

            foreach (var child in Children)
            {
                child.Measure(availableSize);
                width += child.DesiredSize.Width + Spacing;
                height = Math.Max(height, child.DesiredSize.Height);
            }

            width += ellipsis;

            var size = new Size(availableSize.Width, height);

            Debug.WriteLine($"[MeasureOverride] size='{size}', Children='{Children.Count}'");
            */
            //return base.MeasureOverride(size);
            //return size;
            //return base.MeasureOverride(availableSize);
            return base.MeasureOverride(availableSize.WithWidth(availableSize.Width + ellipsis));
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            //Debug.WriteLine($"[ArrangeOverride] availableSize='{finalSize}', Children='{Children.Count}'");

            var spacing = Spacing;
            //var spacing = 0.0;
            
            var ellipsisWidth = 0.0;
            if (EllipsisControl is { })
            {
                ellipsisWidth = EllipsisControl.DesiredSize.Width;
            }

            var width = 0.0;
            var height = 0.0;

            var finalWidth = finalSize.Width;

            bool showEllipsis = false;
            int totalChildren = Children.Count;
            int count = 0;
            for (var i = 0; i < totalChildren; i++)
            {
                var child = Children[i];
                var childWidth = child.DesiredSize.Width /* + spacing */;

                if (width + childWidth > finalWidth)
                {
                    while (true)
                    {
                        if (width + ellipsisWidth > finalWidth)
                        {
                            var previous = i - 1;
                            if (previous >= 0)
                            {
                                var previousChild = Children[previous];
                                count--;
                                width -= previousChild.DesiredSize.Width + spacing;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                    showEllipsis = true;
                    if (EllipsisControl is { })
                    {
                        width += EllipsisControl.DesiredSize.Width;
                    }
                    
                    break;
                }

                width += child.DesiredSize.Width + spacing;
                height = Math.Max(height, child.DesiredSize.Height);
                count++;
            }

            var offset = 0.0;
            for (var i = 0; i < totalChildren; i++)
            {
                var child = Children[i];
                if (i < count)
                {
                    //if (!child.IsVisible)
                    //{
                    //    child.IsVisible = true;
                    //}
                    var rect = new Rect(offset, 0.0, child.DesiredSize.Width, height);
                    child.Arrange(rect);
                    offset += child.DesiredSize.Width + spacing;
                }
                else
                {
                    //child.IsVisible = false;
                    child.Arrange(new Rect(-10000, -10000, 0, 0));
                }
            }

            if (EllipsisControl is { })
            {
                if (showEllipsis)
                {                    
                    //if (!EllipsisControl.IsVisible)
                    //{
                    //    EllipsisControl.IsVisible = true;
                    //}
                    var rect = new Rect(offset, 0.0, EllipsisControl.DesiredSize.Width, height);
                    EllipsisControl.Arrange(rect);
                }
                else
                {
                    EllipsisControl.Arrange(new Rect(-10000, -10000, 0, 0));
                    //EllipsisControl.IsVisible = false;
                }
            }

            var size = new Size(width, height);
            //Debug.WriteLine($"[ArrangeOverride] size='{size}', count='{count}'");

            return size;
            // return base.ArrangeOverride(size);
            // return base.ArrangeOverride(finalSize);
        }
    }
}

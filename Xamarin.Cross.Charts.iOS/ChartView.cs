using Foundation;
using SkiaSharp;
using SkiaSharp.Views.iOS;
using System;
using UIKit;

namespace Xamarin.Cross.Charts.iOS
{
    [Register("ChartView")]
    public class ChartView : SKCanvasView
    {
        public ChartView()
        {
            Initialize();
        }

        [Preserve]
        public ChartView(IntPtr handle) : base(handle)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            Initialize();
        }

        private void Initialize()
        {
            BackgroundColor = UIColor.Clear;
            PaintSurface += OnPaintCanvas;
        }

        private InvalidatedWeakEventHandler<ChartView> handler;

        private Chart chart;

        public Chart Chart
        {
            get => chart;
            set
            {
                if (chart != value)
                {
                    if (chart != null)
                    {
                        handler.Dispose();
                        handler = null;
                    }

                    chart = value;
                    InvalidateChart();

                    if (chart != null)
                    {
                        handler = chart.ObserveInvalidate(this, (view) => view.InvalidateChart());
                    }
                }
            }
        }
        private void InvalidateChart() => SetNeedsDisplayInRect(Bounds);

        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            if (chart != null)
            {
                chart.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
            }
            else
            {
                e.Surface.Canvas.Clear(SKColors.Transparent);
            }
        }
    }
}

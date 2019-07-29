using Foundation;
using SkiaSharp;
using SkiaSharp.Views.iOS;
using System;
using UIKit;
using Xamarin.Cross.Charts.Utilities;

namespace Xamarin.Cross.Charts.iOS
{
    [Register("ChartView")]
    public class ChartView : SKCanvasView
    {
        public ChartView() { Initialize(); }

        [Preserve]
        public ChartView(IntPtr handle) : base(handle) { Initialize(); }

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

        private WeakEventHandler<ChartView> _Handler;

        private Chart _Chart;

        public Chart Chart
        {
            get => _Chart;
            set
            {
                if (_Chart != value)
                {
                    if (_Chart != null)
                    {
                        _Handler.Dispose();
                        _Handler = null;
                    }

                    _Chart = value;
                    InvalidateChart();

                    if (_Chart != null)
                    {
                        _Handler = _Chart.ObserveInvalidate(this, (view) => view.InvalidateChart());
                    }
                }
            }
        }
        private void InvalidateChart() => SetNeedsDisplayInRect(Bounds);

        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            if (_Chart != null)
            {
                _Chart.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
            }
            else
            {
                e.Surface.Canvas.Clear(SKColors.Transparent);
            }
        }
    }
}

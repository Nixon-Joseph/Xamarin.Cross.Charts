using SkiaSharp;
using SkiaSharp.Views.UWP;
using Windows.UI.Xaml;

namespace Xamarin.Cross.Charts.UWP
{
    public class ChartView : SKXamlCanvas
    {
        public ChartView()
        {
            PaintSurface += OnPaintCanvas;
        }

        public static readonly DependencyProperty ChartProperty = DependencyProperty.Register(nameof(Chart), typeof(ChartView), typeof(Chart), new PropertyMetadata(null, new PropertyChangedCallback(OnChartChanged)));

        private WeakEventHandler<ChartView> _Handler;

        private Chart _Chart;

        public Chart Chart
        {
            get { return (Chart)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }

        private static void OnChartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartView _this && e.NewValue is Chart newChart)
            {
                if (_this._Chart != null)
                {
                    _this._Handler.Dispose();
                    _this._Handler = null;
                }

                _this._Chart = newChart;
                _this.Invalidate();

                if (_this._Chart != null)
                {
                    _this._Handler = _this._Chart.ObserveInvalidate(_this, (v) => v.Invalidate());
                }
            }
        }

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

using Android.Content;
using Android.Runtime;
using Android.Util;
using SkiaSharp.Views.Android;
using System;
using Xamarin.Cross.Charts.Utilities;

namespace Xamarin.Cross.Charts.Droid
{
    public class ChartView : SKCanvasView
    {
        public ChartView(Context context) : base(context) { Init(); }
        public ChartView(Context context, IAttributeSet attributes) : base(context, attributes) { Init(); }
        public ChartView(Context context, IAttributeSet attributes, int defStyleAtt) : base(context, attributes, defStyleAtt) { Init(); }
        public ChartView(IntPtr ptr, JniHandleOwnership jni) : base(ptr, jni) { Init(); }
        private void Init() { PaintSurface += OnPaintCanvas; }

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
                    Invalidate();

                    if (_Chart != null)
                    {
                        _Handler = _Chart.ObserveInvalidate(this, (view) => view.Invalidate());
                    }
                }
            }
        }

        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            if (_Chart != null)
            {
                _Chart.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
            }
        }
    }
}

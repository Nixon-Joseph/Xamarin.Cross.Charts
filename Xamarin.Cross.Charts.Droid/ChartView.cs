using Android.Content;
using Android.Runtime;
using Android.Util;
using SkiaSharp.Views.Android;
using System;

namespace Xamarin.Cross.Charts.Droid
{
    public class ChartView : SKCanvasView
    {
        public ChartView(Context context) : base(context)
        {
            PaintSurface += OnPaintCanvas;
        }

        public ChartView(Context context, IAttributeSet attributes) : base(context, attributes)
        {
            PaintSurface += OnPaintCanvas;
        }

        public ChartView(Context context, IAttributeSet attributes, int defStyleAtt) : base(context, attributes, defStyleAtt)
        {
            PaintSurface += OnPaintCanvas;
        }

        public ChartView(IntPtr ptr, JniHandleOwnership jni) : base(ptr, jni)
        {
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
                    Invalidate();

                    if (chart != null)
                    {
                        handler = chart.ObserveInvalidate(this, (view) => view.Invalidate());
                    }
                }
            }
        }

        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            if (chart != null)
            {
                chart.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
            }
        }
    }
}

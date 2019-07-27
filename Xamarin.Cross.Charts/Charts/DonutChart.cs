using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Cross.Charts.Helpers;

namespace Xamarin.Cross.Charts.Charts
{
    public class DonutChart : Chart
    {
        public float HoleRadius { get; set; } = 0.5f;

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            if (Entries != null)
            {
                DrawCaption(canvas, width, height);
                using (new SKAutoCanvasRestore(canvas))
                {
                    canvas.Translate(width / 2, height / 2);
                    var sumValue = Entries.Sum(x => Math.Abs(x.Value));
                    var radius = (Math.Min(width, height) - (2 * Margin)) / 2;

                    var start = 0.0f;
                    for (int i = 0; i < Entries.Count(); i++)
                    {
                        var entry = Entries.ElementAt(i);
                        var end = start + ((Math.Abs(entry.Value) / sumValue) * AnimationProgress);

                        // Sector
                        var path = RadialHelpers.CreateSectorPath(start, end, radius, radius * HoleRadius);
                        using (var paint = new SKPaint
                        {
                            Style = SKPaintStyle.Fill,
                            Color = entry.Color,
                            IsAntialias = true,
                        })
                        {
                            canvas.DrawPath(path, paint);
                        }

                        start = end;
                    }
                }
            }
        }

        private void DrawCaption(SKCanvas canvas, int width, int height)
        {
            var sumValue = Entries.Sum(x => Math.Abs(x.Value));
            var rightValues = new List<ChartEntry>();
            var leftValues = new List<ChartEntry>();

            int i = 0;
            var current = 0.0f;
            while (i < Entries.Count() && (current < sumValue / 2))
            {
                var entry = Entries.ElementAt(i);
                rightValues.Add(entry);
                current += Math.Abs(entry.Value);
                i++;
            }

            while (i < Entries.Count())
            {
                var entry = Entries.ElementAt(i);
                leftValues.Add(entry);
                i++;
            }

            leftValues.Reverse();

            DrawCaptionElements(canvas, width, height, rightValues, false);
            DrawCaptionElements(canvas, width, height, leftValues, true);
        }
    }
}

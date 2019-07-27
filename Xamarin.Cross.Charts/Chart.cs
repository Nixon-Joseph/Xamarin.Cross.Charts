using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Cross.Charts.Extensions;
using Xamarin.Cross.Charts.Utilities;

namespace Xamarin.Cross.Charts
{
    public abstract class Chart : INotifyPropertyChanged
    {
        private IEnumerable<ChartEntry> entries;
        private float animationProgress, margin = 20, labelTextSize = 16;
        private SKColor backgroundColor = SKColors.White;
        private SKColor labelColor = SKColors.Gray;
        private SKTypeface typeface;
        private float? internalMinValue, internalMaxValue;
        private bool isAnimated = true, isAnimating = false;
        private TimeSpan animationDuration = TimeSpan.FromSeconds(1.5f);
        private Task invalidationPlanification;
        private CancellationTokenSource animationCancellation;

        public Chart() { PropertyChanged += OnPropertyChanged; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler Invalidated;
        public bool IsAnimated
        {
            get => isAnimated;
            set
            {
                if (Set(ref isAnimated, value))
                {
                    if (!value)
                    {
                        AnimationProgress = 1;
                    }
                }
            }
        }
        public bool IsAnimating
        {
            get => isAnimating;
            private set => Set(ref isAnimating, value);
        }
        public TimeSpan AnimationDuration
        {
            get => animationDuration;
            set => Set(ref animationDuration, value);
        }
        public float Margin
        {
            get => margin;
            set => Set(ref margin, value);
        }
        public float AnimationProgress
        {
            get => animationProgress;
            set
            {
                value = Math.Min(1, Math.Max(value, 0));
                Set(ref animationProgress, value);
            }
        }
        public float LabelTextSize
        {
            get => labelTextSize;
            set => Set(ref labelTextSize, value);
        }
        public SKTypeface Typeface
        {
            get => typeface;
            set => Set(ref typeface, value);
        }
        public SKColor BackgroundColor
        {
            get => backgroundColor;
            set => Set(ref backgroundColor, value);
        }
        public SKColor LabelColor
        {
            get => labelColor;
            set => Set(ref labelColor, value);
        }
        public IEnumerable<ChartEntry> Entries
        {
            get => entries;
            set => UpdateEntries(value);
        }
        public float MinValue
        {
            get
            {
                if (!Entries.Any())
                {
                    return 0;
                }

                if (InternalMinValue == null)
                {
                    return Math.Min(0, Entries.Min(x => x.Value));
                }

                return Math.Min(InternalMinValue.Value, Entries.Min(x => x.Value));
            }

            set => InternalMinValue = value;
        }
        public float MaxValue
        {
            get
            {
                if (!Entries.Any())
                {
                    return 0;
                }

                if (InternalMaxValue == null)
                {
                    return Math.Max(0, Entries.Max(x => x.Value));
                }

                return Math.Max(InternalMaxValue.Value, Entries.Max(x => x.Value));
            }

            set => InternalMaxValue = value;
        }
        protected float? InternalMinValue
        {
            get => internalMinValue;
            set
            {
                if (Set(ref internalMinValue, value))
                {
                    RaisePropertyChanged(nameof(MinValue));
                }
            }
        }
        protected float? InternalMaxValue
        {
            get => internalMaxValue;
            set
            {
                if (Set(ref internalMaxValue, value))
                {
                    RaisePropertyChanged(nameof(MaxValue));
                }
            }
        }
        public void Draw(SKCanvas canvas, int width, int height)
        {
            canvas.Clear(BackgroundColor);

            DrawContent(canvas, width, height);
        }
        public abstract void DrawContent(SKCanvas canvas, int width, int height);
        protected void DrawCaptionElements(SKCanvas canvas, int width, int height, List<ChartEntry> entries, bool isLeft)
        {
            var totalMargin = 2 * Margin;
            var availableHeight = height - (2 * totalMargin);
            var x = isLeft ? Margin : (width - Margin - LabelTextSize);
            var ySpace = (availableHeight - LabelTextSize) / ((entries.Count <= 1) ? 1 : entries.Count - 1);

            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries.ElementAt(i);
                var y = totalMargin + (i * ySpace);
                if (entries.Count <= 1)
                {
                    y += (availableHeight - LabelTextSize) / 2;
                }

                var hasLabel = !string.IsNullOrEmpty(entry.Label);
                var hasValueLabel = !string.IsNullOrEmpty(entry.ValueLabel);

                if (hasLabel || hasValueLabel)
                {
                    var hasOffset = hasLabel && hasValueLabel;
                    var captionMargin = LabelTextSize * 0.60f;
                    var space = hasOffset ? captionMargin : 0;
                    var captionX = isLeft ? Margin : width - Margin - LabelTextSize;
                    var valueColor = entry.Color.WithAlpha((byte)(entry.Color.Alpha * AnimationProgress));
                    var labelColor = entry.TextColor.WithAlpha((byte)(entry.TextColor.Alpha * AnimationProgress));

                    using (var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = valueColor
                    })
                    {
                        var rect = SKRect.Create(captionX, y, LabelTextSize, LabelTextSize);
                        canvas.DrawRect(rect, paint);
                    }

                    if (isLeft)
                    {
                        captionX += LabelTextSize + captionMargin;
                    }
                    else
                    {
                        captionX -= captionMargin;
                    }

                    canvas.DrawCaptionLabels(entry.Label, labelColor, entry.ValueLabel, valueColor, LabelTextSize, new SKPoint(captionX, y + (LabelTextSize / 2)), isLeft ? SKTextAlign.Left : SKTextAlign.Right, Typeface);
                }
            }
        }
        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AnimationProgress):
                    Invalidate();
                    break;
                case nameof(LabelTextSize):
                case nameof(MaxValue):
                case nameof(MinValue):
                case nameof(BackgroundColor):
                    PlanifyInvalidate();
                    break;
                default:
                    break;
            }
        }
        protected void Invalidate() => Invalidated?.Invoke(this, EventArgs.Empty);
        protected async void PlanifyInvalidate()
        {
            if (invalidationPlanification != null)
            {
                await invalidationPlanification;
            }
            else
            {
                invalidationPlanification = Task.Delay(200);
                await invalidationPlanification;
                Invalidate();
                invalidationPlanification = null;
            }
        }

        public InvalidatedWeakEventHandler<TTarget> ObserveInvalidate<TTarget>(TTarget target, Action<TTarget> onInvalidate) where TTarget : class
        {
            var weakHandler = new InvalidatedWeakEventHandler<TTarget>(this, target, onInvalidate);
            weakHandler.Subsribe();
            return weakHandler;
        }

        public async Task AnimateAsync(bool entrance, CancellationToken token = default(CancellationToken))
        {
            var watch = new Stopwatch();

            var start = entrance ? 0 : 1;
            var end = entrance ? 1 : 0;
            var range = end - start;

            AnimationProgress = start;
            IsAnimating = true;

            watch.Start();

            var source = new TaskCompletionSource<bool>();
            var timer = TimerUtil.Create();

            timer.Start(TimeSpan.FromSeconds(1.0 / 30), () =>
            {
                if (token.IsCancellationRequested)
                {
                    source.SetCanceled();
                    return false;
                }

                var progress = (float)(watch.Elapsed.TotalSeconds / animationDuration.TotalSeconds);
                progress = entrance ? Ease.In(progress) : Ease.Out(progress);
                AnimationProgress = start + (progress * (end - start));

                var shouldContinue = (entrance && AnimationProgress < 1) || (!entrance && AnimationProgress > 0);

                if (!shouldContinue)
                {
                    source.SetResult(true);
                }

                return shouldContinue;
            });

            await source.Task;

            watch.Stop();
            IsAnimating = false;
        }

        private async void UpdateEntries(IEnumerable<ChartEntry> value)
        {
            try
            {
                if (animationCancellation != null)
                {
                    animationCancellation.Cancel();
                }

                var cancellation = new CancellationTokenSource();
                animationCancellation = cancellation;

                if (!cancellation.Token.IsCancellationRequested && entries != null && IsAnimated)
                {
                    await AnimateAsync(false, cancellation.Token);
                }
                else
                {
                    AnimationProgress = 0;
                }

                if (Set(ref entries, value))
                {
                    RaisePropertyChanged(nameof(MinValue));
                    RaisePropertyChanged(nameof(MaxValue));
                }

                if (!cancellation.Token.IsCancellationRequested && entries != null && IsAnimated)
                {
                    await AnimateAsync(true, cancellation.Token);
                }
                else
                {
                    AnimationProgress = 1;
                }
            }
            catch
            {
                if (Set(ref entries, value))
                {
                    RaisePropertyChanged(nameof(MinValue));
                    RaisePropertyChanged(nameof(MaxValue));
                }

                Invalidate();
            }
            finally
            {
                animationCancellation = null;
            }
        }

        protected void RaisePropertyChanged([CallerMemberName]string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected bool Set<T>(ref T field, T value, [CallerMemberName]string property = null)
        {
            if (!EqualityComparer<T>.Equals(field, property))
            {
                field = value;
                RaisePropertyChanged(property);
                return true;
            }

            return false;
        }
    }

    public enum PointMode
    {
        None,
        Circle,
        Square,
    }
}
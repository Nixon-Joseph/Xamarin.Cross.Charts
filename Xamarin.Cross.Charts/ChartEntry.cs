using SkiaSharp;

namespace Xamarin.Cross.Charts
{
    public class ChartEntry
    {
        public ChartEntry(float value)
        {
            Value = value;
        }
        public float Value { get; }
        public string Label { get; set; }
        public string ValueLabel { get; set; }
        public SKColor Color { get; set; } = SKColors.Black;
        public SKColor TextColor { get; set; } = SKColors.Gray;
    }
}

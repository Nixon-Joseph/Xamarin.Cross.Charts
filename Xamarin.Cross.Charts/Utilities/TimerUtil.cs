using System;
using System.Threading.Tasks;

namespace Xamarin.Cross.Charts.Utilities
{
    public static class TimerUtil
    {
        public static Func<ITimer> Create { get; set; } = () => new DelayTimer();
    }

    public interface ITimer
    {
        void Start(TimeSpan interval, Func<bool> step);
    }

    public class DelayTimer : ITimer
    {
        public async void Start(TimeSpan interval, Func<bool> step)
        {
            var shouldContinue = step();

            while (shouldContinue)
            {
                await Task.Delay(interval);
                shouldContinue = step();
            }
        }
    }
}

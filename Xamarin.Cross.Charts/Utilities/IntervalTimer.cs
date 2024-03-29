﻿using System;
using System.Threading.Tasks;

namespace Xamarin.Cross.Charts.Utilities
{
    public class IntervalTimer
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

using System;

namespace Xamarin.Cross.Charts
{
    public class InvalidatedWeakEventHandler<TTarget> : IDisposable where TTarget : class
    {
        private readonly WeakReference<Chart> SourceReference;
        private readonly WeakReference<TTarget> TargetReference;
        private readonly Action<TTarget> TargetMethod;
        private bool isSubscribed;

        public InvalidatedWeakEventHandler(Chart source, TTarget target, Action<TTarget> targetMethod)
        {
            SourceReference = new WeakReference<Chart>(source);
            TargetReference = new WeakReference<TTarget>(target);
            TargetMethod = targetMethod;
        }
        public bool IsAlive => SourceReference.TryGetTarget(out Chart s) && TargetReference.TryGetTarget(out TTarget t);
        public void Subsribe()
        {
            if (!isSubscribed && SourceReference.TryGetTarget(out Chart source))
            {
                source.Invalidated += OnEvent;
                isSubscribed = true;
            }
        }
        public void Unsubscribe()
        {
            if (isSubscribed)
            {
                if (SourceReference.TryGetTarget(out Chart source))
                {
                    source.Invalidated -= OnEvent;
                }

                isSubscribed = false;
            }
        }
        public void Dispose() => Unsubscribe();

        private void OnEvent(object sender, EventArgs args)
        {
            if (TargetReference.TryGetTarget(out TTarget t))
            {
                TargetMethod(t);
            }
            else
            {
                Unsubscribe();
            }
        }
    }
}

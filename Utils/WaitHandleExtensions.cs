using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils.Core
{
    public static class WaitHandleExtensions
    {
        public static Task AsTask(this WaitHandle handle)
        {
            return AsTask(handle, Timeout.InfiniteTimeSpan);
        }

        public static Task AsTask(this WaitHandle handle, TimeSpan timeout)
        {
            var completionSource = new TaskCompletionSource<object>();
            var registration = ThreadPool.RegisterWaitForSingleObject(handle, (state, timedOut) =>
            {
                var localCompletionSource = (TaskCompletionSource<object>)state;

                if (timedOut)
                {
                    localCompletionSource.TrySetCanceled();
                }
                else
                {
                    localCompletionSource.TrySetResult(null);
                }

            }, completionSource, timeout, executeOnlyOnce: true);

            completionSource.Task.ContinueWith((_, state) => ((RegisteredWaitHandle)state).Unregister(null), registration, TaskScheduler.Default);

            return completionSource.Task;
        }

        public static Task<T> AsTask<T>(this WaitHandle handle, TimeSpan timeout)
        {
            var completionSource = new TaskCompletionSource<T>();
            var registration = ThreadPool.RegisterWaitForSingleObject(handle, (state, timedOut) =>
            {
                var localCompletionSource = (TaskCompletionSource<T>)state;

                if (timedOut)
                {
                    localCompletionSource.TrySetCanceled();
                }
                else
                {
                    localCompletionSource.TrySetResult(default(T));
                }

            }, completionSource, timeout, executeOnlyOnce: true);

            completionSource.Task.ContinueWith((_, state) => ((RegisteredWaitHandle)state).Unregister(null), registration, TaskScheduler.Default);

            return completionSource.Task;
        }
    }
}

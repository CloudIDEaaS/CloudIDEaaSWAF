using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Testing
{
    public static class PromiseExtensions
    {
        public static Task<object> ToPromiseTask(this object promise)
        {
            var source = new TaskCompletionSource<object>();
            Action<object> onResolved = result => source.SetResult(result);
            Action<dynamic> onRejected = error => source.SetException(new Exception(error.toString()));

            ((dynamic)promise).then(onResolved, onRejected);

            return source.Task;
        }
    }
}

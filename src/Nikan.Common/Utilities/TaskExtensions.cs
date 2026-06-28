using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Nikan.Common.Utilities
{
    public static class TaskExtensions
    {
        public static ConfiguredTaskAwaitable ConfigureAwaitFalse(this Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            return task.ConfigureAwait(false);
        }

        public static ConfiguredTaskAwaitable<TResult> ConfigureAwaitFalse<TResult>(this Task<TResult> task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            return task.ConfigureAwait(false);
        }
    }
}

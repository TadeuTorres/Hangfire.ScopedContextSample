using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire.Annotations;
using Hangfire.Common;
using Hangfire.States;

namespace WebApplication2.Hangfire
{
    public static class BackgroundJobClientWithContextExtensions
    {
        public static string EnqueueWithContext(
            [NotNull] this IBackgroundJobClientWithContext client,
            [NotNull, InstantHandle] Expression<Action> methodCall,
            object context,
            string queue = null
            )
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var state = queue != null ? new EnqueuedState(queue) : new EnqueuedState();

            return client.Create(Job.FromExpression(methodCall), state, context);
        }

        public static string EnqueueWithContext(
            [NotNull] this IBackgroundJobClientWithContext client,
            [NotNull, InstantHandle] Expression<Func<Task>> methodCall,
            object context,
            string queue = null)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var state = queue != null ? new EnqueuedState(queue) : new EnqueuedState();

            return client.Create(Job.FromExpression(methodCall), state, context);
        }

        public static string EnqueueWithContext<T>(
            [NotNull] this IBackgroundJobClientWithContext client,
            [NotNull, InstantHandle] Expression<Action<T>> methodCall,
            object context,
            string queue = null)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var state = queue != null ? new EnqueuedState(queue) : new EnqueuedState();

            return client.Create(Job.FromExpression(methodCall), state, context);
        }

        public static string EnqueueWithContext<T>(
            [NotNull] this IBackgroundJobClientWithContext client,
            [NotNull, InstantHandle] Expression<Func<T, Task>> methodCall,
            object context,
            string queue = null)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            var state = queue != null ? new EnqueuedState(queue) : new EnqueuedState();

            return client.Create(Job.FromExpression(methodCall), state, context);
        }
    }
}
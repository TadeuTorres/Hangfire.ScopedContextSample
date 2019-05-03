using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire.Annotations;

namespace WebApplication2.Hangfire
{
    public static class BackgroundJobWithContext
    {
        private static readonly Lazy<IBackgroundJobClientWithContext> CachedClient
            = new Lazy<IBackgroundJobClientWithContext>(() => new BackgroundJobClientWithContext());

        private static readonly Func<IBackgroundJobClientWithContext> DefaultFactory
            = () => CachedClient.Value;

        private static Func<IBackgroundJobClientWithContext> _clientFactory;
        private static readonly object ClientFactoryLock = new object();

        internal static Func<IBackgroundJobClientWithContext> ClientFactory
        {
            get
            {
                lock (ClientFactoryLock)
                {
                    return _clientFactory ?? DefaultFactory;
                }
            }
            set
            {
                lock (ClientFactoryLock)
                {
                    _clientFactory = value;
                }
            }
        }

        public static string EnqueueWithContext([NotNull, InstantHandle] Expression<Action> methodCall, object context, string queue = null)
        {
            var client = ClientFactory();
            return client.EnqueueWithContext(methodCall, context, queue);
        }

        public static string EnqueueWithContext([NotNull, InstantHandle] Expression<Func<Task>> methodCall, object context, string queue = null)
        {
            var client = ClientFactory();
            return client.EnqueueWithContext(methodCall, context, queue);
        }

        public static string EnqueueWithContext<T>([NotNull, InstantHandle] Expression<Action<T>> methodCall, object context, string queue = null)
        {
            var client = ClientFactory();
            return client.EnqueueWithContext(methodCall, context, queue);
        }

        public static string EnqueueWithContext<T>([NotNull, InstantHandle] Expression<Func<T, Task>> methodCall, object context, string queue = null)
        {
            var client = ClientFactory();
            return client.EnqueueWithContext(methodCall, context, queue);
        }
    }
}
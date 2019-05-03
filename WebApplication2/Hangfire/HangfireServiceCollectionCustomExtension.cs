using System;
using Hangfire.Annotations;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Hangfire;

#if NETSTANDARD2_0
using Microsoft.Extensions.Hosting;
#endif

namespace WebApplication2.Hangfire
{
    public static class HangfireServiceCollectionCustomExtension
    {
        public static IServiceCollection AddHangfireWithContext(
            [NotNull] this IServiceCollection services,
            [NotNull] Action<IGlobalConfiguration> configuration)
        {
            return AddHangfireWithContext(services, (provider, config) => configuration(config));
        }

        public static IServiceCollection AddHangfireWithContext(
            [NotNull] this IServiceCollection services,
            [NotNull] Action<IServiceProvider, IGlobalConfiguration> configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            services.TryAddSingletonChecked<IBackgroundJobClient>(x =>
            {
                if (GetInternalServices(x, out var factory, out var stateChanger, out _))
                {
                    return new BackgroundJobClientWithContext(x.GetRequiredService<JobStorage>(), factory, stateChanger);
                }

                return new BackgroundJobClientWithContext(
                    x.GetRequiredService<JobStorage>(),
                    x.GetRequiredService<IJobFilterProvider>());
            });

            services
                .AddHangfire(configuration)
            ;

            return services;
        }

        internal static bool GetInternalServices(
            IServiceProvider provider,
            out IBackgroundJobFactory factory,
            out IBackgroundJobStateChanger stateChanger,
            out IBackgroundJobPerformer performer)
        {
            factory = provider.GetService<IBackgroundJobFactory>();
            performer = provider.GetService<IBackgroundJobPerformer>();
            stateChanger = provider.GetService<IBackgroundJobStateChanger>();

            if (factory != null && performer != null && stateChanger != null)
            {
                return true;
            }

            factory = null;
            performer = null;
            stateChanger = null;

            return false;
        }

        private static void TryAddSingletonChecked<T>(
            [NotNull] this IServiceCollection serviceCollection,
            [NotNull] Func<IServiceProvider, T> implementationFactory)
            where T : class
        {
            serviceCollection.TryAddSingleton<T>(serviceProvider =>
            {
                if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

                // ensure the configuration was performed
                serviceProvider.GetRequiredService<IGlobalConfiguration>();

                return implementationFactory(serviceProvider);
            });
        }
    }
}
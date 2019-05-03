using Hangfire;
using Hangfire.Annotations;
using Hangfire.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Hangfire
{
    public class AspNetCoreJobActivatorWithContext : AspNetCoreJobActivator
    {
        public AspNetCoreJobActivatorWithContext([NotNull] IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public override JobActivatorScope BeginScope(JobActivatorContext context)
        {
            var retorno = base.BeginScope(context);

            var param = context.GetJobParameter<object>("HangfireContext");

            var contextProvider = retorno.Resolve(typeof(IHangfireContextProvider)) as IHangfireContextProvider;
            contextProvider.SetContext(param);

            return retorno;
        }
    }
}
using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Hangfire
{
    public interface IBackgroundJobClientWithContext : IBackgroundJobClient
    {
        string Create(Job job, IState state, object context);
    }
}
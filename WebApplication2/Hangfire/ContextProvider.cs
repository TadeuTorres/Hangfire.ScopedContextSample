using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Hangfire
{
    public interface IHangfireContextProvider
    {
        void SetContext(object context);

        object GetContext();
    }

    public class HangfireContextProvider : IHangfireContextProvider
    {
        private object context = null;

        public object GetContext()
        {
            return context;
        }

        public void SetContext(object context)
        {
            this.context = context;
        }
    }
}
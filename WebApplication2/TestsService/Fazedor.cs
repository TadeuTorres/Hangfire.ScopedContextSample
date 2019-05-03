using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Hangfire;

namespace WebApplication2.TestsService
{
    public interface IFazedor
    {
        void Fazer(string frase, int outroParam);
    }

    public class Fazedor : IFazedor
    {
        private readonly IHangfireContextProvider hangfireContextProvider;

        public Fazedor(IHangfireContextProvider hangfireContextProvider)
        {
            this.hangfireContextProvider = hangfireContextProvider;
        }

        public void Fazer(string frase, int outroParam)
        {
            var context = hangfireContextProvider.GetContext().ToString();
            Debug.WriteLine(context);
            Debug.WriteLine(frase);
            Debug.WriteLine(outroParam);
            Debug.WriteLine(context);
        }
    }
}
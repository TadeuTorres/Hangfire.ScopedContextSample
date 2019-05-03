using Newtonsoft.Json;
using System;
using System.Diagnostics;
using WebApplication2.Hangfire;

namespace WebApplication2.TestsService
{
    public interface IAgendador
    {
        string AgendaComContext();
    }

    public class Agendador : IAgendador
    {
        public string AgendaComContext()
        {
            var context = new ContextQualquer
            {
                Guid = Guid.NewGuid(),
                Inteiro = DateTime.Now.Hour,
                Name = "Pai" + Guid.NewGuid().ToString(),
                Filho = new Filho
                {
                    Id = DateTime.Now.Minute,
                    Name = "Filho" + Guid.NewGuid().ToString()
                }
            };
            BackgroundJobWithContext
                .EnqueueWithContext<IFazedor>(
                x => x.Fazer("Fire and Forget!" + Guid.NewGuid().ToString(), DateTime.Now.Second), context);

            var retorno = JsonConvert.SerializeObject(context);

            Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Debug.WriteLine("Context gerado = " + retorno);
            Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

            return retorno;
        }
    }

    public class ContextQualquer
    {
        public int Inteiro { get; set; }

        public Guid Guid { get; set; }

        public string Name { get; set; }

        public Filho Filho { get; set; }
    }

    public class Filho
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
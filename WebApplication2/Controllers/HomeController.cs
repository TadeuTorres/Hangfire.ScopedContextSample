using Microsoft.AspNetCore.Mvc;
using WebApplication2.TestsService;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAgendador agendador;

        public HomeController(IAgendador agendador)
        {
            this.agendador = agendador;
        }

        public IActionResult Index()
        {
            var context = agendador.AgendaComContext();
            return View(nameof(Index), context);
        }
    }
}
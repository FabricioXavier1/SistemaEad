using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PUC.LDSI.ModuloProfessor.Models;
using System.Diagnostics;

namespace PUC.LDSI.ModuloProfessor.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var errorViewModel = new ErrorViewModel { ErrorMessage = exceptionFeature.Error.Message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };

            return View(errorViewModel);
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace TechSolutions.Controllers
{
    public class ProductosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VeriStore.Models;

namespace VeriStore.Controllers
{
    [RequireLogin]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DxnStoreContext _context;

        public HomeController(ILogger<HomeController> logger, DxnStoreContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Fetch products using the stored procedure via context
            var urunler = _context.GetAllUruns();
            return View(urunler);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

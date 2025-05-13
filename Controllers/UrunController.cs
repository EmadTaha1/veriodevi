using Microsoft.AspNetCore.Mvc;
using VeriStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace VeriStore.Controllers
{
    [RequireLogin]
    public class UrunController : Controller
    {
        private readonly DxnStoreContext _context;
        public UrunController(DxnStoreContext context)
        {
            _context = context;
        }

        // GET: Urun/Details/5
        public IActionResult Details(int id)
        {
            var urun = _context.Uruns
                .FromSqlRaw("CALL sp_GetUrunById({0})", id)
                .AsEnumerable()
                .FirstOrDefault();
            if (urun == null)
            {
                return NotFound();
            }
            return View(urun);
        }

        // GET: Urun/Icecek
        public IActionResult Icecek()
        {
            var urunler = _context.Uruns.Where(u => u.Kategori == "İçecek").ToList();
            return View("List", urunler);
        }

        // GET: Urun/Sampuan
        public IActionResult Sampuan()
        {
            // Include products with Kategori == "Şampuan" or UrunAdi contains "Shampoo"
            var urunler = _context.Uruns
                .Where(u => u.Kategori == "Şampuan" || (u.UrunAdi != null && u.UrunAdi.Contains("Shampoo")))
                .ToList();
            return View("List", urunler);
        }

        // GET: Urun/Sabun
        public IActionResult Sabun()
        {
            var keywords = new[] { "sabun", "soap", "soup", "oil" };
            var urunler = _context.Uruns
                .Where(u => u.Kategori == "Sabun" ||
                    (u.Kategori == "Kişisel Bakım" && u.UrunAdi != null && (u.UrunAdi.ToLower().Contains("sabun") || u.UrunAdi.ToLower().Contains("soap") || u.UrunAdi.ToLower().Contains("soup") || u.UrunAdi.ToLower().Contains("oil"))) ||
                    (u.UrunAdi != null && (u.UrunAdi.ToLower().Contains("sabun") || u.UrunAdi.ToLower().Contains("soap") || u.UrunAdi.ToLower().Contains("soup") || u.UrunAdi.ToLower().Contains("oil"))))
                .ToList();
            return View("List", urunler);
        }

        // Search products by name
        [HttpGet]
        public IActionResult Search(string q)
        {
            var urunler = string.IsNullOrWhiteSpace(q)
                ? _context.Uruns.ToList()
                : _context.Uruns.Where(u => u.UrunAdi != null && u.UrunAdi.Contains(q)).ToList();
            ViewData["SearchQuery"] = q;
            return View("List", urunler);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using VeriStore.Models;
using Microsoft.EntityFrameworkCore;

namespace VeriStore.Controllers
{
    [RequireLogin]
    public class CartController : Controller
    {
        private readonly DxnStoreContext _context;
        public CartController(DxnStoreContext context)
        {
            _context = context;
        }

        // عرض السلة
        public IActionResult Index()
        {
            int? musteriId = HttpContext.Session.GetInt32("UserId");
            if (musteriId == null)
                return RedirectToAction("Login", "Auth");

            // جلب السلة من الإجراء المخزن
            var cart = _context.CartItems
                .FromSqlRaw("CALL sp_GetCartItemsByMusteriId({0})", musteriId)
                .ToList();
            return View(cart);
        }

        // إضافة منتج للسلة
        [HttpPost]
        public IActionResult AddToCart(int urunId)
        {
            int? musteriId = HttpContext.Session.GetInt32("UserId");
            if (musteriId == null)
                return RedirectToAction("Login", "Auth");

            // إضافة أو تحديث السلة عبر الإجراء المخزن
            _context.Database.ExecuteSqlRaw(
                "CALL sp_AddToCart({0}, {1}, {2})",
                musteriId, urunId, 1);
            return RedirectToAction("Index");
        }

        // حذف منتج من السلة
        [HttpPost]
        public IActionResult Remove(int urunId)
        {
            int? musteriId = HttpContext.Session.GetInt32("UserId");
            if (musteriId == null)
                return RedirectToAction("Login", "Auth");

            // حذف المنتج من السلة عبر تحديث الكمية إلى 0 أو إجراء خاص
            _context.Database.ExecuteSqlRaw(
                "CALL sp_AddToCart({0}, {1}, {2})",
                musteriId, urunId, 0);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Clear()
        {
            int? musteriId = HttpContext.Session.GetInt32("UserId");
            if (musteriId == null)
                return RedirectToAction("Login", "Auth");
            _context.Database.ExecuteSqlRaw("CALL sp_ClearCartByMusteriId({0})", musteriId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            int? musteriId = HttpContext.Session.GetInt32("UserId");
            if (musteriId == null)
                return RedirectToAction("Login", "Auth");
            _context.Database.ExecuteSqlRaw("CALL sp_RemoveCartItem({0})", cartItemId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult BuyNow()
        {
            int? musteriId = HttpContext.Session.GetInt32("UserId");
            if (musteriId == null)
                return RedirectToAction("Login", "Auth");
            // تنفيذ الشراء: تفريغ السلة وتفعيل التريجر
            _context.Database.ExecuteSqlRaw("CALL sp_ClearCartByMusteriId({0})", musteriId);
            // التريجر trg_StokAzalt سيعمل تلقائياً عند حذف أو تحديث السلة في قاعدة البيانات
            TempData["BuyNowMessage"] = "Siparişiniz başarıyla alınmıştır. Ürünleriniz en kısa sürede kargoya verilecektir.";
            return RedirectToAction("BuyNowSuccess");
        }

        public IActionResult BuyNowSuccess()
        {
            ViewBag.Message = TempData["BuyNowMessage"];
            return View();
        }
    }
}

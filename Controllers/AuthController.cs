using Microsoft.AspNetCore.Mvc;
using VeriStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VeriStore.Controllers
{
    public class AuthController : Controller
    {
        private readonly DxnStoreContext _context;
        public AuthController(DxnStoreContext context)
        {
            _context = context;
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        public IActionResult Login(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // تحقق من المستخدم باستخدام الإجراء المخزن
            var user = _context.Musteris
                .FromSqlRaw("CALL sp_GetMusteriByEmailAndPassword({0}, {1})", model.Email, HashPassword(model.Password))
                .AsEnumerable()
                .FirstOrDefault();
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.MusteriId);
                HttpContext.Session.SetString("UserEmail", user.Email ?? "");
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "البريد الإلكتروني أو كلمة المرور غير صحيحة";
            return View(model);
        }

        // GET: /Auth/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Auth/Register
        [HttpPost]
        public IActionResult Register(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // إضافة مستخدم جديد باستخدام الإجراء المخزن
            _context.Database.ExecuteSqlRaw(
                "CALL sp_CreateMusteri({0}, {1}, {2})",
                model.Name ?? "", model.Email, HashPassword(model.Password));
            return RedirectToAction("Login");
        }

        // تسجيل الخروج
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // دالة لتشفير كلمة المرور (SHA256)
        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }

    // فلتر تحقق الجلسة
    public class RequireLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            if (httpContext.Session.GetInt32("UserId") == null)
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
            base.OnActionExecuting(context);
        }
    }
}

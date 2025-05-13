namespace VeriStore.Models
{
    public class UserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Name { get; set; } // للاستخدام في التسجيل فقط
        public string? Role { get; set; } // Customer, Admin, etc.
    }
}

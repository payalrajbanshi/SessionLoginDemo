using Microsoft.AspNetCore.Mvc;
using SessionLoginDemo.Models;

namespace SessionLoginDemo.Controllers
{
    public class AccountController : Controller
    {
        private readonly string validUsername = "admin";
        private readonly string validPassword = "123";
        private static List<User> registeredUsers = new List<User>();
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            var matchedUser = registeredUsers.FirstOrDefault(u => 
                u.Username == user.Username && u.Password == user.Password);
            if (matchedUser != null )
            {
                HttpContext.Session.SetString("Username", matchedUser.Username);
                HttpContext.Session.SetString("Role", matchedUser.Role);
                return RedirectToAction("Dashboard");
            }
    if (user.Username == validUsername && user.Password == validPassword)
    {
        HttpContext.Session.SetString("Username", validUsername);
        HttpContext.Session.SetString("Role", "Admin"); 
        return RedirectToAction("Dashboard");
    }

            ViewBag.Error = "Invalid credentials!";
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (registeredUsers.Any(u => u.Username == user.Username))
            {
                ViewBag.Error = "Username already exists!";
                return View();
            }
            user.Role = "User"; 
            registeredUsers.Add(user);
            return RedirectToAction("Login");
        }
        public IActionResult Dashboard()
        {
            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");
            if (username == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.User = username;
            ViewBag.Role = role;
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

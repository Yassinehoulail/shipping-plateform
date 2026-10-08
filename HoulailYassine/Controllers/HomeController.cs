using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HoulailYassine.Models;
using HoulailYassine.Repos;
using HoulailYassine.Entities;
using System.Security.Cryptography;
using System.Text;

namespace HoulailYassine.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ClientRepos _clientRepos;

        public HomeController(ILogger<HomeController> logger, 
            ClientRepos clientRepos)
        {
            _logger = logger;
            _clientRepos = clientRepos;
        }

        [HttpGet]
        public IActionResult Index()
        {
        
            if (HttpContext.Session.GetString("UserId") != null)
            {
                return RedirectToAction("Index", "Produit");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var client = _clientRepos.GetByEmail(model.Email);

                if (client != null && VerifyPassword(model.Password, client.PasswordHash))
                {
                    // Set session
                    HttpContext.Session.SetString("UserId", client.Id.ToString());
                    HttpContext.Session.SetString("UserEmail", client.Email);
                    HttpContext.Session.SetString("UserName", $"{client.Nom} {client.Prenom}");
                    HttpContext.Session.SetString("IsAdmin", client.IsAdmin.ToString());

                    if (client.IsAdmin)
                    {
                        return RedirectToAction("Index", "Admin");
                    }

                    return RedirectToAction("Index", "Produit");
                }

                TempData["LoginError"] = "Invalid email or password.";
            }
            else
            {
                TempData["LoginError"] = "Please correct the errors and try again.";
            }
            // Return to home page with login tab active
            TempData["ShowLogin"] = true;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(CreationClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                var existingClient = _clientRepos.GetByEmail(model.EMail);
                if (existingClient != null)
                {
                    TempData["SignUpError"] = "This email is already registered.";
                    TempData["ShowSignUp"] = true;
                    return RedirectToAction("Index");
                }

                // Create new client
                var client = new Client
                {
                    Nom = model.Nom,
                    Prenom = model.Prenom,
                    Telephone = model.NumdeTelephone,
                    Email = model.EMail,
                    PasswordHash = HashPassword(model.MotPasse),
                    CreateAt = DateTime.Now,
                    IsDelete = false
                };

                _clientRepos.Add(client);

                // Set session
                HttpContext.Session.SetString("UserId", client.Id.ToString());
                HttpContext.Session.SetString("UserEmail", client.Email);
                HttpContext.Session.SetString("UserName", $"{client.Nom} {client.Prenom}");
                HttpContext.Session.SetString("IsAdmin", client.IsAdmin.ToString());

                if (client.IsAdmin)
                {
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("Index", "Produit");
            }
            TempData["SignUpError"] = "Please correct the errors and try again.";
            TempData["ShowSignUp"] = true;
            return RedirectToAction("Index");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string? passwordHash)
        {
            if (string.IsNullOrEmpty(passwordHash))
                return false;

            var hashOfInput = HashPassword(password);
            return hashOfInput == passwordHash;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

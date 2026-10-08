using Microsoft.AspNetCore.Mvc;
using HoulailYassine.Models;
using HoulailYassine.Repos;
using HoulailYassine.Entities;
using System.Security.Cryptography;
using System.Text;

namespace HoulailYassine.Controllers
{
    public class ClientController : Controller
    {
        private readonly ClientRepos _clientRepos;

        public ClientController(ClientRepos clientRepos)
        {
            _clientRepos = clientRepos;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
           
            if (HttpContext.Session.GetString("UserId") != null)
            {
                return RedirectToAction("Index", "Produit");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(CreationClientViewModel model)
        {
            if (ModelState.IsValid)
            {
         
                var existingClient = _clientRepos.GetByEmail(model.EMail);
                if (existingClient != null)
                {
                    ModelState.AddModelError("EMail", "This email is already registered.");
                    TempData["ShowSignUp"] = true;
                    return RedirectToAction("Index", "Home");
                }

            
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

         
                HttpContext.Session.SetString("UserId", client.Id.ToString());
                HttpContext.Session.SetString("UserEmail", client.Email);
                HttpContext.Session.SetString("UserName", $"{client.Nom} {client.Prenom}");

                return RedirectToAction("Index", "Produit");
            }
            TempData["ShowSignUp"] = true;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
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

                ModelState.AddModelError("", "Invalid email or password.");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
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
    }
}

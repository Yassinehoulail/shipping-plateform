using Microsoft.AspNetCore.Mvc;
using HoulailYassine.Repos;
using HoulailYassine.Entities;
using HoulailYassine.Models;

using HoulailYassine.Shared;
using System.Security.Cryptography;
using System.Text;

namespace HoulailYassine.Controllers
{
    public class AdminController : Controller
    {
        private readonly ClientRepos _clientRepos;
        private readonly ProduitRepos _produitRepos;
        private readonly CommandeRepos _commandeRepos;

        public AdminController(
            ClientRepos clientRepos,
            ProduitRepos produitRepos,
            CommandeRepos commandeRepos)
        {
            _clientRepos = clientRepos;
            _produitRepos = produitRepos;
            _commandeRepos = commandeRepos;
        }

        private bool IsAdmin()
        {
            var isAdminStr = HttpContext.Session.GetString("IsAdmin");
            if (string.IsNullOrEmpty(isAdminStr))
                return false;

            return bool.TryParse(isAdminStr, out bool isAdmin) && isAdmin;
        }

        public IActionResult Index()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var stats = new AdminDashboardViewModel
            {
                TotalClients = _clientRepos.Gets().Count,
                TotalProducts = _produitRepos.GetProduits().Count,
                TotalOrders = _commandeRepos.Gets().Count,
                ActiveProducts = _produitRepos.GetProduits().Count(p => p.Stock && !p.IsDelete)
            };

            return View(stats);
        }

        public IActionResult Products()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var products = _produitRepos.GetProduits().Where(p => !p.IsDelete).ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProduct(Produit product)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                product.CreateAt = DateTime.Now;
                product.IsDelete = false;
                _produitRepos.Add(product);
                return RedirectToAction("Products");
            }

            return View(product);
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var product = _produitRepos.GetProduitById(id);
            if (product == null || product.IsDelete)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(Produit product)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                _produitRepos.Update(product);
                return RedirectToAction("Products");
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProduct(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var product = _produitRepos.GetProduitById(id);
            if (product != null)
            {
                _produitRepos.Delete(product);
            }

            return RedirectToAction("Products");
        }

       
        public IActionResult Clients()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var clients = _clientRepos.Gets();
            return View(clients);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteClient(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var client = _clientRepos.Get(id);
            if (client != null)
            {
                _clientRepos.Delete(client);
            }

            return RedirectToAction("Clients");
        }

        public IActionResult Orders()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var orders = _commandeRepos.Gets();
            return View(orders);
        }

        [HttpGet]
        public IActionResult OrderDetail(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var order = _commandeRepos.Get(id);
            if (order == null || order.IsDelete)
            {
                return NotFound();
            }

       
            var client = _clientRepos.Get(order.ClientId);
            ViewBag.ClientName = client != null ? $"{client.Nom} {client.Prenom}" : "Unknown";
            ViewBag.ClientEmail = client?.Email ?? "N/A";

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderStatus(int id, StatutCommande status)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var order = _commandeRepos.Get(id);
            if (order != null && !order.IsDelete)
            {
                order.Statut = status;
                _commandeRepos.Update(order);
            }

            return RedirectToAction("OrderDetail", new { id });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using HoulailYassine.Repos;

namespace HoulailYassine.Controllers
{
    public class CommandeController : Controller
    {
        private readonly CommandeRepos _commandeRepos;
        private readonly ClientRepos _clientRepos;

        public CommandeController(CommandeRepos commandeRepos, ClientRepos clientRepos)
        {
            _commandeRepos = commandeRepos;
            _clientRepos = clientRepos;
        }

        public IActionResult Index()
        {
     
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Home");
            }

            var clientId = int.Parse(userId);
            var orders = _commandeRepos.GetByClientId(clientId);
            return View(orders);
        }

        public IActionResult Detail(int id)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Home");
            }

            var order = _commandeRepos.Get(id);
            if (order == null || order.IsDelete || order.ClientId != int.Parse(userId))
            {
                return NotFound();
            }

            var client = _clientRepos.Get(order.ClientId);
            ViewBag.ClientName = client != null ? $"{client.Nom} {client.Prenom}" : "Unknown";
            
            return View(order);
        }
    }
}

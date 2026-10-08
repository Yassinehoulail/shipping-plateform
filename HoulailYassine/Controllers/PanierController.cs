using Microsoft.AspNetCore.Mvc;
using HoulailYassine.Data;
using HoulailYassine.Entities;
using HoulailYassine.Extensions;
using HoulailYassine.Models;
using HoulailYassine.Repos;
using HoulailYassine.Shared;

namespace HoulailYassine.Controllers
{
    public class PanierController : Controller
    {
        private readonly ProduitRepos _produitRepos;
        private readonly ClientRepos _clientRepos;
        private readonly PanierRepos _panierRepos;

        public PanierController(ProduitRepos produitRepos, ClientRepos clientRepos, PanierRepos panierRepos)
        {
            _produitRepos = produitRepos;
            _clientRepos = clientRepos;
            _panierRepos = panierRepos;
        }

        public IActionResult Index()
        {
            var panier = HttpContext.Session.GetObjectFromJson<List<ItemPanier>>("Panier") ?? new List<ItemPanier>();
            return View(panier);
        }


        public IActionResult Ajouter(int id)
        {
            var produit = _produitRepos.GetProduitById(id);
            if (produit == null) return NotFound();

            var panier = HttpContext.Session.GetObjectFromJson<List<ItemPanier>>("Panier") ?? new List<ItemPanier>();

            var itemExistant = panier.FirstOrDefault(x => x.ProduitId == id);
            if (itemExistant != null)
            {
                itemExistant.Quantite++;
            }
            else
            {
                panier.Add(new ItemPanier { ProduitId = produit.Id, Titre = produit.Titre, Prix = produit.Prix, Quantite = 1 });
            }

            HttpContext.Session.SetObjectAsJson("Panier", panier);
            return RedirectToAction("Index", "Produit");
        }

        [HttpPost]
        public IActionResult ValiderCommande()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login", "Client");

            var panier = HttpContext.Session.GetObjectFromJson<List<ItemPanier>>("Panier");
            if (panier == null || !panier.Any()) return RedirectToAction("Index");

            int clientId = int.Parse(userIdStr);
            _panierRepos.Valider(clientId, panier);

            HttpContext.Session.Remove("Panier");

            return RedirectToAction("Index", "Commande"); 
        }

        public IActionResult Vider()
        {
            HttpContext.Session.Remove("Panier");
            return RedirectToAction("Index");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using HoulailYassine.Repos;
using HoulailYassine.Models;

namespace HoulailYassine.Controllers
{
    public class ProduitController : Controller
    {
        private readonly ProduitRepos _produitRepos;

        public ProduitController(ProduitRepos produitRepos)
        {
            _produitRepos = produitRepos;
        }

        public IActionResult Index()
        {
            
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var produits = _produitRepos.GetProduits()
                .Where(a => a.Stock == true)
                .Select(p => new ProduitListViewModel
                {
                    Id = p.Id,
                    Titre = p.Titre,
                    Description = p.Description,
                    Prix = p.Prix
                }).ToList();

            return View(produits);
        }
    }
}

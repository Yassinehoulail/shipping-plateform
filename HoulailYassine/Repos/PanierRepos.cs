using HoulailYassine.Data;
using HoulailYassine.Entities;
using HoulailYassine.Models;
using HoulailYassine.Shared;

namespace HoulailYassine.Repos
{
    public class PanierRepos
    {
        private readonly MyDbContextGlovo _context;
        private readonly ClientRepos _clientRepos;

        public PanierRepos(MyDbContextGlovo context, ClientRepos clientRepos)
        {
            _context = context;
            _clientRepos = clientRepos;
        }

        public void Valider(int clientId, List<ItemPanier> panier)
        {
            var client = _clientRepos.Get(clientId);

            var commande = new Commande
            {
                ClientId = clientId,
                Numero = new Random().Next(100000, 999999),
                Adresse = client?.Adresse ?? "Adresse inconnue",
                Statut = StatutCommande.EnPreparation,
                CreateAt = DateTime.Now,
                IsDelete = false,
                LivreurId = null,
                Montant = panier.Sum(x => x.Prix * x.Quantite)
            };

            _context.Commandes.Add(commande);
            _context.SaveChanges();

            foreach (var item in panier)
            {
                var ligne = new LigneCommande
                {
                    CommandeId = commande.Id,
                    ProduitId = item.ProduitId,
                    Quantite = item.Quantite,
                    PrixUnitaire = item.Prix,
                    CreateAt = DateTime.Now
                };
                _context.LigneCommandes.Add(ligne);
            }
            _context.SaveChanges();
        }
    }
}

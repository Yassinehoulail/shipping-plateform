using HoulailYassine.Data;
using HoulailYassine.Entities;
using HoulailYassine.Shared;

namespace HoulailYassine.Repos
{
    public class CommandeRepos
    {
        private readonly MyDbContextGlovo _context;

        public CommandeRepos(MyDbContextGlovo context)
        {
            _context = context;
        }

        public Commande? Get(int id)
        {
            return _context.Commandes.Find(id);
        }

        public List<Commande> Gets()
        {
            return _context.Commandes.Where(c => !c.IsDelete).OrderByDescending(c => c.CreateAt).ToList();
        }

        public void Add(Commande commande)
        {
            _context.Commandes.Add(commande);
            _context.SaveChanges();
        }

        public void Update(Commande commande)
        {
            _context.Commandes.Update(commande);
            _context.SaveChanges();
        }

        public void Delete(Commande commande)
        {
            commande.IsDelete = true;
            commande.DeleteAt = DateTime.Now;
            _context.Commandes.Update(commande);
            _context.SaveChanges();
        }

        public List<Commande> GetByClientId(int clientId)
        {
            return _context.Commandes.Where(c => c.ClientId == clientId && !c.IsDelete).OrderByDescending(c => c.CreateAt).ToList();
        }
    }
}

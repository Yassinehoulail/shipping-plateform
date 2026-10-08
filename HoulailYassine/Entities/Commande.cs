using System.ComponentModel.DataAnnotations.Schema;
using HoulailYassine.Shared;

namespace HoulailYassine.Entities
{
    [Table("Commande")]
    public class Commande : Base
    {
        public int Numero { get; set; }
        public int ClientId { get; set; }
        public int? LivreurId { get; set; }
        public string Adresse { get; set; } = string.Empty;
        public decimal Montant { get; set; }
        public StatutCommande Statut { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using HoulailYassine.Entities;

namespace HoulailYassine.Entities
{
    [Table("LigneCommande")]
    public class LigneCommande : Base
    {
        public int CommandeId { get; set; }
        public int ProduitId { get; set; }
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }

        [ForeignKey("ProduitId")]
        public virtual Produit? Produit { get; set; }
    }
}
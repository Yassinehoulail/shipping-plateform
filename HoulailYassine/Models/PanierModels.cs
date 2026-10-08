namespace HoulailYassine.Models
{
    public class ItemPanier
    {
        public int ProduitId { get; set; }
        public string Titre { get; set; } = string.Empty;
        public decimal Prix { get; set; }
        public int Quantite { get; set; }
    }
}
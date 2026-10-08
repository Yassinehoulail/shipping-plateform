using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoulailYassine.Entities
{
    public class Personne : Base
    {
        [MaxLength(300)]
        public string Nom { get; set; } = string.Empty;

        private string _Prenom = string.Empty;
        [MaxLength(300)]
        public string Prenom
        {
            get { return _Prenom; }
            set { _Prenom = value; }
        }

        [MaxLength(300)]
        public string? Adresse { get; set; }
        [MaxLength(12)]
        public string Telephone { get; set; } = string.Empty;
        [MaxLength(300)]
        public string Email { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? PasswordHash { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}

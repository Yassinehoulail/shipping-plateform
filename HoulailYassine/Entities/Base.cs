using System.ComponentModel.DataAnnotations;

namespace HoulailYassine.Entities
{
    public class Base
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? DeleteAt { get; set; }

    }
}

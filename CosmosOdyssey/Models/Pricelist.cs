using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmosOdyssey.Models
{
    public class Pricelist
    {
        [Column(TypeName = "VARCHAR(40)")]
        [Key]
        public string Id { get; set; }
        public DateTime ValidUntil { get; set; }
    }

    public class Leg
    {
        [Column(TypeName = "VARCHAR(40)")]
        [Key]
        public string Id { get; set; }

        [Column(TypeName = "VARCHAR(40)")]
        public Pricelist PricelistData { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmosOdyssey.Models
{
    public class Provider
    {
        [Column(TypeName = "VARCHAR(40)")]
        [Key]
        public string Id { get; set; }

        [Column(TypeName = "VARCHAR(40)")]
        //public string LegId { get; set; }
        public Leg Leg { get; set; }

        [Column(TypeName = "VARCHAR(40)")]
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }

        [Column(TypeName = "Decimal(65,2)")]
        public decimal Price { get; set; }
        public DateTime FlightStart { get; set; }
        public DateTime FlightEnd { get; set; }
    }
}
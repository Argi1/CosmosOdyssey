using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmosOdyssey.Models
{
    public class Reservation
    {
        [Column(TypeName = "VARCHAR(40)")]
        [Key]
        public Guid ReservationId { get; set; }

        [Column(TypeName = "VARCHAR(40)")]
        public Guid OrderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Column(TypeName = "DECIMAL(65,2)")]
        public decimal TotalPrice { get; set; }

        [Column(TypeName = "VARCHAR(20)")]
        public string TotalTravelTime { get; set; }

        [Column(TypeName = "VARCHAR(40)")]
        public string ProviderId { get; set; }
        public Provider Provider { get; set; }
        public string CompanyName { get; set; }
        public string StartPlanetName { get; set; }
        public string ToPlanetName { get; set; }

        [Column(TypeName = "VARCHAR(40)")]
        public string RouteId { get; set; }
        public Route Route { get; set; }
    }
}

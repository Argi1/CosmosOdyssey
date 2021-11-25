using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmosOdyssey.Models
{
    public class Route
    {
        [Column(TypeName = "VARCHAR(40)")]
        [Key]
        public string Id { get; set; }

        [Column(TypeName = "VARCHAR(40)")]
        //public string LegId { get; set; }
        public Leg Leg { get; set; }

        [Column(TypeName = "VARCHAR(40)")]
        public string StartPlanetId { get; set; }
        public string StartPlanetName { get; set; }

        [Column(TypeName = "VARCHAR(40)")]
        public string ToPlanetId { get; set; }
        public string ToPlanetName { get; set; }
        public long Distance { get; set; }
    }
}

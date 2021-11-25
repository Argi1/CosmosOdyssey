using System;
using System.Collections.Generic;

namespace CosmosOdyssey.Models
{
    public class JsonPricelistData
    {
        public string Id { get; set; }
        public DateTime ValidUntil { get; set; }
        public List<JsonLeg> Legs { get; set; }
    }

    public class JsonLeg
    {
        public string Id { get; set; }
        public JsonRoute RouteInfo { get; set; }
        public List<JsonProvider> Providers { get; set; }
    }

    public class JsonRoute
    {
        public string Id { get; set; }
        public JsonPlanet From { get; set; }
        public JsonPlanet To { get; set; }
        public long Distance { get; set; }
    }

    public class JsonProvider
    {
        public string Id { get; set; }
        public JsonCompany Company { get; set; }
        public decimal Price { get; set; }
        public DateTime FlightStart { get; set; }
        public DateTime FlightEnd { get; set; }
    }

    public class JsonPlanet
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class JsonCompany
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}

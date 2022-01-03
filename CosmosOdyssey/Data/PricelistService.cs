using CosmosOdyssey.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CosmosOdyssey.Data
{
    public class PricelistService : BackgroundService
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContext;

        private readonly HttpClient _client = new();

        public PricelistService(IDbContextFactory<DatabaseContext> dbContext)
        {
            _dbContext = dbContext;

            //Because PricelistService is the first service to run, i check if the database exists.
            var dbCreationContext = _dbContext.CreateDbContext();
            dbCreationContext.Database.EnsureCreated();
        }

        // Method that is constantly running in the background.
        // Checks the expiration time of the latest Pricelist, waits until it passes, then gets new info from /
        //  ... the api and stores it in the database. If there is more than 15 Pricelists, removes the oldest Pricelist and any data that
        //  ... is connected to it.
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) 
        {
            await Task.Delay(5000);
            while (!stoppingToken.IsCancellationRequested)
            {
                var nextRunTime = GetValidUntilDate().Subtract(DateTime.UtcNow);

                if (nextRunTime.TotalSeconds < 0)
                {
                    nextRunTime = TimeSpan.Zero;
                }

                await Task.Delay(nextRunTime, stoppingToken);

                var data = await GetDeserializedDataFromApi();

                AddPricelistToDb(data);

                var priceListCount = GetPricelistCountFromDb();

                if (priceListCount >= 16)
                {
                    for (int x = 0; x < priceListCount - 15; x++)
                    {
                        RemoveOldestPricelistAndReservations();
                    }
                }
            }
        }


        private void AddPricelistToDb(JsonPricelistData jsonData)
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                var pricelistData = RemapPricelistJsonData(jsonData);

                dbContext.Pricelists.Add(pricelistData);

                foreach (var jsonLeg in jsonData.Legs)
                {
                    var leg = RemapLegJsonData(jsonLeg, pricelistData);

                    var route = RemapRouteJsonData(jsonLeg.RouteInfo, leg);

                    dbContext.Legs.Add(leg);
                    dbContext.Routes.Add(route);

                    foreach (var jsonProvider in jsonLeg.Providers)
                    {
                        var provider = RemapProviderJsonData(jsonProvider, leg);
                        dbContext.Providers.Add(provider);
                    }
                }
                dbContext.SaveChanges();
            }
        }

        private Pricelist RemapPricelistJsonData(JsonPricelistData jsonData)
        {
            return new Pricelist { Id = jsonData.Id, ValidUntil = jsonData.ValidUntil };
        }

        private Leg RemapLegJsonData(JsonLeg jsonLeg, Pricelist pricelistData)
        {
            return new Leg
            {
                Id = jsonLeg.Id,
                PricelistData = pricelistData
            };
        }

        private Route RemapRouteJsonData(JsonRoute jsonRoute, Leg leg)
        {
            return new Route
            {
                Id = jsonRoute.Id,
                Leg = leg,
                StartPlanetId = jsonRoute.From.Id,
                StartPlanetName = jsonRoute.From.Name,
                ToPlanetId = jsonRoute.To.Id,
                ToPlanetName = jsonRoute.To.Name,
                Distance = jsonRoute.Distance
            };
        }

        private Provider RemapProviderJsonData(JsonProvider jsonProvider, Leg leg)
        {
            return new Provider
            {
                Id = jsonProvider.Id,
                Leg = leg,
                CompanyId = jsonProvider.Company.Id,
                CompanyName = jsonProvider.Company.Name,
                Price = jsonProvider.Price,
                FlightStart = jsonProvider.FlightStart,
                FlightEnd = jsonProvider.FlightEnd
            };
        }



        // Get the data from the API and deserialize it from JSON into Pricelist Model.
        private async Task<JsonPricelistData> GetDeserializedDataFromApi()
        {
            var url = "https://cosmos-odyssey.azurewebsites.net/api/v1.0/TravelPrices";
            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            JsonSerializerOptions jsonOptions = new()
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<JsonPricelistData>(content, jsonOptions);
        }

        // Get the latest "validuntil" from the database and add 5 seconds to it for buffer.
        // Returns current time + 5s if there are no Pricelists in the database.
        private DateTime GetValidUntilDate()
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                var validUntil = dbContext.Pricelists.OrderByDescending(x => x.ValidUntil).FirstOrDefault();

                if (validUntil is null)
                {
                    return DateTime.MinValue;
                }

                return validUntil.ValidUntil.AddSeconds(5);
            }
        }

        private int GetPricelistCountFromDb()
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                return dbContext.Pricelists.Count();
            }
        }

        private void RemoveOldestPricelistAndReservations()
        {
            using (var dbContext = _dbContext.CreateDbContext())
            {
                var oldestId = dbContext
                    .Pricelists
                    .Where(x =>
                        x.ValidUntil == dbContext.Pricelists.Min(y => y.ValidUntil))
                    .FirstOrDefault()
                    .Id;

                dbContext.RemoveRange(dbContext.Pricelists.Where(x => x.Id == oldestId));

                dbContext.SaveChanges();
            }
        }
    }
}

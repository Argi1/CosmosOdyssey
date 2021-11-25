using CosmosOdyssey.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosOdyssey.Data
{
    public class FlightReservationService
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContext;

        private string LatestPricelistId;
        public DateTime LatestPricelistValidUntil;

        public List<Provider> ChosenProviders { get; set; }

        public FlightReservationService(IDbContextFactory<DatabaseContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SetLatestPricelist()
        {
            using (var context = _dbContext.CreateDbContext())
            {
                var latestPricelist = await context.Pricelists.OrderByDescending(x => x.ValidUntil).FirstOrDefaultAsync();
                LatestPricelistId = latestPricelist.Id;
                LatestPricelistValidUntil = latestPricelist.ValidUntil;
            }
        }

        public async Task<List<Route>> GetAllRoutesAsync()
        {
            using (var context = _dbContext.CreateDbContext())
            {
                return await context.Routes.Where(x => x.Leg.PricelistData.Id == LatestPricelistId).Include(x => x.Leg).ToListAsync();
            }
        }

        public async Task<Leg> GetLegAsync(Provider provider)
        {
            using (var context = _dbContext.CreateDbContext())
            {
                return await context.Legs.Where(x => x.Id == provider.Leg.Id).FirstOrDefaultAsync();
            }
        }

        public async Task<Leg> GetLegAsync(Route route)
        {
            using (var context = _dbContext.CreateDbContext())
            {
                return await context.Legs.Where(x => x.Id == route.Leg.Id).FirstOrDefaultAsync();
            }
        }
        public async Task<List<Provider>> GetProvidersAsync(Leg leg)
        {
            using (var context = _dbContext.CreateDbContext())
            {
                return await context.Providers.Where(x => x.Leg.Id == leg.Id).Include(x => x.Leg).ToListAsync();
            }
        }
        public async Task<Route> GetRouteAsync(Leg leg)
        {
            using (var context = _dbContext.CreateDbContext())
            {
                return await context.Routes.Where(x => x.Leg.Id == leg.Id).Include(x => x.Leg).FirstOrDefaultAsync();
            }
        }

        public async Task<bool> InsertReservation(List<Reservation> reservation)
        {
            using (var context = _dbContext.CreateDbContext())
            {
                await context.Reservations.AddRangeAsync(reservation);
                await context.SaveChangesAsync();
                return true;
            }
        }
    }
}

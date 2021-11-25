using CosmosOdyssey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosOdyssey.Pages
{
    public partial class FlightPlan
    {
        public List<Route> Routes { get; set; }

        List<string> FromPlanets;
        List<string> ToPlanets;

        string SelectedFromPlanet;
        string SelectedToPlanet;

        bool ShowTables = true;

        bool IsErrorActive;
        string Title;
        string Message;

        List<List<Route>> AllPossibleRoutes;
        List<List<(Route, List<Provider>)>> AllRoutesProviders;

        string SortCompanyNameText;
        string SortCompanyName;

        private async Task OnSearch()
        {
            if (FlightService.LatestPricelistValidUntil.CompareTo(DateTime.UtcNow) > 0)
            {
                if (SelectedToPlanet != "" && SelectedToPlanet is not null && SelectedFromPlanet != "" && SelectedFromPlanet is not null)
                {
                    AllPossibleRoutes = new List<List<Route>>();
                    AllRoutesProviders = new List<List<(Route, List<Provider>)>>();

                    LookForRoutesToSelectedPlanet(SelectedFromPlanet, new List<Route>());

                    AllPossibleRoutes = AllPossibleRoutes.OrderBy(l => l.Count).ToList();

                    await PopulateAllRoutesProviders();

                    ShowTables = true;
                }
                else
                {
                    ShowTables = false;
                }
            }
            else
            {
                ShowError("The flight plan has expired!", "Please refresh your page to see the new flight plan.");
            }
        }

        private async Task PopulateAllRoutesProviders()
        {
            foreach (var possibleRoute in AllPossibleRoutes)
            {
                if (possibleRoute.Count <= 4)
                {
                    AllRoutesProviders.Add(new List<(Route, List<Provider>)> { });

                    DateTime previousEarliestLanding = DateTime.MinValue;

                    foreach (var routePiece in possibleRoute)
                    {
                        Leg leg = await FlightService.GetLegAsync(routePiece);

                        List<Provider> providers = await FlightService.GetProvidersAsync(leg);

                        providers = providers.Where(x => DateTime.Compare(previousEarliestLanding, x.FlightStart) <= 0).ToList();

                        if (providers.Count == 0)
                        {
                            AllRoutesProviders.Remove(AllRoutesProviders.ElementAt(AllRoutesProviders.Count - 1));
                            break;
                        }
                        else
                        {
                            AllRoutesProviders.ElementAt(AllRoutesProviders.Count - 1).Add((routePiece, providers));
                            previousEarliestLanding = providers.Min(x => x.FlightEnd);
                        }
                    }
                }
            }
        }

        private void LookForRoutesToSelectedPlanet(string startPlanet, List<Route> currentRoutes)
        {
            var availableRoutes = Routes.Where(route => route.StartPlanetName == startPlanet).ToList();

            foreach (var availableRoute in availableRoutes)
            {
                // Check to make sure that it dosent loop back around.
                if (currentRoutes.Any(x => x.ToPlanetName == availableRoute.ToPlanetName || x.StartPlanetName == availableRoute.ToPlanetName))
                {
                    continue;
                }

                currentRoutes.Add(availableRoute);

                if (availableRoute.ToPlanetName == SelectedToPlanet)
                {
                    //Create a copy of the current route to add a new list of routes to the list instead of adding a reference.
                    AllPossibleRoutes.Add(new List<Route> { });

                    foreach (var routeCopy in currentRoutes)
                    {
                        AllPossibleRoutes.ElementAt(AllPossibleRoutes.Count - 1)
                            .Add(new Route
                            {
                                Id = routeCopy.Id,
                                Leg = routeCopy.Leg,
                                StartPlanetId = routeCopy.StartPlanetId,
                                StartPlanetName = routeCopy.StartPlanetName,
                                ToPlanetId = routeCopy.ToPlanetId,
                                ToPlanetName = routeCopy.ToPlanetName,
                                Distance = routeCopy.Distance
                            });
                    }

                    // Remove the finishing route so that the first if check dosent catch a valid route.
                    currentRoutes.RemoveAt(currentRoutes.Count - 1);
                    continue;
                }

                LookForRoutesToSelectedPlanet(availableRoute.ToPlanetName, currentRoutes);

                // Remove the route from the current route list when all of its possible destination routes have been looked trough.
                currentRoutes.RemoveAt(currentRoutes.Count - 1);
            }
        }

        private void SetCompanyNameFilter()
        {
            SortCompanyName = SortCompanyNameText;
            SortCompanyNameText = "";
        }

        private void ResetCompanyNameFilter()
        {
            SortCompanyName = null;
        }
    }
}

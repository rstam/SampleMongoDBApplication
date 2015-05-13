using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private const string __connectionString = "mongodb://localhost";
        private static Lazy<MongoClient> __client = new Lazy<MongoClient>(() => new MongoClient(__connectionString));

        public async Task<ActionResult> Index()
        {
            var viewModel = await CreateIndexViewModelAsync();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Search(SearchCriteriaModel criteriaModel)
        {
            var viewModel = await CreateSearchResultViewModelAsync(criteriaModel);
            return View("SearchResultView", viewModel);
        }

        private async Task<IndexViewModel> CreateIndexViewModelAsync()
        {
            var findAirlinesTask = FindAirlinesAsync();
            var findAirportsTask = FindAirportsAsync();

            await Task.WhenAll(findAirlinesTask, findAirportsTask);

            return new IndexViewModel
            {
                Airlines = findAirlinesTask.Result.OrderBy(a => a.Description),
                Airports = findAirportsTask.Result.OrderBy(a => a.Description)
            };
        }

        private async Task<IEnumerable<Airline>> FindAirlinesAsync()
        {
            var client = __client.Value;
            var database = client.GetDatabase("flights");
            var collection = database.GetCollection<Airline>("airlines");

            return await collection.Find(new BsonDocument()).ToListAsync();
        }

        private async Task<IEnumerable<Airport>> FindAirportsAsync()
        {
            var client = __client.Value;
            var database = client.GetDatabase("flights");
            var collection = database.GetCollection<Airport>("airports");

            return await collection.Find(new BsonDocument()).ToListAsync();
        }

        private async Task<SearchResultViewModel> CreateSearchResultViewModelAsync(SearchCriteriaModel criteriaModel)
        {
            var client = __client.Value;
            var database = client.GetDatabase("flights");
            var collection = database.GetCollection<Flight>("flights");

            var aggregateOfFlight = collection.Aggregate();

            var filter = CreateFilter(criteriaModel);
            if (filter != null)
            {
                aggregateOfFlight = aggregateOfFlight.Match(filter);
            }

            var aggregateOfSearchResultViewModel = aggregateOfFlight.Group(
                f => 1,
                g => new SearchResultViewModel
                {
                    TotalNumberOfFlights = g.Count(),
                    TotalNumberOfDelayedFlights = g.Sum(f => f.ArrivalDelay > 0.0 ? 1 : 0),
                    AverageDelayInMinutes = (double)g.Average(f => f.ArrivalDelay > 0.0 ? (double?)f.ArrivalDelay : null)
                });

            return await aggregateOfSearchResultViewModel.SingleAsync();
        }

        private FilterDefinition<Flight> CreateFilter(SearchCriteriaModel criteriaModel)
        {
            var filterBuilder = Builders<Flight>.Filter;
            var clauses = CreateClauses(criteriaModel, filterBuilder);

            if (clauses.Count > 0)
            {
                return filterBuilder.And(clauses);
            }
            else
            {
                return null;
            }
        }

        private List<FilterDefinition<Flight>> CreateClauses(SearchCriteriaModel criteriaModel, FilterDefinitionBuilder<Flight> filterBuilder)
        {
            var clauses = new List<FilterDefinition<Flight>>();
            if (criteriaModel.FromDate != null)
            {
                var fromDate = DateTime.SpecifyKind(DateTime.Parse(criteriaModel.FromDate), DateTimeKind.Utc);
                var clause = filterBuilder.Gte(f => f.FlightDate, fromDate);
                clauses.Add(clause);
            }
            if (criteriaModel.ToDate != null)
            {
                var toDate = DateTime.SpecifyKind(DateTime.Parse(criteriaModel.ToDate), DateTimeKind.Utc);
                var clause = filterBuilder.Lte(f => f.FlightDate, toDate);
                clauses.Add(clause);
            }
            if (criteriaModel.AirlineId != null)
            {
                var clause = filterBuilder.Eq(f => f.AirlineId, criteriaModel.AirlineId.Value);
                clauses.Add(clause);
            }
            if (criteriaModel.OriginId != null)
            {
                var clause = filterBuilder.Eq(f => f.OriginAirportId, criteriaModel.OriginId.Value);
                clauses.Add(clause);
            }
            if (criteriaModel.DestinationId != null)
            {
                var clause = filterBuilder.Eq(f => f.DestinationAirportId, criteriaModel.DestinationId.Value);
                clauses.Add(clause);
            }
            return clauses;
        }
    }
}
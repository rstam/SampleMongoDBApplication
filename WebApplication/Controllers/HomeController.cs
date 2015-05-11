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
                FromDate = null,
                ToDate = null,
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
            var collection = database.GetCollection<BsonDocument>("flights");

            var aggregate = collection.Aggregate();
            var filter = CreateFilter(criteriaModel);
            if (filter != null)
            {
                aggregate = aggregate.Match(filter);
            }
            aggregate = aggregate.Group(
                "{" +
                "    _id : null," +
                "    TotalNumberOfFlights : { $sum : 1 }," +
                "    TotalNumberOfDelayedFlights : { $sum : { $cond : { if : { $gt : [\"$ARR_DELAY\", 0] }, then : 1, else : 0 } } }," +
                "    AverageDelayInMinutes : { $avg : { $cond : { if : { $gt : [\"$ARR_DELAY\", 0] }, then : \"$ARR_DELAY\", else : null } } }" +
                "}"
            );
            var result = await aggregate.SingleAsync();

            return new SearchResultViewModel
            {
                TotalNumberOfFlights = result["TotalNumberOfFlights"].ToInt32(),
                TotalNumberOfDelayedFlights = result["TotalNumberOfDelayedFlights"].ToInt32(),
                AverageDelayInMinutes = result["AverageDelayInMinutes"].ToDouble()
            };
        }

        private FilterDefinition<BsonDocument> CreateFilter(SearchCriteriaModel criteriaModel)
        {
            var filterBuilder = Builders<BsonDocument>.Filter;

            var clauses = new List<FilterDefinition<BsonDocument>>();
            if (criteriaModel.FromDate != null)
            {
                var fromDate = DateTime.SpecifyKind(DateTime.Parse(criteriaModel.FromDate), DateTimeKind.Utc);
                var clause = filterBuilder.Gte("FL_DATE", fromDate);
                clauses.Add(clause);
            }
            if (criteriaModel.ToDate != null)
            {
                var toDate = DateTime.SpecifyKind(DateTime.Parse(criteriaModel.ToDate), DateTimeKind.Utc);
                var clause = filterBuilder.Lte("FL_DATE", toDate);
                clauses.Add(clause);
            }
            if (criteriaModel.AirlineId != null)
            {
                var clause = filterBuilder.Eq("AIRLINE_ID", criteriaModel.AirlineId.Value);
            }
            if (criteriaModel.OriginId != null)
            {
                var clause = filterBuilder.Eq("ORIGIN_AIRPORT_ID", criteriaModel.OriginId.Value);
            }
            if (criteriaModel.DestinationId != null)
            {
                var clause = filterBuilder.Eq("DESTINATION_AIRPORT_ID", criteriaModel.DestinationId.Value);
            }

            if (clauses.Count > 0)
            {
                return filterBuilder.And(clauses);
            }
            else
            {
                return null;
            }
        }
    }
}
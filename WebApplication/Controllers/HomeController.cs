using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
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
            var carriers = new Carrier[0];
            var origins = new Airport[0];
            var destinations = new Airport[0];

            return new IndexViewModel
            {
                From = new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                To = new DateTime(2014, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                Carriers = carriers,
                Origins = origins,
                Destinations = destinations
            };
        }

        private async Task<SearchResultViewModel> CreateSearchResultViewModelAsync(SearchCriteriaModel criteriaModel)
        {
            return new SearchResultViewModel
            {
                TotalNumberOfFlights = 1,
                TotalNumberOfDelayedFlights = 2,
                AverageDelay = TimeSpan.FromMinutes(3)
            };
        }
    }
}
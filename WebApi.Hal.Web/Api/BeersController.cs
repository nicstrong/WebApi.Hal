using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApi.Hal.Web.Api.Resources;
using WebApi.Hal.Web.Data;
using WebApi.Hal.Web.Data.Queries;
using WebApi.Hal.Web.Models;

namespace WebApi.Hal.Web.Api
{
    public class BeersController : Controller
    {
        public const int PageSize = 5;

        readonly IRepository repository;

        public BeersController(IRepository repository)
        {
            this.repository = repository;
        }

        // GET beers
        [HttpGet("beers")]
        public BeerListRepresentation Get([FromQuery]string searchTerm, [FromQuery]int page = 1)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                var beers = repository.Find(new GetBeersQuery(), page, PageSize);

                var resourceList = new BeerListRepresentation(beers.ToList(), beers.TotalResults, beers.TotalPages,
                    page, LinkTemplates.Beers.GetBeers);

                return resourceList;
            }
            else
            {
                var beers = repository.Find(new GetBeersQuery(b => b.Name.Contains(searchTerm)), page, PageSize);

                // snap page back to actual page found
                if (page > beers.TotalPages) page = beers.TotalPages;

                //var link = LinkTemplates.Beers.SearchBeers.CreateLink(new { searchTerm, page });
                var beersResource = new BeerListRepresentation(beers.ToList(), beers.TotalResults, beers.TotalPages,
                    page,
                    LinkTemplates.Beers.SearchBeers,
                    new {searchTerm})
                {
                    Page = page,
                    TotalResults = beers.TotalResults
                };

                return beersResource;
            }
        }

        // POST beers
        [HttpPost("beers")]
        public IActionResult Post(BeerRepresentation value)
        {
            var newBeer = new Beer(value.Name);
            repository.Add(newBeer);

            return Created(LinkTemplates.Beers.Beer.CreateUri(new { id = newBeer.Id }), newBeer);
        }
    }
}
﻿using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi.Hal.Web.Api.Resources;
using WebApi.Hal.Web.Data;

namespace WebApi.Hal.Web.Api
{
    public class BeerController : Controller
    {
        readonly IBeerDbContext beerDbContext;

        public BeerController(IBeerDbContext beerDbContext, JsonOutputFormatter outputFormatter)
        {
            this.beerDbContext = beerDbContext;
        }

        // GET beers/5
        [HttpGet("beers/{id:int}")]
        public BeerRepresentation Get(int id)
        {
            var beer = beerDbContext.Beers.Include("Brewery").Include("Style").Single(br => br.Id == id); // lazy loading isn't on for this query; force loading

            return new BeerRepresentation
            {
                Id = beer.Id,
                Name = beer.Name,
                BreweryId = beer.Brewery == null ? (int?)null : beer.Brewery.Id,
                BreweryName = beer.Brewery == null ? null : beer.Brewery.Name,
                StyleId = beer.Style == null ? (int?)null : beer.Style.Id,
                StyleName = beer.Style == null ? null : beer.Style.Name,
                ReviewIds = beerDbContext.Reviews.Where(r => r.Beer_Id == id).Select(r => r.Id).ToList()
            };
        }

        // PUT beers/5
        public void Put(int id, string value)
        {
        }

        // DELETE beers/5
        public void Delete(int id)
        {
        }
    }
}
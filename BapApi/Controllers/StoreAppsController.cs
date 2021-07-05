// https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio
// https://entityframework.net/linq-queries

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BapApi.Models;

namespace BapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreAppsController : ControllerBase
    {
        private readonly StoreAppsContext _context;

        public StoreAppsController(StoreAppsContext context)
        {
            _context = context;
        }

        // GET: api/StoreApps (StoreApps as in StoreAppsController, line 17)
        // Get all the data from the database
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<StoreAppDTO>>> GetStoreApps()
        {
            return await _context.StoreApps.Select(x => StoreAppToDTO(x)).ToListAsync();
        }

        // GET: api/StoreApps/{searchString}
        // Get data from DB where the name contains {search}
        [HttpGet("{search}")]
        public async Task<ActionResult<StoreAppDTO>> GetSearchResults(string search)
        {
            //eliminate case by placing all in lower
            var lowerCaseSearchString = search.ToLower();
            //Query against the DB - Where input contains lowerCaseSearchString in the Name add to list
            var searchResults = await _context.StoreApps.Where(a => a.Name.ToLower().Contains(lowerCaseSearchString)).ToListAsync();

            if (searchResults == null)
            {
                return NotFound();
            }

            return Ok(searchResults);
        }

        // GET: api/StoreApps/category/{category}
        // Get data from DB where the Category contains {category}
        [HttpGet("category/{category}")]
                public async Task<ActionResult<StoreAppDTO>> GetCategory(string category)
                {
                    //eliminate case by placing all in lower
                    var lowerCaseCategory = category.ToLower();
                    //Query against the DB - Where input contains lowerCaseCategory in the Category, add to list
                    var searchResults = await _context.StoreApps.Where(a => a.Category.ToLower().Contains(lowerCaseCategory)).ToListAsync();

                    if (searchResults == null)
                    {
                        return NotFound();
                    }

                    return Ok(searchResults);
                }

        // GET: api/StoreApps/1
        // Get a single row from the database by Id
        [HttpGet("id/{id}")]
        public async Task<ActionResult<StoreAppDTO>> GetStoreApp(int id)
        {
            var storeApp = await _context.StoreApps.FindAsync(id);

            if (storeApp == null)
            {
                return NotFound();
            }

            return StoreAppToDTO(storeApp);
        }

        // GET: api/StoreApps/rating/{rating}
        // Return the data for selected rating and above from DB e.g if 3.0 selected return apps of 3.0, 3.5, 4.0, 4.5 and 5.0
        [HttpGet("rating/{rating}")]
        public async Task<ActionResult<StoreAppDTO>> GetRating(double rating)
        {
            var searchResults = await _context.StoreApps.Where(a => a.Rating >= rating).ToListAsync();

            if (searchResults == null)
            {
                return NotFound();
            }

            return Ok(searchResults);
        }

        [HttpGet("people/{people}")]
        public async Task<ActionResult<StoreAppDTO>> GetPeople(int people)
        {
            var searchResults = await _context.StoreApps.Where(a => a.People >= people).ToListAsync();

            if (searchResults == null)
            {
                return NotFound();
            }

            return Ok(searchResults);
        }


        // GET: api/StoreApps/FirstTen
        // Get the first ten results from the database aftering ordering the data by Id
        [HttpGet("FirstTen")]
        public async Task<ActionResult<IEnumerable<StoreAppDTO>>> GetStoreTopTen()
        {

            var storeTopTen = await _context.StoreApps.Select(x => StoreAppToDTO(x)).Take(10).ToListAsync();

            if (storeTopTen == null)
            {
                return NotFound();
            }
            
            return storeTopTen; 
        }

        // POST: api/StoreApps
        // Add a new record to the database

        // Delete: api/StoreApps/1
        // Delete a single row from the database by Id

        // DTO helper method. "Production apps typically limit the data that's input and returned using a subset of the model"
        private static StoreAppDTO StoreAppToDTO(StoreApp storeApp) =>
            new StoreAppDTO
            {
                Id = storeApp.Id,
                Name = storeApp.Name,
                Rating = storeApp.Rating,
                People = storeApp.People,
                Category = storeApp.Category,
                Date = storeApp.Date,
                Price = storeApp.Price
            };
    }

}

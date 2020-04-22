using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Request;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{

    [ApiController]
    [Route("api/animals")]
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalsDbService _dbService;

        public AnimalsController(IAnimalsDbService dbService)
        {
            this._dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetAnimals(string sortBy)
        {
            var result = _dbService.GetAnimals(sortBy);
            if (result == null)
            {
                ObjectResult res = new ObjectResult(result);
                res.StatusCode = 400;
                return res;
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateAnimal(AnimalRequest animal)
        {
           
                var res = _dbService.CreateAnimal(animal);
                if (res == null)  
                return BadRequest();
                return Created("", res);
            
           
        }
    }
}

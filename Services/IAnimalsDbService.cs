using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Request;

namespace WebApplication2.Services
{
    public interface IAnimalsDbService
    {
        public IEnumerable<Animal> GetAnimals(string sortBy);

        public AnimalRequest CreateAnimal(AnimalRequest animal);
    }
}

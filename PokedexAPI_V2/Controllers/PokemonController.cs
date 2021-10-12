using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokedexAPI_V2.Models;
using PokedexAPI_V2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexAPI_V2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet]
        public List<Pokemon> GetAll()
        {
            return _pokemonService.GetAll();
        }

        [HttpGet("{id}")]
        public Pokemon GetById([FromRoute] int id)
        {
            return _pokemonService.GetById(id);
        }

        [HttpDelete("{id}")]
        public void DeleteById([FromRoute] int id)
        {
            _pokemonService.DeletePokemon(id);
        }

        [HttpPost]
        public ActionResult Add([FromBody] Pokemon newPkm)
        {
            if (newPkm.Types.Length > 2)
            {
                return BadRequest();
            }
            _pokemonService.AddPokemon(newPkm);

            return StatusCode(201);
        }

        [HttpPut("{id}")]
        public void Update([FromRoute] int id, [FromBody] Pokemon updatedPkm)
        {
            _pokemonService.UpdatePokemon(id, updatedPkm);
        }

        [HttpGet("by-type/{type}")]
        public List<Pokemon> GetAllByType([FromRoute] string type)
        {
            return _pokemonService.GetAllByType(type);
        }

        [HttpGet("by-weight")]
        public List<Pokemon> GetAllByWeight([FromQuery] double minWeight, [FromQuery] double maxWeight)
        {
            return _pokemonService.GetAllByWeight(minWeight, maxWeight);
        }
    }
}

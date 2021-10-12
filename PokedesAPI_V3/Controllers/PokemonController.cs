using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokedexAPI_V3.Data;
using PokedexAPI_V3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexAPI_V3.Controllers
{
    [Route("v3/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly PokemonDao _dao;

        public PokemonController(PokemonDao dao)
        {
            _dao = dao;
        }

        [HttpGet]
        public List<Pokemon> GetAll()
        {
            return _dao.GetAll();
        }

    }
}

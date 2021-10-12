using Microsoft.Extensions.Configuration;
using PokedexAPI_V3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace PokedexAPI_V3.Data
{
    // Questa classe mi serve per gestire soltanto i metodi CRUD su DB
    // per la tabella Types
    public class PokemonTypesDao : Dao
    {
        public PokemonTypesDao(IConfiguration config) : base(config)
        {
        }

        public List<PokemonType> GetAll()
        {
            var results = db.Read("SELECT * FROM types;");
            return results.Select(result => FromDictionary(result))
                          .ToList();
        }

        public PokemonType GetById(int id)
        {
            return FromDictionary
            (
                db.ReadOne($"SELECT * FROM types WHERE id = {id};")
            );
        }

        // TODO completare con il resto del CRUD
        public void Update(int id, PokemonType pokemonType)
        {
            // TODO: 
            db.Update("");
        }

        private PokemonType FromDictionary(Dictionary<string, string> result)
        {
            if (result is null)
            {
                return null;
            }

            return new PokemonType { Id = int.Parse(result["id"]), Type = result["type"] };
        }
    }
}

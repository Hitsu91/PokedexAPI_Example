using Microsoft.Extensions.Configuration;
using PokedexAPI_V3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexAPI_V3.Data
{
    public class GenerationsDao : Dao
    {
        public GenerationsDao(IConfiguration config) : base(config) { }

        public List<Generation> GetAll()
        {
            return db.Read("SELECT * FROM generations;")
                     .Select(result => FromDictionary(result))
                     .ToList();
        }

        public Generation GetById(int id)
        {
            return FromDictionary
            (
                db.ReadOne($"SELECT * FROM generations WHERE id = {id};")
            );
        }

        private Generation FromDictionary(Dictionary<string, string> result)
        {
            return new Generation
            {
                Id = int.Parse(result["id"]),
                Gen = int.Parse(result["generation"])
            };
        }
    }
}

using Microsoft.Extensions.Configuration;
using PokedexAPI_V3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexAPI_V3.Data
{
    public class PokemonDao : Dao
    {
        private readonly GenerationsDao _generationsDao;
        private readonly PokemonTypesDao _pokemonTypesDao;
        private readonly MovesDao _movesDao;

        public PokemonDao(
            IConfiguration config,
            GenerationsDao generationsDao,
            PokemonTypesDao pokemonTypesDao,
            MovesDao movesDao) : base(config)
        {
            _generationsDao = generationsDao;
            _pokemonTypesDao = pokemonTypesDao;
            _movesDao = movesDao;
        }

        // Resituisco senza mosse
        public List<Pokemon> GetAll()
        {
            var results = db.Read("SELECT * FROM pokemon;");

            return ListFromResults(results);
        }

        private List<Pokemon> ListFromResults(List<Dictionary<string, string>> results)
        {
            return results.Select(result =>
            {
                var pokemonId = int.Parse(result["id"]);
                return new Pokemon
                {
                    Id = pokemonId,
                    Name = result["name"],
                    Weight = double.Parse(result["weight"]),
                    Generation = _generationsDao.GetById(int.Parse(result["generation_id"])),
                    Types = TypesFromPokemonId(pokemonId),
                    Moves = MovesFromPokemonId(pokemonId)
                };
            }).ToList();
        }

        /// <summary>
        /// Metodo che attraverso l'id del pokémon va a sfruttare il Dao Delle mosse
        /// Per fare ORM delle Mosse da assegnare al Pokémon
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Move[] MovesFromPokemonId(int id)
        {
            // TODO: Mancano Generazione e Tipi

            var movesResults = db.Read($"SELECT * FROM pokemon_moves WHERE pokemon_id = {id}");

            return movesResults
                                .Select(dic =>
                                {
                                    var moveId = int.Parse(dic["move_id"]);
                                    return _movesDao.GetById(moveId);
                                }).ToArray();
        }

        private PokemonType[] TypesFromPokemonId(int id)
        {
            // Devo trovare attraverso la tabella associativa dei tipi, quali sono quelli
            // assegnati al pokemon
            var typesResults = db.Read($"SELECT * FROM pokemon_types WHERE pokemon_id = {id}");

            // Il risultato della select della tabella associativa contiene
            // l'informazione dell'id del tipo associato al pokémon che sto costruendo
            // Quindi, sfuttando il dao per la gestione dei types, posso andare a recuperare
            // l'oggetto, noi dalla lista di risultati lo proiettiamo con il metodo Select
            // in un array di Type
            var pokemonTypes = typesResults
                                .Select(dic =>
                                {
                                    var typeId = int.Parse(dic["type_id"]);
                                    return _pokemonTypesDao.GetById(typeId);
                                })
                                .ToArray();
            return pokemonTypes;
        }
    }


}

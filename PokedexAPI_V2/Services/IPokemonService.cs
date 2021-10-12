using PokedexAPI_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexAPI_V2.Services
{
    public interface IPokemonService
    {
        List<Pokemon> GetAll();
        Pokemon GetById(int id);
        void AddPokemon(Pokemon newPkm);
        void DeletePokemon(int id);
        void UpdatePokemon(int id, Pokemon updatedPkm);
    }
}

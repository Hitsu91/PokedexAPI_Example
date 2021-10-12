using Microsoft.Extensions.Configuration;
using PokedexAPI_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace PokedexAPI_V2.Services
{
    public class PokemonService : IPokemonService
    {
        private const string Query = "SELECT * FROM pokemon";
        private readonly Database db;

        // Al service per mezzo della DI, mi faccio fornire un oggetto di tipo 
        // IConfiguration, grazie questo posso accedere alle varie configurazioni
        // a livello di progetto; il nostro scopo è utilizzarlo per andare a recuperare
        // il nome del DB da utilizzare!
        public PokemonService(IConfiguration config)
        {
            // La stringa che passo al metodo GetValue serve per andare a recuperare
            // una determinata stringa di configurazione, nel nostro caso
            // alla chiave di configurazione DbName andremo ad associare il nome del DB
            // da utilizzare. Il motivo per fare tutto questo lavoro in più
            // è di dare la possibilità di cambiare il dabatase a livello di configurazione
            // AL POSTO DI modificare direttamente il codice!!!
            var dbName = config.GetValue<string>(key: "Db:Name");
            // Questo passaggio è molto utile quando voglio esternalizzare
            // Parte di configurazione dell'applicazione
            var dbHost = config.GetValue<string>("Db:Host");
            // L'oggetto di tipo IConfiguration ha la possibilità di essere utilizzato
            // come una mappa [key]
            var dbUsername = config["Db:Username"];
            var dbPass = config["Db:Password"];

            db = new Database(dbName, dbHost, dbUsername, dbPass);
        }

        public void AddPokemon(Pokemon newPkm)
        {
            db.Update
            (
                $"INSERT INTO pokemon (name, types, weight) " +
                $"VALUES('{newPkm.Name}', '{string.Join(',', newPkm.Types)}', {newPkm.Weight});"
            );
        }

        public void DeletePokemon(int id)
        {
            db.Update($"DELETE FROM pokemon WHERE id = {id};");
        }

        public List<Pokemon> GetAll()
        {
            var results = db.Read(Query);
            return results.Select(result => FromDictionary(result))
                          .ToList();
        }

        public List<Pokemon> GetAllByType(string type)
        {
            var results = db.Read($"SELECT * FROM pokemon WHERE types LIKE '%{type}%'");
            return results.Select(result => FromDictionary(result))
                          .ToList();
        }

        public List<Pokemon> GetAllByWeight(double minWeight, double maxWeight)
        {
            var results = db.Read
            (
                $"SELECT * FROM pokemon WHERE weight BETWEEN {minWeight} AND {maxWeight};"
            );
            return results.Select(result => FromDictionary(result))
                          .ToList();
        }

        public Pokemon GetById(int id)
        {
            var result = db.ReadOne($"SELECT * FROM pokemon WHERE id = {id}");
            return FromDictionary(result);
        }

        public void UpdatePokemon(int id, Pokemon updatedPkm)
        {
            db.Update
            (
                $"UPDATE pokemon SET name = '{updatedPkm.Name}', " +
                $"types = '{string.Join(',', updatedPkm.Types)}', " +
                $"weight = {updatedPkm.Weight} " +
                $"WHERE id = {id};"
            );
        }

        // Applico il DRY, don't repeat yourself!
        // Quando possibile, rifattorizzare parti di metodi in altri metodi
        // e riutilizzarli, invece di fare copia/incolla
        private Pokemon FromDictionary(Dictionary<string, string> dictionary)
        {
            if (dictionary is null)
            {
                return null;
            }
            return new Pokemon
            {
                Id = int.Parse(dictionary["id"]),
                Name = dictionary["name"],
                Types = dictionary["types"].Split(","),
                Weight = double.Parse(dictionary["weight"])
            };
        }

    }
}

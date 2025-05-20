using AnimeFunctionCode.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeFunctionCode.Data
{
    internal class NarutoCRUD
    {
        private IMongoDatabase db;

        public NarutoCRUD(string database)
        {
            var connectionString = Environment.GetEnvironmentVariable("CosmosString");
            var client = new MongoClient(connectionString); //lägger in connectionstring här men när den är tom använder den lokal db.
            db = client.GetDatabase(database);
        }

        public async Task AddCharacter (string container, NarutoCharacters character)
        {
            try
            {
                var collection = db.GetCollection<NarutoCharacters>(container);
                await collection.InsertOneAsync(character);
                Console.WriteLine($"Added {character.Name}. Dattebayo!");

            } catch (Exception ex)
            {
                new ApplicationException($"Failed to add character. Even Jiraiya couldn't save this insert. Error message: {ex.Message}");
            }

        }

        public async Task<List<NarutoCharacters>> GetCharacters (string container)
        {
            try
            {
                var collection = db.GetCollection<NarutoCharacters>(container);
                return await collection.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                new ApplicationException($"Failed to fetch characters. Good going, Orochimaru's summoning has nothing on you. Error message: {ex.Message}");
                return null;
            }
        }

        public async Task<NarutoCharacters?> GetCharacterById (string container, Guid id)
        {
            try
            {
                var collection = db.GetCollection<NarutoCharacters>(container);
                var find = await collection.FindAsync(c => c.Id == id);
                return await find.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                new ApplicationException($"Failed to fetch characters. What is this, a search for Sakura’s fanbase? Error message: {ex.Message}");
                return null;
            }
        }

        public async Task UpdateCharacterById(NarutoCharacters character, string container)
        {
            try
            {
                var collection = db.GetCollection<NarutoCharacters>(container);
                await collection.ReplaceOneAsync(c => c.Id == character.Id, character, new ReplaceOptions { IsUpsert = true });
                Console.WriteLine($"{character.Name}'s data has been updated. Unlike Naruto’s warderobe, this data is now fresh.");
            }
            catch (Exception ex)
            {
                new ApplicationException($"Failed to update character harder than Konohamaru failed at leading Team 7. Error message: {ex.Message}");
            }
        }

        public async Task DeleteCharacter (string container, Guid id)
        {
            try
            {
                var collection = db.GetCollection<NarutoCharacters>(container);
                var character = collection.Find(c => c.Id == id);
                var name = character.FirstOrDefault();
                if (name != null)
                {
                    Console.WriteLine($"Deleted {name.Name} from the collection. Don't worry, they'll be back in a flashback episode.");

                }
                var characterToDelete = await collection.DeleteOneAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                new ApplicationException($"Failed to delete character. They must've used reanimation jutsu. Error message: {ex.Message}");
            }
        }
    }
}

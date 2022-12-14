using System;
using System.Collections.Generic;
using System.Text;
using TareaMongo.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace TareaMongo.Services
{
    public class PersonService
    {
        private readonly IMongoCollection<Person> _personsCollection;
        public PersonService()
        {
            _personsCollection = new MongoClient(Item.client)
                .GetDatabase(Item.database)
                .GetCollection<Person>(Item.collection);
        }
        public async Task<List<Person>> GetAsync() =>
        await _personsCollection.Find(_ => true).ToListAsync();

        public async Task<Person> GetAsync(string id) =>
            await _personsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Person newPerson) =>
            await _personsCollection.InsertOneAsync(newPerson);

        public async Task UpdateAsync(string id, Person updatedPerson) =>
            await _personsCollection.ReplaceOneAsync(x => x.Id == id, updatedPerson);

        public async Task RemoveAsync(string id) =>
            await _personsCollection.DeleteOneAsync(x => x.Id == id);
    }

    public class Item
    {
        public static string client = "mongodb://localhost:27017";
        public static string database = "person";
        public static string collection = "persons";
    }
}

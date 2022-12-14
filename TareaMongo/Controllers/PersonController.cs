using System;
using System.Collections.Generic;
using System.Text;
using TareaMongo.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TareaMongo.Services;


namespace TareaMongo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        private readonly PersonService _personsService;

        public PersonController(PersonService personService)
        {
            _personsService = personService;
        }

        [HttpGet]
        public async Task<List<Person>> Get() =>
               await _personsService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Person>> GetByID(string id)
        {
            var person = await _personsService.GetAsync(id);

            if (person is null)
            {
                return NotFound();
            }

            return person;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Person newPerson)
        {
            await _personsService.CreateAsync(newPerson);
            return CreatedAtAction(nameof(Get), new { id = newPerson.Id }, newPerson);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Person updatedPerson)
        {
            var person = await _personsService.GetAsync(id);

            if (person is null)
            {
                return NotFound();
            }

            updatedPerson.Id = person.Id;

            await _personsService.UpdateAsync(id, updatedPerson);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var person = await _personsService.GetAsync(id);

            if (person is null)
            {
                return NotFound();
            }

            await _personsService.RemoveAsync(id);

            return NoContent();
        }
    }
}
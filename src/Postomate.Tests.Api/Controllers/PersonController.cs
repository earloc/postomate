﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Postomate.Tests.Api.Controllers
{


    public class Person
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
    }

    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> logger;

        public PersonController(ILogger<PersonController> logger, IDictionary<string, Person> persons)
        {
            this.logger = logger;
            this.persons = persons;
        }

        private readonly IDictionary<string, Person> persons;

        [HttpGet]
        public IEnumerable<Person> Get() => persons.Values;

        [HttpPost]
        public ActionResult Post(string firstName, string surname) {
            var person = new Person()
            {
                FirstName = firstName,
                Surname = surname
            };

            persons.Add(person.Id, person);

            return Ok(person);
        }

        //[HttpPatch]
        //public ActionResult Post(string personId, JsonPatchDocument<Person> patch)
        //{
        //    if (!persons.ContainsKey(personId)) {
        //        return NotFound();
        //    }

        //    var person = persons[personId];

        //    patch.ApplyTo(person);

        //    return Ok(person);
        //}

        //[HttpPut]
        //public ActionResult<Person> Put(Person person)
        //{
        //    person.CreatedAt = DateTimeOffset.Now;
        //    person.Id = Guid.NewGuid().ToString();

        //    persons[person.Id] = person;

        //    return Ok(person);
        //}

        //[HttpDelete]
        //public ActionResult<Person> Delete(string personId)
        //{

        //    if (!persons.ContainsKey(personId)) {
        //        return NotFound();
        //    }

        //    persons.Remove(personId);

        //    return Ok();
        //}

    }
}

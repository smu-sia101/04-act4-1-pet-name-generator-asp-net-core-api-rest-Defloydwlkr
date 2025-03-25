using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace RandomPetNameGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetNameGeneratorController : ControllerBase
    {
        private static readonly Dictionary<string, List<string>> PetNames = new()
        {
            ["dog"] = new() { "Buddy", "Max", "Charlie", "Rocky", "Rex" },
            ["cat"] = new() { "Whiskers", "Mittens", "Luna", "Simba", "Tiger" },
            ["bird"] = new() { "Tweety", "Sky", "Chirpy", "Raven", "Sunny" }
        };

        private readonly Random _random = new();

        [HttpPost("generate")]
        public IActionResult GeneratePetName([FromBody] PetNameRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.AnimalType) ||
                !PetNames.TryGetValue(request.AnimalType.Trim().ToLower(), out var names))
            {
                return BadRequest(new { error = "Invalid or missing animal type. Allowed: dog, cat, bird." });
            }

            string generatedName = names[_random.Next(names.Count)];
            if (request.TwoPart == true)
                generatedName += " " + names[_random.Next(names.Count)];

            return Ok(new { name = generatedName });
        }
    }

    public record PetNameRequest(string AnimalType, bool? TwoPart);
}

using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers;


[ApiController]
[Route("experiences")]
public class ExperiencesController : ControllerBase
{
    // Dummy data: Liste af hardcodede Experience-objekter
    private static readonly List<Experience> Experiences = new List<Experience>
    {
        new Experience {Name = "Tur over graensen", Description = "En tur ned over graensen for at hente bajer.", Price = 1000},
        new Experience {Name = "Koere Gokart", Description = "Koere Gokart med gutterne.", Price = 500},
        new Experience {Name = "En tur i storcenter Nord", Description = "Et saet franske hotdogs", Price = 30}
    };

    // HTTP GET Endpoint
    [HttpGet]
    public IEnumerable<Experience> Get()
    {
        return Experiences;
    }
}

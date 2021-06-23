using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spanApp.Models;
using spanApp.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace spanApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PodaciController : ControllerBase
    {
        private readonly spanDbContext context;

        public PodaciController(spanDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PodaciDTO>>> GetPodaci()
        {
            return await context.Podacis
                .Select(x => PodaciToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PodaciDTO>> GetPodatak(long id)
        {
            var podatak = await context.Podacis.FindAsync(id);

            if (podatak == null)
            {
                return NotFound();
            }

            return PodaciToDTO(podatak);
        }

        private static PodaciDTO PodaciToDTO(Podaci podatak) =>
            new PodaciDTO
            {
                FirstName = podatak.FirstName,
                City = podatak.City,
                LastName = podatak.LastName,
                ZipCode = podatak.ZipCode,
                PhoneNumber = podatak.PhoneNumber
            };
    }
}

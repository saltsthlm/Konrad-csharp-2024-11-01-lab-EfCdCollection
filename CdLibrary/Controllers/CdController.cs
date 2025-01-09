namespace CdLibrary.Controllers;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CdLibrary.Models;
using CdLibrary.Data;

[ApiController]
[Route("api/[controller]")]
public class CdsController : Controller
{
    private readonly CdContext _context;

    public CdsController(CdContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cd>>> GetCds([FromQuery] string? genre)
    {
        var query = _context.Cd.Include(cd => cd.Genre).AsQueryable();

        if (!string.IsNullOrEmpty(genre))
        {
            query = query.Where(cd => cd.Genre != null && cd.Genre.Name == genre);
        }

        var list = await query.ToListAsync();
        return list;
    }

    [HttpPost]
    public async Task<ActionResult<Cd>> PostCd(Cd cd)
    {
        _context.Cd.Add(cd);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCd", new { id = cd.Id }, cd);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cd>> GetCd(int id)
    {
        var cd = await _context.Cd.FindAsync(id);

        if (cd == null)
        {
            return NotFound();
        }

        return cd;
    }

    [HttpPut("{id}/artist")]
    public async Task<IActionResult> UpdateCd(int id, string artist)
    {
        var cd = await _context.Cd.FindAsync(id);

        if (cd == null)
        {
            return NotFound();
        }

        cd.Artist = artist;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!CdExists(id))
        {
            return NotFound();
        }

        return NoContent();
        
    }

    [HttpPut("{id}/genre")]
    public async Task<IActionResult> UpdateCd(int id, Genre genre)
    {
        var cd = await _context.Cd.FindAsync(id);

        if (cd == null)
        {
            return NotFound();
        }

        cd.Genre = genre;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!CdExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    private bool CdExists(int id)
    {
        return _context.Cd.Any(cd => cd.Id == id);
    }

}


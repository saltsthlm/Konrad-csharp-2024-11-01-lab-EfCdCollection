namespace CdLibrary.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CdLibrary.Models;
using CdLibrary.Data;

public class CdController : Controller
{
    private readonly CdContext _context;

    public CdController(CdContext context)
    {
        _context = context;
    }

    [HttpGet("{genre?}")]
    public async Task<ActionResult<IEnumerable<Cd>>> GetCd(string? genre)
    {
        var list = await _context.Cd.ToListAsync();
        if (genre != null)
        {
            return list;
        }
        return list.Where(cd => cd.Genre.Name == genre).ToList();
    }

}


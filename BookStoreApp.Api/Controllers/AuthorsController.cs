﻿using BookStoreApp.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthorsController : ControllerBase
	{
		private readonly BookStoreDbContext _context;

		public AuthorsController(BookStoreDbContext context)
		{
			_context = context;
		}

		// GET: api/Authors
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
		{
			return Ok(await _context.Authors.ToListAsync());
		}

		// GET: api/Authors/5
		[HttpGet("{id:int}")]
		public async Task<ActionResult<Author>> GetAuthor(int id)
		{
			var author = await _context.Authors.FindAsync(id);

			if (author == null)
			{
				return NotFound();
			}

			return Ok(author);
		}

		// PUT: api/Authors/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id:int}")]
		public async Task<IActionResult> PutAuthor(int id, Author author)
		{
			if (id != author.Id)
			{
				return BadRequest();
			}

			_context.Entry(author).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!AuthorExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Authors
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Author>> PostAuthor(Author author)
		{
			await _context.Authors.AddAsync(author);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
		}

		// DELETE: api/Authors/5
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteAuthor(int id)
		{
			var author = await _context.Authors.FindAsync(id);
			if (author == null)
			{
				return NotFound();
			}

			_context.Authors.Remove(author);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool AuthorExists(int id)
		{
			return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
using AutoMapper;
using BookStoreApp.Api.Data;
using BookStoreApp.Api.Models.Author;
using BookStoreApp.Api.Static;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthorsController : ControllerBase
	{
		private readonly BookStoreDbContext _context;
		private readonly IMapper _mapper;
		private readonly Logger<AuthorsController> _logger;

		public AuthorsController(BookStoreDbContext context, IMapper mapper,
			Logger<AuthorsController> logger)
		{
			_context = context;
			_mapper = mapper;
			_logger = logger;
		}

		// GET: api/Authors
		[HttpGet]
		public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>>
			GetAuthors()
		{
			try
			{
				var authors = await _context.Authors.ToListAsync();
				var authorReadOnlyDtos = _mapper.Map<IEnumerable<AuthorReadOnlyDto>>
					(authors);
				return Ok(authorReadOnlyDtos);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
				return StatusCode(500, Messages.Error500Message);
			}
		}

		// GET: api/Authors/5
		[HttpGet("{id:int}")]
		public async Task<ActionResult<AuthorReadOnlyDto>> GetAuthor(int id)
		{
			try
			{
				var author = await _context.Authors.FindAsync(id);

				if (author == null)
				{
					_logger.LogWarning($"Author not found for {id}");
					return NotFound();
				}

				var authorReadOnlyDto = _mapper.Map<AuthorReadOnlyDto>(author);
				return Ok(authorReadOnlyDto);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
				return StatusCode(500, Messages.Error500Message);
			}
		}

		// PUT: api/Authors/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id:int}")]
		public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
		{
			try
			{
				if (id != authorDto.Id)
				{
					_logger.LogWarning($"Author not found for {id}");
					return BadRequest();
				}

				var author = await _context.Authors.FindAsync(id);
				if (author == null)
					return NotFound();

				_mapper.Map(authorDto, author);

				_context.Entry(author).State = EntityState.Modified;

				try
				{
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!AuthorExists(id))
					{
						_logger.LogWarning($"Author not found for {id}");
						return NotFound();
					}
					else
					{
						throw;
					}
				}

				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
				return StatusCode(500, Messages.Error500Message);
			}
		}

		// POST: api/Authors
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
		{
			try
			{
				if (ModelState.IsValid)
				{
					_logger.LogWarning($"Model is not valid");
					return BadRequest(ModelState);
				}
				var author = _mapper.Map<Author>(authorDto);
				await _context.Authors.AddAsync(author);
				await _context.SaveChangesAsync();

				return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
				return StatusCode(500, Messages.Error500Message);
			}
		}

		// DELETE: api/Authors/5
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteAuthor(int id)
		{
			try
			{
				var author = await _context.Authors.FindAsync(id);
				if (author == null)
				{
					_logger.LogWarning($"Author not found for {id}");
					return NotFound();
				}

				_context.Authors.Remove(author);
				await _context.SaveChangesAsync();

				return NoContent();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
				return StatusCode(500, Messages.Error500Message);
			}
		}

		private bool AuthorExists(int id)
		{
			return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}

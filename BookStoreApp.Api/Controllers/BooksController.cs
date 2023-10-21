using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.Api.Data;
using BookStoreApp.Api.Models.Book;
using BookStoreApp.Api.Static;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BooksController : ControllerBase
	{
		private readonly BookStoreDbContext _context;
		private readonly IMapper _mapper;
		private readonly ILogger<BooksController> _logger;

		public BooksController(BookStoreDbContext context,
			IMapper mapper,
			ILogger<BooksController> logger)
		{
			_context = context;
			_mapper = mapper;
			_logger = logger;
		}

		// GET: api/Books
		[HttpGet]
		public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks()
		{
			try
			{
				var bookReadOnlyDtos = await _context.Books
					.Include(d => d.Author)
					.ProjectTo<BookReadOnlyDto>(_mapper.ConfigurationProvider)
					.ToListAsync();
				//var bookReadOnlyDtos = _mapper.Map<IEnumerable<BookReadOnlyDto>>(books);
				return Ok(bookReadOnlyDtos);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
				return StatusCode(500, Messages.Error500Message);
			}
		}

		// GET: api/Books/5
		[HttpGet("{id}")]
		public async Task<ActionResult<BookDetailsDto>> GetBook(int id)
		{
			try
			{
				var book = await _context.Books
						.Include(d => d.Author)
						.ProjectTo<BookDetailsDto>(_mapper.ConfigurationProvider)
						.FirstOrDefaultAsync(d => d.Id == id);

				if (book == null)
				{
					return NotFound();
				}
				return Ok(book);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
				return StatusCode(500, Messages.Error500Message);
			}
		}

		// PUT: api/Books/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto)
		{
			try
			{
				if (id != bookDto.Id)
				{
					return BadRequest();
				}

				var book = _context.Books.FirstOrDefault(d => d.Id == id);
				if (book == null)
				{
					return NotFound();
				}

				_mapper.Map(bookDto, book);
				_context.Entry(book).State = EntityState.Modified;

				try
				{
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!BookExists(id))
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
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
				return StatusCode(500, Messages.Error500Message);
			}
		}

		// POST: api/Books
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<BookDetailsDto>> PostBook(BookCreateDto bookDto)
		{
			try
			{
				var book = _mapper.Map<Book>(bookDto);
				_context.Books.Add(book);
				await _context.SaveChangesAsync();

				return CreatedAtAction(nameof(GetBook),
					new { id = book.Id }, book);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
				return StatusCode(500, Messages.Error500Message);
			}
		}

		// DELETE: api/Books/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBook(int id)
		{
			var book = await _context.Books.FindAsync(id);
			if (book == null)
			{
				return NotFound();
			}

			_context.Books.Remove(book);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool BookExists(int id)
		{
			return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}

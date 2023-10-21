using AutoMapper;
using BookStoreApp.Api.Data;
using BookStoreApp.Api.Models.Author;
using BookStoreApp.Api.Models.Book;

namespace BookStoreApp.Api.Configurations
{
	public class MapperConfig : Profile
	{
		public MapperConfig()
		{
			CreateMap<AuthorCreateDto, Author>().ReverseMap();
			CreateMap<AuthorUpdateDto, Author>().ReverseMap();
			CreateMap<AuthorReadOnlyDto, Author>().ReverseMap();

			CreateMap<BookCreateDto, Book>().ReverseMap();
			CreateMap<BookUpdateDto, Book>().ReverseMap();
			CreateMap<Book, BookReadOnlyDto>()
				.ForMember(d => d.AuthorName,
					d =>
						d.MapFrom(t => $"{t.Author.FirstName} {t.Author.LastName}"))
				.ReverseMap();
			CreateMap<Book, BookDetailsDto>()
				.ForMember(d => d.AuthorName,
					d =>
						d.MapFrom(t => $"{t.Author.FirstName} {t.Author.LastName}"))
				.ReverseMap();
		}
	}
}

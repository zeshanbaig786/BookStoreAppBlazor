﻿using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.Api.Data;

public partial class BookStoreDbContext : DbContext
{
	public BookStoreDbContext()
	{
	}

	public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
		: base(options)
	{
	}

	public virtual DbSet<Author> Authors { get; set; }

	public virtual DbSet<Book> Books { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Author>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Authors__3214EC07F0471EF8");

			entity.Property(e => e.Bio).HasMaxLength(250);
			entity.Property(e => e.FirstName).HasMaxLength(50);
			entity.Property(e => e.LastName).HasMaxLength(50);
		});

		modelBuilder.Entity<Book>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Books__3214EC07D6C0C4FE");

			entity.HasIndex(e => e.Isbn, "UQ__Books__447D36EA633E987C").IsUnique();

			entity.Property(e => e.Image).HasMaxLength(50);
			entity.Property(e => e.Isbn)
				.HasMaxLength(50)
				.HasColumnName("ISBN");
			entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
			entity.Property(e => e.Summary).HasMaxLength(250);
			entity.Property(e => e.Title).HasMaxLength(50);

			entity.HasOne(d => d.Author).WithMany(p => p.Books)
				.HasForeignKey(d => d.AuthorId)
				.HasConstraintName("FK_Books_ToAuthor");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.Api.Data;

public partial class BookStoreDbContext : IdentityDbContext<ApiUser>
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
		base.OnModelCreating(modelBuilder);



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

		modelBuilder.Entity<IdentityRole>().HasData(
			new IdentityRole
			{
				Id = "cdb3953f-a1e8-47fb-a8d9-d79455bc5de3",
				Name = "User",
				NormalizedName = "USER"
			},
			new IdentityRole
			{
				Id = "dba69f4a-e6e8-4483-9ec4-b439ee57fcdb",
				Name = "Administrator",
				NormalizedName = "ADMINISTRATOR"
			}
			);
		var hasher = new PasswordHasher<ApiUser>();
		modelBuilder.Entity<ApiUser>().HasData(
			new ApiUser
			{
				Id = "f597909f-f0cc-4840-a7ad-b9de190c2a99",
				Email = "admin@bookstore.com",
				UserName = "admin@bookstore.com",
				NormalizedEmail = "ADMIN@BOOKSTORE.COM",
				NormalizedUserName = "ADMIN@BOOKSTORE.COM",
				FirstName = "System",
				LastName = "Admin",
				PasswordHash = hasher.HashPassword(null, "SuperAdmin@123")
			},
			new ApiUser
			{
				Id = "c87fbf65-0f4b-498c-92b5-343e148ed144",
				Email = "user@bookstore.com",
				UserName = "user@bookstore.com",
				NormalizedEmail = "USER@BOOKSTORE.COM",
				NormalizedUserName = "USER@BOOKSTORE.COM",
				FirstName = "System",
				LastName = "User",
				PasswordHash = hasher.HashPassword(null, "Password@123")
			}
		);
		modelBuilder.Entity<IdentityUserRole<string>>()
			.HasData(
				new IdentityUserRole<string>()//admin to admin role
				{
					RoleId = "dba69f4a-e6e8-4483-9ec4-b439ee57fcdb",
					UserId = "f597909f-f0cc-4840-a7ad-b9de190c2a99"
				},
				new IdentityUserRole<string>()//user to user role
				{
					RoleId = "cdb3953f-a1e8-47fb-a8d9-d79455bc5de3",
					UserId = "c87fbf65-0f4b-498c-92b5-343e148ed144"
				}
			);
		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

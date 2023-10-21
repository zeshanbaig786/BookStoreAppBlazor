using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeededDefaultUsersAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "cdb3953f-a1e8-47fb-a8d9-d79455bc5de3", "31d93844-80e6-478e-b33a-7f6a06787f26", "User", "USER" },
                    { "dba69f4a-e6e8-4483-9ec4-b439ee57fcdb", "3582bec4-baf4-46be-8809-0ca6fce8a6bf", "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "c87fbf65-0f4b-498c-92b5-343e148ed144", 0, "42c200ff-cd2f-4e2e-959c-fb4046a99c86", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "USER@BOOKSTORE.COM", "AQAAAAEAACcQAAAAEIt6m0n28qw3H5+jCoiyabDg5DLy9L1fUe795Ob6MvyzcRNoG2iOcYG47gSfjti1mw==", null, false, "46569a62-a956-42f6-9efc-19c8fb44038b", false, "user@bookstore.com" },
                    { "f597909f-f0cc-4840-a7ad-b9de190c2a99", 0, "d2cc4859-c162-45c2-be51-e0ebff44080f", "admin@bookstore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAEAACcQAAAAEBHJ3cR86LS4k8vAE+khI/NYte30seTyYSKDBraeXs2t582pWQrNkgsXfYP6K+LJiQ==", null, false, "a5b46ef2-082a-4807-829d-46e053d7781e", false, "admin@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "cdb3953f-a1e8-47fb-a8d9-d79455bc5de3", "c87fbf65-0f4b-498c-92b5-343e148ed144" },
                    { "dba69f4a-e6e8-4483-9ec4-b439ee57fcdb", "f597909f-f0cc-4840-a7ad-b9de190c2a99" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "cdb3953f-a1e8-47fb-a8d9-d79455bc5de3", "c87fbf65-0f4b-498c-92b5-343e148ed144" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "dba69f4a-e6e8-4483-9ec4-b439ee57fcdb", "f597909f-f0cc-4840-a7ad-b9de190c2a99" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cdb3953f-a1e8-47fb-a8d9-d79455bc5de3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dba69f4a-e6e8-4483-9ec4-b439ee57fcdb");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c87fbf65-0f4b-498c-92b5-343e148ed144");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f597909f-f0cc-4840-a7ad-b9de190c2a99");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetNET.Migrations
{
    /// <inheritdoc />
    public partial class migrationfixusers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DletedAt",
                table: "Users",
                newName: "DeletedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "Users",
                newName: "DletedAt");
        }
    }
}

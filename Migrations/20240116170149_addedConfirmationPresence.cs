using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetNET.Migrations
{
    /// <inheritdoc />
    public partial class addedConfirmationPresence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Confirmation",
                table: "HistoriquePresences",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confirmation",
                table: "HistoriquePresences");
        }
    }
}

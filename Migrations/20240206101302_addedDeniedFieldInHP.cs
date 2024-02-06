using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetNET.Migrations
{
    /// <inheritdoc />
    public partial class addedDeniedFieldInHP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Confirmation",
                table: "HistoriquePresences",
                newName: "Denied");

            migrationBuilder.AddColumn<bool>(
                name: "Confirmed",
                table: "HistoriquePresences",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confirmed",
                table: "HistoriquePresences");

            migrationBuilder.RenameColumn(
                name: "Denied",
                table: "HistoriquePresences",
                newName: "Confirmation");
        }
    }
}

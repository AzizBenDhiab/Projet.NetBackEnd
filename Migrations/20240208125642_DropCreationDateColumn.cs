using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetNET.Migrations
{
    /// <inheritdoc />
    public partial class DropCreationDateColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
     name: "CreationDate",
     table: "Blames");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
        name: "CreationDate",
        table: "Blames",
        nullable: true,
        defaultValueSql: "GETDATE()");
        }
    }
}

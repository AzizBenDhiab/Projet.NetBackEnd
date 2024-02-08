using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetNET.Migrations
{
    /// <inheritdoc />
    public partial class migrationfixuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DletedAt",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DletedAt",
                table: "Users");
        }
    }
}

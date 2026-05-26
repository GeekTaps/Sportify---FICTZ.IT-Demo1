using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sportify.Infraestructura.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsuarioIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Borrado",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Dni",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Edad",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Turnos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    cupo = table.Column<int>(type: "INTEGER", nullable: false),
                    IdDeporte = table.Column<Guid>(type: "TEXT", nullable: false),
                    nombreTurno = table.Column<string>(type: "TEXT", nullable: false),
                    nommbreProfesor = table.Column<string>(type: "TEXT", nullable: false),
                    horaInicio = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    horaFin = table.Column<TimeOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turnos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Turnos");

            migrationBuilder.DropColumn(
                name: "Borrado",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Dni",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Edad",
                table: "AspNetUsers");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sportify.Infraestructura.Migrations
{
    /// <inheritdoc />
    public partial class FechaNacimientoUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Edad",
                table: "AspNetUsers",
                newName: "FechaNacimiento");

            migrationBuilder.AddColumn<bool>(
                name: "ListaEsperaHabilitada",
                table: "Turnos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Precio",
                table: "Turnos",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "CancelacionesMes",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Creditos",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "EsAdmin",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Suspendido",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    idReserva = table.Column<Guid>(type: "TEXT", nullable: false),
                    idUsuario = table.Column<Guid>(type: "TEXT", nullable: false),
                    monto = table.Column<decimal>(type: "TEXT", nullable: false),
                    fecha = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    idUsuario = table.Column<Guid>(type: "TEXT", nullable: false),
                    idTurno = table.Column<Guid>(type: "TEXT", nullable: false),
                    fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    paga = table.Column<bool>(type: "INTEGER", nullable: false),
                    monto = table.Column<double>(type: "REAL", nullable: false),
                    titulo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropColumn(
                name: "ListaEsperaHabilitada",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "Precio",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "CancelacionesMes",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Creditos",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EsAdmin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Suspendido",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "FechaNacimiento",
                table: "AspNetUsers",
                newName: "Edad");
        }
    }
}

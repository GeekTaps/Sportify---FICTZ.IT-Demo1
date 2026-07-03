using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sportify.Infraestructura.Migrations
{
    /// <inheritdoc />
    public partial class AddListaDeEspera : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ListaDeEsperaAbono",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    idUsuario = table.Column<Guid>(type: "TEXT", nullable: false),
                    idDeporte = table.Column<Guid>(type: "TEXT", nullable: false),
                    fecha = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListaDeEsperaAbono", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ListaDeEsperaTurno",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    idUsuario = table.Column<Guid>(type: "TEXT", nullable: false),
                    idTurno = table.Column<Guid>(type: "TEXT", nullable: false),
                    fecha = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListaDeEsperaTurno", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListaDeEsperaAbono");

            migrationBuilder.DropTable(
                name: "ListaDeEsperaTurno");
        }
    }
}

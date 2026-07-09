using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sportify.Infraestructura.Migrations
{
    /// <inheritdoc />
    public partial class AddHorarioYGeneracionMensual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IdHorario",
                table: "Turnos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Horarios",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    idDeporte = table.Column<Guid>(type: "TEXT", nullable: false),
                    diaSemana = table.Column<string>(type: "TEXT", nullable: false),
                    hora = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    eliminado = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horarios", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Horarios");

            migrationBuilder.DropColumn(
                name: "IdHorario",
                table: "Turnos");
        }
    }
}

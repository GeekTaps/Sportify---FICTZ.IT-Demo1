using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sportify.Infraestructura.Migrations
{
    /// <inheritdoc />
    public partial class RenameIdDeporteToIdHorarioListaEsperaAbono : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "idDeporte",
                table: "ListaDeEsperaAbono",
                newName: "idHorario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "idHorario",
                table: "ListaDeEsperaAbono",
                newName: "idDeporte");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sportify.Infraestructura.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNotificacionesListaEspera : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "IdHorario",
                table: "Turnos",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNotificacion",
                table: "ListaDeEsperaTurno",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Notificado",
                table: "ListaDeEsperaTurno",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNotificacion",
                table: "ListaDeEsperaAbono",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Notificado",
                table: "ListaDeEsperaAbono",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "idDeporte",
                table: "ListaDeEsperaAbono",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaNotificacion",
                table: "ListaDeEsperaTurno");

            migrationBuilder.DropColumn(
                name: "Notificado",
                table: "ListaDeEsperaTurno");

            migrationBuilder.DropColumn(
                name: "FechaNotificacion",
                table: "ListaDeEsperaAbono");

            migrationBuilder.DropColumn(
                name: "Notificado",
                table: "ListaDeEsperaAbono");

            migrationBuilder.DropColumn(
                name: "idDeporte",
                table: "ListaDeEsperaAbono");

            migrationBuilder.AlterColumn<Guid>(
                name: "IdHorario",
                table: "Turnos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");
        }
    }
}

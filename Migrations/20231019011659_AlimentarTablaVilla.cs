using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle..", new DateTime(2023, 10, 18, 22, 16, 59, 74, DateTimeKind.Local).AddTicks(6553), new DateTime(2023, 10, 18, 22, 16, 59, 74, DateTimeKind.Local).AddTicks(6511), "", 50, "Villa Real", 5, 200.0 },
                    { 2, "", "Detalle..", new DateTime(2023, 10, 18, 22, 16, 59, 74, DateTimeKind.Local).AddTicks(6557), new DateTime(2023, 10, 18, 22, 16, 59, 74, DateTimeKind.Local).AddTicks(6556), "", 50, "Premium Vista a la Pisina", 4, 500.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}

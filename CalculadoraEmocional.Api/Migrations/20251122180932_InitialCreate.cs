using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalculadoraEmocional.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_GS_CHECKIN",
                columns: table => new
                {
                    id_checkin = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<long>(type: "bigint", nullable: false),
                    dt_checkin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    vl_humor = table.Column<int>(type: "int", nullable: false),
                    vl_foco = table.Column<int>(type: "int", nullable: false),
                    minutos_pausas = table.Column<int>(type: "int", nullable: true),
                    horas_trabalhadas = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ds_observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tags = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    origem = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_GS_CHECKIN", x => x.id_checkin);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_GS_CHECKIN");
        }
    }
}

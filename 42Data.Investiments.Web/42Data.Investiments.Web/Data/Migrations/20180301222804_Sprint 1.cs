using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace _42Data.Investiments.Web.Data.Migrations
{
    public partial class Sprint1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorEntrada",
                table: "WalletCliente");

            migrationBuilder.DropColumn(
                name: "ValorRentabilidade",
                table: "WalletCliente");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorInicial",
                table: "WalletCliente",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorFinal",
                table: "WalletCliente",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorComissaoTotal",
                table: "WalletCliente",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataContratacao",
                table: "WalletCliente",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PlanoContratado",
                table: "WalletCliente",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorEntradaBold",
                table: "WalletCliente",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorEntradaWise",
                table: "WalletCliente",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorRentabilidadeBold",
                table: "WalletCliente",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorRentabilidadeWise",
                table: "WalletCliente",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataContratacao",
                table: "WalletCliente");

            migrationBuilder.DropColumn(
                name: "PlanoContratado",
                table: "WalletCliente");

            migrationBuilder.DropColumn(
                name: "ValorEntradaBold",
                table: "WalletCliente");

            migrationBuilder.DropColumn(
                name: "ValorEntradaWise",
                table: "WalletCliente");

            migrationBuilder.DropColumn(
                name: "ValorRentabilidadeBold",
                table: "WalletCliente");

            migrationBuilder.DropColumn(
                name: "ValorRentabilidadeWise",
                table: "WalletCliente");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorInicial",
                table: "WalletCliente",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorFinal",
                table: "WalletCliente",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorComissaoTotal",
                table: "WalletCliente",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorEntrada",
                table: "WalletCliente",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorRentabilidade",
                table: "WalletCliente",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

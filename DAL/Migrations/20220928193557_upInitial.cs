using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class upInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ValorVenda = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorCompra = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendedores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendedores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vendas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdStatus = table.Column<int>(type: "int", nullable: false),
                    DtVenda = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    IdVendedor = table.Column<int>(type: "int", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vendas_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Vendas_TbStatus_IdStatus",
                        column: x => x.IdStatus,
                        principalTable: "TbStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Vendas_Vendedores_IdVendedor",
                        column: x => x.IdVendedor,
                        principalTable: "Vendedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "VendasHistoricos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdVenda = table.Column<int>(type: "int", nullable: false),
                    IdStatus = table.Column<int>(type: "int", nullable: false),
                    DtInsert = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    Historico = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendasHistoricos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendasHistoricos_TbStatus_IdStatus",
                        column: x => x.IdStatus,
                        principalTable: "TbStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_VendasHistoricos_Vendas_IdVenda",
                        column: x => x.IdVenda,
                        principalTable: "Vendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendasItens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdVenda = table.Column<int>(type: "int", nullable: false),
                    IdProduto = table.Column<int>(type: "int", nullable: false),
                    Qtde = table.Column<int>(type: "int", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendasItens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendasItens_Produtos_IdProduto",
                        column: x => x.IdProduto,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_VendasItens_Vendas_IdVenda",
                        column: x => x.IdVenda,
                        principalTable: "Vendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_IdCliente",
                table: "Vendas",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_IdStatus",
                table: "Vendas",
                column: "IdStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_IdVendedor",
                table: "Vendas",
                column: "IdVendedor");

            migrationBuilder.CreateIndex(
                name: "IX_VendasHistoricos_IdStatus",
                table: "VendasHistoricos",
                column: "IdStatus");

            migrationBuilder.CreateIndex(
                name: "IX_VendasHistoricos_IdVenda",
                table: "VendasHistoricos",
                column: "IdVenda");

            migrationBuilder.CreateIndex(
                name: "IX_VendasItens_IdProduto",
                table: "VendasItens",
                column: "IdProduto");

            migrationBuilder.CreateIndex(
                name: "IX_VendasItens_IdVenda",
                table: "VendasItens",
                column: "IdVenda");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendasHistoricos");

            migrationBuilder.DropTable(
                name: "VendasItens");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Vendas");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "TbStatus");

            migrationBuilder.DropTable(
                name: "Vendedores");
        }
    }
}

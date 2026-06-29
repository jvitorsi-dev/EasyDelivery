using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyDelivery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entregas_Pedidos_PedidoId",
                table: "Entregas");

            migrationBuilder.DropIndex(
                name: "IX_Entregas_PedidoId",
                table: "Entregas");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "Restaurantes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Nota",
                table: "Restaurantes",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "Pedidos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Preco",
                table: "ItensRestaurante",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "ItensRestaurante",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "ItensRestaurante",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "ItensRestaurante",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Entregas",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "CategoriasItensRestaurante",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasItensRestaurante", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoriasRestaurantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasRestaurantes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Restaurantes_CategoriaId",
                table: "Restaurantes",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensRestaurante_CategoriaId",
                table: "ItensRestaurante",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensRestaurante_CategoriasItensRestaurante_CategoriaId",
                table: "ItensRestaurante",
                column: "CategoriaId",
                principalTable: "CategoriasItensRestaurante",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurantes_CategoriasRestaurantes_CategoriaId",
                table: "Restaurantes",
                column: "CategoriaId",
                principalTable: "CategoriasRestaurantes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensRestaurante_CategoriasItensRestaurante_CategoriaId",
                table: "ItensRestaurante");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurantes_CategoriasRestaurantes_CategoriaId",
                table: "Restaurantes");

            migrationBuilder.DropTable(
                name: "CategoriasItensRestaurante");

            migrationBuilder.DropTable(
                name: "CategoriasRestaurantes");

            migrationBuilder.DropIndex(
                name: "IX_Restaurantes_CategoriaId",
                table: "Restaurantes");

            migrationBuilder.DropIndex(
                name: "IX_ItensRestaurante_CategoriaId",
                table: "ItensRestaurante");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "Restaurantes");

            migrationBuilder.DropColumn(
                name: "Nota",
                table: "Restaurantes");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "ItensRestaurante");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "ItensRestaurante");

            migrationBuilder.AlterColumn<decimal>(
                name: "Preco",
                table: "ItensRestaurante",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "ItensRestaurante",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Entregas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Entregas_PedidoId",
                table: "Entregas",
                column: "PedidoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Entregas_Pedidos_PedidoId",
                table: "Entregas",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

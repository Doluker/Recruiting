using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Recruiting.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PaymentStatuses_pkey", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PaymentStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Ожидает оплату" },
                    { 2, "Оплачено" },
                    { 3, "Ошибка оплаты" },
                    { 4, "Возврат" }
                });
            migrationBuilder.Sql("""SELECT setval('"PaymentStatuses_Id_seq"', (SELECT MAX("Id") FROM "PaymentStatuses"));""");
            migrationBuilder.Sql("""CREATE UNIQUE INDEX IF NOT EXISTS "UX_PaymentStatuses_Name" ON "PaymentStatuses" (lower("Name"));""");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentStatus",
                table: "Payments",
                column: "PaymentStatusId");

            migrationBuilder.AddForeignKey(
                name: "Payments_PaymentStatusId_fkey",
                table: "Payments",
                column: "PaymentStatusId",
                principalTable: "PaymentStatuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""DROP INDEX IF EXISTS "UX_PaymentStatuses_Name";""");
            migrationBuilder.DropForeignKey(
                name: "Payments_PaymentStatusId_fkey",
                table: "Payments");
            migrationBuilder.DropForeignKey(
                name: "Payments_PaymentStatusId_fkey",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "PaymentStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentStatus",
                table: "Payments");
        }
    }
}

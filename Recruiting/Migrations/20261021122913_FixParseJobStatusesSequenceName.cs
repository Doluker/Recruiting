using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recruiting.Migrations
{
    /// <inheritdoc />
    public partial class FixParseJobStatusesSequenceName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""ALTER SEQUENCE "ParceJobStatuses_Id_seq" RENAME TO "ParseJobStatuses_Id_seq";""");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ParseJobStatuses",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "nextval('\"ParceJobStatuses_Id_seq\"'::regclass)",
                defaultValueSql: "nextval('\"ParseJobStatuses_Id_seq\"'::regclass)",
                oldNullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ParseJobStatuses",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "nextval('\"ParseJobStatuses_Id_seq\"'::regclass)",
                defaultValueSql: "nextval('\"ParceJobStatuses_Id_seq\"'::regclass)");

            migrationBuilder.Sql("""ALTER SEQUENCE "ParseJobStatuses_Id_seq" RENAME TO "ParceJobStatuses_Id_seq";""");
        }   
    }
}

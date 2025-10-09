using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingApplication.Migrations
{
    /// <inheritdoc />
    public partial class MakeBillPayIDIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
    name: "PK_BillPays",
    table: "BillPays");
           
            migrationBuilder.DropColumn(
                name: "BillPayID",
                table: "BillPays");

            migrationBuilder.AddColumn<int>(
                name: "BillPayID",
                table: "BillPays",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BillPays",
                table: "BillPays",
                column: "BillPayID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
            name: "PK_BillPays",
            table: "BillPays");

            migrationBuilder.DropColumn(
            name: "BillPayID",
            table: "BillPays");

            migrationBuilder.AddColumn<int>(
            name: "BillPayID",
            table: "BillPays",
            type: "int",
            nullable: false,
            defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
            name: "PK_BillPays",
            table: "BillPays",
            column: "BillPayID");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfApp1.Migrations
{
    public partial class fix_type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Divisions_Clients_ClientsId",
                table: "Divisions");

            migrationBuilder.DropIndex(
                name: "IX_Divisions_ClientsId",
                table: "Divisions");

            migrationBuilder.DropColumn(
                name: "ClientsId",
                table: "Divisions");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_DivisionId",
                table: "Clients",
                column: "DivisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Divisions_DivisionId",
                table: "Clients",
                column: "DivisionId",
                principalTable: "Divisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Divisions_DivisionId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_DivisionId",
                table: "Clients");

            migrationBuilder.AddColumn<int>(
                name: "ClientsId",
                table: "Divisions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Divisions_ClientsId",
                table: "Divisions",
                column: "ClientsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Divisions_Clients_ClientsId",
                table: "Divisions",
                column: "ClientsId",
                principalTable: "Clients",
                principalColumn: "Id");
        }
    }
}

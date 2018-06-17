using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Maonot_Net.Migrations
{
    public partial class AutoriztionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AutId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Authorizations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AutName = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorizations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_AutId",
                table: "Users",
                column: "AutId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Authorizations_AutId",
                table: "Users",
                column: "AutId",
                principalTable: "Authorizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Authorizations_AutId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Authorizations");

            migrationBuilder.DropIndex(
                name: "IX_Users_AutId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AutId",
                table: "Users");
        }
    }
}

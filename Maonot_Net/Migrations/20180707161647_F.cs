using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Maonot_Net.Migrations
{
    public partial class F : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VisitorsLogId",
                table: "ApprovalKits",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalKits_VisitorsLogId",
                table: "ApprovalKits",
                column: "VisitorsLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalKits_VisitorsLogs_VisitorsLogId",
                table: "ApprovalKits",
                column: "VisitorsLogId",
                principalTable: "VisitorsLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalKits_VisitorsLogs_VisitorsLogId",
                table: "ApprovalKits");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalKits_VisitorsLogId",
                table: "ApprovalKits");

            migrationBuilder.DropColumn(
                name: "VisitorsLogId",
                table: "ApprovalKits");
        }
    }
}

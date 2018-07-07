using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Maonot_Net.Migrations
{
    public partial class FullName2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentFirstName",
                table: "VisitorsLogs");

            migrationBuilder.RenameColumn(
                name: "StudentLasttName",
                table: "VisitorsLogs",
                newName: "StudentFullName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StudentFullName",
                table: "VisitorsLogs",
                newName: "StudentLasttName");

            migrationBuilder.AddColumn<string>(
                name: "StudentFirstName",
                table: "VisitorsLogs",
                nullable: false,
                defaultValue: "");
        }
    }
}

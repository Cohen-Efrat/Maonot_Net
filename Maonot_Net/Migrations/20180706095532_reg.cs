using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Maonot_Net.Migrations
{
    public partial class reg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PartnerFirstName",
                table: "Registrations",
                newName: "ParentFullName2");

            migrationBuilder.RenameColumn(
                name: "ParentLastName",
                table: "Registrations",
                newName: "ParentFullName1");

            migrationBuilder.RenameColumn(
                name: "ParentID",
                table: "Registrations",
                newName: "ParentID2");

            migrationBuilder.RenameColumn(
                name: "ParentAge",
                table: "Registrations",
                newName: "ParentID1");

            migrationBuilder.AddColumn<int>(
                name: "ParentAge1",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentAge2",
                table: "Registrations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentAge1",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "ParentAge2",
                table: "Registrations");

            migrationBuilder.RenameColumn(
                name: "ParentID2",
                table: "Registrations",
                newName: "ParentID");

            migrationBuilder.RenameColumn(
                name: "ParentID1",
                table: "Registrations",
                newName: "ParentAge");

            migrationBuilder.RenameColumn(
                name: "ParentFullName2",
                table: "Registrations",
                newName: "PartnerFirstName");

            migrationBuilder.RenameColumn(
                name: "ParentFullName1",
                table: "Registrations",
                newName: "ParentLastName");
        }
    }
}

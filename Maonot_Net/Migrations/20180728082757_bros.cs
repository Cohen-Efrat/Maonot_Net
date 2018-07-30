using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Maonot_Net.Migrations
{
    public partial class bros : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "test",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "Familym1_Age",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Familym1_name",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Familym2_Age",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Familym2_name",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Familym3_Age",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Familym3_name",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Familym4_Age",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Familym4_name",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Familym5_Age",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Familym5_name",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Familym6_Age",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Familym6_name",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Familym7_Age",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Familym7_name",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Familym8_Age",
                table: "Registrations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Familym8_name",
                table: "Registrations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Familym1_Age",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "Familym1_name",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "Familym2_Age",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "Familym2_name",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "Familym3_Age",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "Familym3_name",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "Familym4_Age",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "Familym4_name",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "Familym5_Age",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "Familym5_name",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "Familym6_Age",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "Familym6_name",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "Familym7_Age",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "Familym7_name",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "Familym8_Age",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "Familym8_name",
                table: "Registrations");

            migrationBuilder.AddColumn<string>(
                name: "test",
                table: "Messages",
                nullable: true);
        }
    }
}

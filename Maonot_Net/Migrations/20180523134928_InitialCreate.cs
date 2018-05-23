using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Maonot_Net.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FaultForms",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Apartment = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    FullName = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    RoomNum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaultForms", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Addressee = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    Subject = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApartmentNum = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Room = table.Column<int>(nullable: true),
                    StundetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "VisitorsLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApartmentNum = table.Column<int>(nullable: false),
                    EnteryDate = table.Column<DateTime>(nullable: false),
                    ExitDate = table.Column<DateTime>(nullable: false),
                    Room = table.Column<int>(nullable: false),
                    Signature = table.Column<bool>(nullable: false),
                    StudentFirstName = table.Column<string>(nullable: false),
                    StudentLasttName = table.Column<string>(nullable: false),
                    VisitorID = table.Column<int>(nullable: false),
                    VistorName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorsLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Adress = table.Column<string>(nullable: false),
                    ApertmantType = table.Column<int>(nullable: false),
                    Bday = table.Column<DateTime>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    FieldOfStudy = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    HealthCondition = table.Column<int>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    ParentAge = table.Column<int>(nullable: false),
                    ParentID = table.Column<int>(nullable: false),
                    ParentLastName = table.Column<string>(nullable: true),
                    PartnerFirstName = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: false),
                    PostalCode = table.Column<int>(nullable: false),
                    Seniority = table.Column<int>(nullable: false),
                    SteadyYear = table.Column<int>(nullable: false),
                    StundetId = table.Column<int>(nullable: false),
                    TypeOfService = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: true),
                    gender = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Registrations_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Warnings",
                columns: table => new
                {
                    WarningId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BlaBla = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    StudentId = table.Column<int>(nullable: false),
                    WarningNumber = table.Column<int>(nullable: false),
                    userID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warnings", x => x.WarningId);
                    table.ForeignKey(
                        name: "FK_Warnings_Users_userID",
                        column: x => x.userID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalKits",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: false),
                    HealthCondition = table.Column<int>(nullable: true),
                    LastName = table.Column<string>(nullable: false),
                    LivingWithReligious = table.Column<int>(nullable: false),
                    LivingWithSmoker = table.Column<int>(nullable: false),
                    PartnerId1 = table.Column<int>(nullable: false),
                    PartnerId2 = table.Column<int>(nullable: false),
                    PartnerId3 = table.Column<int>(nullable: false),
                    PartnerId4 = table.Column<int>(nullable: false),
                    RegID = table.Column<int>(nullable: true),
                    ReligiousType = table.Column<int>(nullable: false),
                    RoomType = table.Column<int>(nullable: false),
                    StundetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalKits", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ApprovalKits_Registrations_RegID",
                        column: x => x.RegID,
                        principalTable: "Registrations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalKits_RegID",
                table: "ApprovalKits",
                column: "RegID");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_UserID",
                table: "Registrations",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Warnings_userID",
                table: "Warnings",
                column: "userID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalKits");

            migrationBuilder.DropTable(
                name: "FaultForms");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "VisitorsLogs");

            migrationBuilder.DropTable(
                name: "Warnings");

            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

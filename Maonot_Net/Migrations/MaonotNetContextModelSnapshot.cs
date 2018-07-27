﻿// <auto-generated />
using Maonot_Net.Data;
using Maonot_Net.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Maonot_Net.Migrations
{
    [DbContext(typeof(MaonotNetContext))]
    partial class MaonotNetContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Maonot_Net.Models.ApprovalKit", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<int?>("HealthCondition");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<int?>("LivingWithReligious")
                        .IsRequired();

                    b.Property<int?>("LivingWithSmoker")
                        .IsRequired();

                    b.Property<int?>("PartnerId1");

                    b.Property<int?>("PartnerId2");

                    b.Property<int?>("PartnerId3");

                    b.Property<int?>("PartnerId4");

                    b.Property<int?>("RegID");

                    b.Property<int?>("ReligiousType")
                        .IsRequired();

                    b.Property<int?>("RoomType")
                        .IsRequired();

                    b.Property<int>("StundetId");

                    b.Property<int?>("VisitorsLogId");

                    b.HasKey("ID");

                    b.HasIndex("RegID");

                    b.HasIndex("VisitorsLogId");

                    b.ToTable("ApprovalKits");
                });

            modelBuilder.Entity("Maonot_Net.Models.Authorization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AutName");

                    b.HasKey("Id");

                    b.ToTable("Authorizations");
                });

            modelBuilder.Entity("Maonot_Net.Models.FamilyM", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Age");

                    b.Property<string>("FullName");

                    b.Property<int?>("RegistrationID");

                    b.Property<int>("StudentID");

                    b.HasKey("ID");

                    b.HasIndex("RegistrationID");

                    b.ToTable("FamilyM");
                });

            modelBuilder.Entity("Maonot_Net.Models.FaultForm", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Apartment");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("FullName")
                        .IsRequired();

                    b.Property<string>("PhoneNumber");

                    b.Property<int>("RoomNum");

                    b.HasKey("ID");

                    b.ToTable("FaultForms");
                });

            modelBuilder.Entity("Maonot_Net.Models.Message", b =>
                {
                    b.Property<int>("MessageID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Addressee")
                        .IsRequired();

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<string>("Subject")
                        .IsRequired();

                    b.Property<string>("test");

                    b.HasKey("MessageID");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Maonot_Net.Models.Registration", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Adress")
                        .IsRequired();

                    b.Property<int?>("ApertmantType")
                        .IsRequired();

                    b.Property<bool>("Approved");

                    b.Property<DateTime>("Bday");

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<int?>("FieldOfStudy")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<int?>("HealthCondition")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<int?>("ParentAge1");

                    b.Property<int?>("ParentAge2");

                    b.Property<string>("ParentFullName1");

                    b.Property<string>("ParentFullName2");

                    b.Property<int?>("ParentID1");

                    b.Property<int?>("ParentID2");

                    b.Property<string>("PhoneNumber")
                        .IsRequired();

                    b.Property<int?>("PostalCode");

                    b.Property<int?>("Seniority")
                        .IsRequired();

                    b.Property<int?>("SteadyYear")
                        .IsRequired();

                    b.Property<int>("StundetId");

                    b.Property<int?>("Total");

                    b.Property<int?>("TypeOfService")
                        .IsRequired();

                    b.Property<int?>("UserID");

                    b.Property<int?>("gender")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Registrations");
                });

            modelBuilder.Entity("Maonot_Net.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ApartmentNum");

                    b.Property<int?>("AutId");

                    b.Property<int>("Authorization");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<int?>("Room");

                    b.Property<int>("StundetId");

                    b.HasKey("ID");

                    b.HasIndex("AutId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Maonot_Net.Models.VisitorsLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ApartmentNum");

                    b.Property<DateTime>("EnteryDate");

                    b.Property<DateTime?>("ExitDate");

                    b.Property<int?>("Room")
                        .IsRequired();

                    b.Property<bool>("Signature");

                    b.Property<string>("StudentFullName")
                        .IsRequired();

                    b.Property<int>("VisitorID");

                    b.Property<string>("VistorName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("VisitorsLogs");
                });

            modelBuilder.Entity("Maonot_Net.Models.Warning", b =>
                {
                    b.Property<int>("WarningId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BlaBla");

                    b.Property<DateTime>("Date");

                    b.Property<int>("StudentId");

                    b.Property<int?>("WarningNumber")
                        .IsRequired();

                    b.Property<int?>("userID");

                    b.HasKey("WarningId");

                    b.HasIndex("userID");

                    b.ToTable("Warnings");
                });

            modelBuilder.Entity("Maonot_Net.Models.ApprovalKit", b =>
                {
                    b.HasOne("Maonot_Net.Models.Registration", "Reg")
                        .WithMany()
                        .HasForeignKey("RegID");

                    b.HasOne("Maonot_Net.Models.VisitorsLog")
                        .WithMany("App")
                        .HasForeignKey("VisitorsLogId");
                });

            modelBuilder.Entity("Maonot_Net.Models.FamilyM", b =>
                {
                    b.HasOne("Maonot_Net.Models.Registration")
                        .WithMany("Family")
                        .HasForeignKey("RegistrationID");
                });

            modelBuilder.Entity("Maonot_Net.Models.Registration", b =>
                {
                    b.HasOne("Maonot_Net.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("Maonot_Net.Models.User", b =>
                {
                    b.HasOne("Maonot_Net.Models.Authorization", "Aut")
                        .WithMany("users")
                        .HasForeignKey("AutId");
                });

            modelBuilder.Entity("Maonot_Net.Models.Warning", b =>
                {
                    b.HasOne("Maonot_Net.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("userID");
                });
#pragma warning restore 612, 618
        }
    }
}

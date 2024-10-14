﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ST10298850_PROG6212_POE.Data;

#nullable disable

namespace ST10298850_PROG6212_POE.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241013095825_mssql.local_migration_437")]
    partial class mssqllocal_migration_437
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ST10298850_PROG6212_POE.Models.AcademicManagerModel", b =>
                {
                    b.Property<int>("ManagerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ManagerId"));

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ManagerId");

                    b.ToTable("AcademicManagers");
                });

            modelBuilder.Entity("ST10298850_PROG6212_POE.Models.ApprovalModel", b =>
                {
                    b.Property<int>("ApprovalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ApprovalId"));

                    b.Property<DateTime>("ApprovalDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ClaimId")
                        .HasColumnType("int");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.HasKey("ApprovalId");

                    b.HasIndex("ClaimId")
                        .IsUnique();

                    b.ToTable("Approvals");
                });

            modelBuilder.Entity("ST10298850_PROG6212_POE.Models.CoordinatorModel", b =>
                {
                    b.Property<int>("CoordinatorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CoordinatorId"));

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CoordinatorId");

                    b.ToTable("Coordinators");
                });

            modelBuilder.Entity("ST10298850_PROG6212_POE.Models.DocumentModel", b =>
                {
                    b.Property<int>("DocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocumentId"));

                    b.Property<int>("ClaimId")
                        .HasColumnType("int");

                    b.Property<string>("DocumentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DocumentId");

                    b.HasIndex("ClaimId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("ST10298850_PROG6212_POE.Models.LecturerClaimModel", b =>
                {
                    b.Property<int>("ClaimId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClaimId"));

                    b.Property<decimal>("HoursWorked")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("LecturerId")
                        .HasColumnType("int");

                    b.Property<decimal>("OvertimeWorked")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<DateTime>("SubmissionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ClaimId");

                    b.HasIndex("LecturerId");

                    b.ToTable("Claims");
                });

            modelBuilder.Entity("ST10298850_PROG6212_POE.Models.LecturerModel", b =>
                {
                    b.Property<int>("LecturerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LecturerId"));

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LecturerId");

                    b.ToTable("Lecturers");
                });

            modelBuilder.Entity("ST10298850_PROG6212_POE.Models.ApprovalModel", b =>
                {
                    b.HasOne("ST10298850_PROG6212_POE.Models.LecturerClaimModel", "Claim")
                        .WithOne("Approval")
                        .HasForeignKey("ST10298850_PROG6212_POE.Models.ApprovalModel", "ClaimId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Claim");
                });

            modelBuilder.Entity("ST10298850_PROG6212_POE.Models.DocumentModel", b =>
                {
                    b.HasOne("ST10298850_PROG6212_POE.Models.LecturerClaimModel", "Claim")
                        .WithMany("Documents")
                        .HasForeignKey("ClaimId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Claim");
                });

            modelBuilder.Entity("ST10298850_PROG6212_POE.Models.LecturerClaimModel", b =>
                {
                    b.HasOne("ST10298850_PROG6212_POE.Models.LecturerModel", "Lecturer")
                        .WithMany("Claims")
                        .HasForeignKey("LecturerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lecturer");
                });

            modelBuilder.Entity("ST10298850_PROG6212_POE.Models.LecturerClaimModel", b =>
                {
                    b.Navigation("Approval")
                        .IsRequired();

                    b.Navigation("Documents");
                });

            modelBuilder.Entity("ST10298850_PROG6212_POE.Models.LecturerModel", b =>
                {
                    b.Navigation("Claims");
                });
#pragma warning restore 612, 618
        }
    }
}

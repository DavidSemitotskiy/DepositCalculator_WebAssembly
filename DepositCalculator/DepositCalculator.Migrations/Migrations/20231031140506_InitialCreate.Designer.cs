﻿// <auto-generated />
using System;
using DepositCalculator.DAL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DepositCalculator.Migrations.Migrations
{
    [DbContext(typeof(DepositCalculatorContext))]
    [Migration("20231031140506_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DepositCalculator.Entities.DepositCalculationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CalculatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("DepositAmount")
                        .HasPrecision(9, 2)
                        .HasColumnType("decimal(9,2)");

                    b.Property<decimal>("Percent")
                        .HasPrecision(8, 5)
                        .HasColumnType("decimal(8,5)");

                    b.Property<int>("PeriodInMonths")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DepositCalculations", (string)null);
                });

            modelBuilder.Entity("DepositCalculator.Entities.MonthlyDepositCalculationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("DepositByMonth")
                        .HasPrecision(17, 9)
                        .HasColumnType("decimal(17,9)");

                    b.Property<Guid>("DepositCalculationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("MonthNumber")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalDepositAmount")
                        .HasPrecision(17, 9)
                        .HasColumnType("decimal(17,9)");

                    b.HasKey("Id");

                    b.HasIndex("DepositCalculationId");

                    b.ToTable("MonthlyDepositCalculations", (string)null);
                });

            modelBuilder.Entity("DepositCalculator.Entities.MonthlyDepositCalculationEntity", b =>
                {
                    b.HasOne("DepositCalculator.Entities.DepositCalculationEntity", null)
                        .WithMany("MonthlyCalculations")
                        .HasForeignKey("DepositCalculationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DepositCalculator.Entities.DepositCalculationEntity", b =>
                {
                    b.Navigation("MonthlyCalculations");
                });
#pragma warning restore 612, 618
        }
    }
}

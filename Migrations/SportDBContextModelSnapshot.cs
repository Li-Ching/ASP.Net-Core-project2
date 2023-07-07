﻿// <auto-generated />
using System;
using CCP_HW2_Project_A9210256.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CCP_HW2_Project_A9210256.Migrations
{
    [DbContext(typeof(SportDBContext))]
    partial class SportDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CCP_HW2_Project_A9210256.Models.SportRecord", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int?>("heartbeat")
                        .HasColumnType("int");

                    b.Property<string>("remark")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("sportDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("sportType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("time")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("SportRecords");
                });

            modelBuilder.Entity("CCP_HW2_Project_A9210256.Models.SportType", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("sportType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("SportTypes");
                });
#pragma warning restore 612, 618
        }
    }
}
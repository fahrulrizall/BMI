﻿// <auto-generated />
using System;
using BMI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BMI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20201013061046_addshipmenttable")]
    partial class addshipmenttable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BMI.Models.Fgmodel", b =>
                {
                    b.Property<int>("id_fg")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("plant")
                        .HasColumnType("int");

                    b.Property<float>("price_lbs")
                        .HasColumnType("real");

                    b.Property<float>("processing_fee")
                        .HasColumnType("real");

                    b.Property<string>("sap_code")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("std_price")
                        .HasColumnType("real");

                    b.Property<DateTime>("updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("id_fg");

                    b.HasIndex("sap_code");

                    b.ToTable("Fg");
                });

            modelBuilder.Entity("BMI.Models.Masterdatamodel", b =>
                {
                    b.Property<string>("sap_code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("lbs")
                        .HasColumnType("real");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("sap_code");

                    b.ToTable("Master_data");
                });

            modelBuilder.Entity("BMI.Models.Rmmodel", b =>
                {
                    b.Property<int>("id_raw")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("cases")
                        .HasColumnType("int");

                    b.Property<string>("container")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("eta")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("etd")
                        .HasColumnType("datetime2");

                    b.Property<float?>("ex_rate")
                        .HasColumnType("real");

                    b.Property<string>("landing_site")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("qty_pl")
                        .HasColumnType("real");

                    b.Property<float?>("qty_received")
                        .HasColumnType("real");

                    b.Property<string>("reff")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sap_code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("saved")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("uom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.Property<float?>("usd_price")
                        .HasColumnType("real");

                    b.HasKey("id_raw");

                    b.HasIndex("sap_code");

                    b.ToTable("Rm");
                });

            modelBuilder.Entity("BMI.Models.Shipmentmodel", b =>
                {
                    b.Property<int>("id_shipment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("destination")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("eta")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("etd")
                        .HasColumnType("datetime2");

                    b.Property<int>("po")
                        .HasColumnType("int");

                    b.Property<string>("saved")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("id_shipment");

                    b.ToTable("Shipment");
                });

            modelBuilder.Entity("BMI.Models.Usermodel", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConfirmEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConfirmPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("BMI.Models.Fgmodel", b =>
                {
                    b.HasOne("BMI.Models.Masterdatamodel", "Masterdatamodel")
                        .WithMany("Fgmodels")
                        .HasForeignKey("sap_code")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BMI.Models.Rmmodel", b =>
                {
                    b.HasOne("BMI.Models.Masterdatamodel", "Masterdatamodel")
                        .WithMany("Rmmodels")
                        .HasForeignKey("sap_code");
                });
#pragma warning restore 612, 618
        }
    }
}

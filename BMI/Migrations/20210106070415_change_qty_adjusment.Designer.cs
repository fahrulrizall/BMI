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
    [Migration("20210106070415_change_qty_adjusment")]
    partial class change_qty_adjusment
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BMI.Models.AdjustmentFGModel", b =>
                {
                    b.Property<int>("id_adjustmentFG")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("bmi_code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("created_by")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("landing_site")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("po")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("production_date")
                        .HasColumnType("datetime2");

                    b.Property<double>("qty")
                        .HasColumnType("float");

                    b.Property<string>("raw_source")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("updated_by")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id_adjustmentFG");

                    b.HasIndex("bmi_code");

                    b.HasIndex("po");

                    b.ToTable("AdjustmentFG");
                });

            modelBuilder.Entity("BMI.Models.AdjustmentRawModel", b =>
                {
                    b.Property<int>("id_adjustmentRaw")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("created_by")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("qty")
                        .HasColumnType("float");

                    b.Property<string>("raw_source")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sap_code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("updated_by")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id_adjustmentRaw");

                    b.HasIndex("sap_code");

                    b.ToTable("AdjustmentRaw");
                });

            modelBuilder.Entity("BMI.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("BMI.Models.Fgmodel", b =>
                {
                    b.Property<int>("id_fg")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("created_by")
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<string>("updated_by")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id_fg");

                    b.HasIndex("sap_code");

                    b.ToTable("Fg");
                });

            modelBuilder.Entity("BMI.Models.MasterBMIModel", b =>
                {
                    b.Property<string>("bmi_code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("created_by")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("daily_category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("index")
                        .HasColumnType("real");

                    b.Property<string>("index_category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("index_cs")
                        .HasColumnType("real");

                    b.Property<float?>("index_lb")
                        .HasColumnType("real");

                    b.Property<float>("lbs")
                        .HasColumnType("real");

                    b.Property<string>("sap_code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("updated_by")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("weekly_category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("zafc_cs")
                        .HasColumnType("real");

                    b.Property<float?>("zafc_kg")
                        .HasColumnType("real");

                    b.HasKey("bmi_code");

                    b.ToTable("Master_BMI");
                });

            modelBuilder.Entity("BMI.Models.Masterdatamodel", b =>
                {
                    b.Property<string>("sap_code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("created_by")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("lbs")
                        .HasColumnType("real");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("updated_by")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("sap_code");

                    b.ToTable("Master_data");
                });

            modelBuilder.Entity("BMI.Models.POModel", b =>
                {
                    b.Property<string>("po")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("batch")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("container")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("created_by")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("destination")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("document_date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("eta")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("etd")
                        .HasColumnType("datetime2");

                    b.Property<string>("fda_no")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("house_bol")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("inv_no")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("master_bol")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ocean_carrier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("plant")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("po_status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("port_loading")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("port_receipt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("pt")
                        .HasColumnType("int");

                    b.Property<string>("pt_status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("saved")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("seal_no")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("shipment_no")
                        .HasColumnType("int");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("updated_by")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("vessel_name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("voyage_no")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("po");

                    b.ToTable("PO");
                });

            modelBuilder.Entity("BMI.Models.PendingModel", b =>
                {
                    b.Property<int>("id_pending")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("created_by")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("qty")
                        .HasColumnType("real");

                    b.Property<string>("raw_source")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("updated_by")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id_pending");

                    b.HasIndex("raw_source");

                    b.ToTable("Pending");
                });

            modelBuilder.Entity("BMI.Models.ProductionInputModel", b =>
                {
                    b.Property<int>("id_productioninput")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("created_by")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("gi_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("landing_site")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("po")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("po_bmi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("qty")
                        .HasColumnType("real");

                    b.Property<string>("raw_source")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sap_code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("updated_by")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id_productioninput");

                    b.HasIndex("po");

                    b.HasIndex("sap_code");

                    b.ToTable("Production_input");
                });

            modelBuilder.Entity("BMI.Models.ProductionOutputModel", b =>
                {
                    b.Property<int>("id_productionoutput")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("bmi_code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("created_by")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("gr_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("landing_site")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("po")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("po_bmi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("qty")
                        .HasColumnType("real");

                    b.Property<string>("raw_source")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("updated_by")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id_productionoutput");

                    b.HasIndex("bmi_code");

                    b.HasIndex("po");

                    b.ToTable("Production_output");
                });

            modelBuilder.Entity("BMI.Models.RegisterView", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConfirmPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("BMI.Models.RepackModel", b =>
                {
                    b.Property<int>("id_repack")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("created_by")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<string>("from_bmi_code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("from_po")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("landing_site")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("po")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("production_date")
                        .HasColumnType("datetime2");

                    b.Property<double>("qty")
                        .HasColumnType("float");

                    b.Property<string>("raw_source")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("to_bmi_code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("to_po")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("updated_by")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id_repack");

                    b.HasIndex("from_bmi_code");

                    b.HasIndex("from_po");

                    b.HasIndex("to_bmi_code");

                    b.HasIndex("to_po");

                    b.ToTable("Repack");
                });

            modelBuilder.Entity("BMI.Models.RmDetailModel", b =>
                {
                    b.Property<int>("id_raw")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CS_location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("cases")
                        .HasColumnType("int");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("created_by")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("ex_rate")
                        .HasColumnType("real");

                    b.Property<string>("landing_site")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("qty_pl")
                        .HasColumnType("real");

                    b.Property<float?>("qty_received")
                        .HasColumnType("real");

                    b.Property<string>("raw_source")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("sap_code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("saved")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("uom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("updated_by")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("usd_price")
                        .HasColumnType("real");

                    b.HasKey("id_raw");

                    b.HasIndex("raw_source");

                    b.HasIndex("sap_code");

                    b.ToTable("Rm_detail");
                });

            modelBuilder.Entity("BMI.Models.RmModel", b =>
                {
                    b.Property<string>("raw_source")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("container")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("created_by")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("eta")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("etd")
                        .HasColumnType("datetime2");

                    b.Property<string>("saved")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("updated_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("updated_by")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("raw_source");

                    b.ToTable("Rm");
                });

            modelBuilder.Entity("BMI.Models.ShipmentModel", b =>
                {
                    b.Property<string>("id_shipment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("batch")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("bmi_code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("created_by")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("index")
                        .HasColumnType("real");

                    b.Property<DateTime>("pdc")
                        .HasColumnType("datetime2");

                    b.Property<string>("po")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("qty")
                        .HasColumnType("int");

                    b.Property<string>("raw_source")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("updated_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("updated_by")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id_shipment");

                    b.HasIndex("batch");

                    b.HasIndex("bmi_code");

                    b.HasIndex("po");

                    b.ToTable("Shipment");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("BMI.Models.AdjustmentFGModel", b =>
                {
                    b.HasOne("BMI.Models.MasterBMIModel", "MasterBMIModel")
                        .WithMany()
                        .HasForeignKey("bmi_code");

                    b.HasOne("BMI.Models.POModel", "POModel")
                        .WithMany()
                        .HasForeignKey("po");
                });

            modelBuilder.Entity("BMI.Models.AdjustmentRawModel", b =>
                {
                    b.HasOne("BMI.Models.Masterdatamodel", "Masterdatamodel")
                        .WithMany()
                        .HasForeignKey("sap_code");
                });

            modelBuilder.Entity("BMI.Models.Fgmodel", b =>
                {
                    b.HasOne("BMI.Models.Masterdatamodel", "Masterdatamodel")
                        .WithMany()
                        .HasForeignKey("sap_code")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BMI.Models.PendingModel", b =>
                {
                    b.HasOne("BMI.Models.RmModel", "RmModel")
                        .WithMany()
                        .HasForeignKey("raw_source");
                });

            modelBuilder.Entity("BMI.Models.ProductionInputModel", b =>
                {
                    b.HasOne("BMI.Models.POModel", "POModel")
                        .WithMany()
                        .HasForeignKey("po");

                    b.HasOne("BMI.Models.Masterdatamodel", "Masterdatamodel")
                        .WithMany()
                        .HasForeignKey("sap_code");
                });

            modelBuilder.Entity("BMI.Models.ProductionOutputModel", b =>
                {
                    b.HasOne("BMI.Models.MasterBMIModel", "MasterBMIModel")
                        .WithMany()
                        .HasForeignKey("bmi_code");

                    b.HasOne("BMI.Models.POModel", "POModel")
                        .WithMany()
                        .HasForeignKey("po");
                });

            modelBuilder.Entity("BMI.Models.RepackModel", b =>
                {
                    b.HasOne("BMI.Models.MasterBMIModel", "fromMasterBMIModel")
                        .WithMany()
                        .HasForeignKey("from_bmi_code");

                    b.HasOne("BMI.Models.POModel", "fromPOModel")
                        .WithMany()
                        .HasForeignKey("from_po");

                    b.HasOne("BMI.Models.MasterBMIModel", "toMasterBMIModel")
                        .WithMany()
                        .HasForeignKey("to_bmi_code");

                    b.HasOne("BMI.Models.POModel", "toPOModel")
                        .WithMany()
                        .HasForeignKey("to_po");
                });

            modelBuilder.Entity("BMI.Models.RmDetailModel", b =>
                {
                    b.HasOne("BMI.Models.RmModel", "RmModel")
                        .WithMany()
                        .HasForeignKey("raw_source");

                    b.HasOne("BMI.Models.Masterdatamodel", "Masterdatamodel")
                        .WithMany()
                        .HasForeignKey("sap_code");
                });

            modelBuilder.Entity("BMI.Models.ShipmentModel", b =>
                {
                    b.HasOne("BMI.Models.POModel", "POModelBatch")
                        .WithMany()
                        .HasForeignKey("batch");

                    b.HasOne("BMI.Models.MasterBMIModel", "MasterBMIModel")
                        .WithMany()
                        .HasForeignKey("bmi_code");

                    b.HasOne("BMI.Models.POModel", "POModel")
                        .WithMany()
                        .HasForeignKey("po");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BMI.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BMI.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BMI.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BMI.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

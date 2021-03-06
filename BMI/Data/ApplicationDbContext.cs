﻿using BMI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BMI.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Fgmodel> Fg { get; set; }
        public DbSet<RmDetailModel> Rm_detail { get; set; }
        public DbSet<RmModel> Rm { get; set; }
        public DbSet<Masterdatamodel> Master_data { get; set; }
        public DbSet<MasterBMIModel> Master_BMI { get; set; }
        public DbSet<ShipmentModel> Shipment { get; set; }
        public DbSet<ProductionInputModel> Production_input { get; set; }
        public DbSet<ProductionOutputModel> Production_output { get; set; }
        public DbSet<POModel> PO { get; set; }
        public DbSet<AdjustmentFGModel> AdjustmentFG { get; set; }
        public DbSet<AdjustmentRawModel> AdjustmentRaw { get; set; }
        public DbSet<RepackModel> Repack { get; set; }
        public DbSet<PendingModel> Pending { get; set; }
        public DbSet<FundModel> Fund { get; set; }
        public DbSet<DepositModel> Deposit { get; set; }
        public DbSet<SAP_POModel> SAP_PO { get; set; }
        public DbSet<VendorModel> Vendor { get; set; }
        public DbSet<SAP_PODetailModel> SAP_PO_Detail { get; set; }
        public DbSet<DateVesselModel> Date_vessel { get; set; }
        public DbSet<QtyLine1Input> QtyLine1Input { get; set; }
        public DbSet<QtyLine1Output> QtyLine1Output { get; set; }
        public DbSet<CostAnalystModel> CostAnalyst { get; set; }
        public DbSet<RmCostModel> Rm_Cost { get; set; }
    }
}


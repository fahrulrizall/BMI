using BMI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;



namespace BMI.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Masterdatamodel>()
                .HasMany(b => b.Fgmodels)
                .WithOne(p => p.Masterdatamodel);

            //modelBuilder.Entity<Shipmentmodel>().HasNoKey();
        }
        public DbSet<Usermodel> User { get; set; }
        public DbSet<Fgmodel> Fg { get; set; }
        public DbSet<Rmmodel> Rm { get; set; }
        public DbSet<Masterdatamodel> Master_data { get; set; }
        public DbSet<Shipmentmodel> Shipment { get; set; }
        public DbSet<MasterBMIModel> Master_BMI { get; set; }
        public DbSet<ShipmentDetailModel> Shipment_detail { get; set; }
        public DbSet<ProductionInputModel> Production_input { get; set; }
        public DbSet<ProductionOutputModel> Production_output { get; set; }

    }
}

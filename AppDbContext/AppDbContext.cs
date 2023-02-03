using Entities;
using Entities.Catalogue;
using Entities.Configuration;
using Interface.DbContext;
using Extensions;
using Microsoft.EntityFrameworkCore;

namespace AppDbContext
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<OTPHistories>(x => x.ToTable("OTPHistories"));
            //modelBuilder.Entity<SMSEmailTemplates>(x => x.ToTable("SMSEmailTemplates"));

            #region Configuration
            modelBuilder.Entity<tbl_EmailConfigurations>(x => x.ToTable("tbl_EmailConfigurations"));
            modelBuilder.Entity<tbl_SMSConfigurations>(x => x.ToTable("tbl_SMSConfigurations"));
            #endregion

            //Data seeding (tạo dữ liệu mẫu - ....Extension.ModelBuilderExtensions)
            //modelBuilder.Seed();
            base.OnModelCreating(modelBuilder);
        }

        //public DbSet<Districts> Districts { get; set; }
        //public DbSet<Cities> Cities { get; set; }
        public DbSet<tbl_Users> tbl_Users { get; set; }
        public DbSet<tbl_Role> tbl_Role { get; set; }
        public DbSet<tbl_Menu> tbl_Menu { get; set; }
        public DbSet<tbl_Products> tbl_Products { get; set; }
        public DbSet<tbl_Providers> tbl_Providers { get; set; }
        public DbSet<tbl_Biddings> tbl_Biddings { get; set; }
        public DbSet<tbl_BiddingSessions> tbl_BiddingSessions { get; set; }
        public DbSet<tbl_BiddingTickets> tbl_BiddingTickets { get; set; }
        public DbSet<tbl_Notification> tbl_Notifications { get; set; }
        public DbSet<tbl_TechnicalOptions> tbl_TechnicalOptions { get; set; }
        public DbSet<tbl_TechnicalProduct> tbl_TechnicalProducts { get; set; }
        #region Configuration
        public DbSet<tbl_EmailConfigurations> tbl_EmailConfigurations { get; set; }
        public DbSet<tbl_SMSConfigurations> tbl_SMSConfigurations { get; set; }
        public DbSet<tbl_Cities> tbl_Cities { get; set; }
        public DbSet<tbl_Districts> tbl_Districts { get; set; }
        public DbSet<tbl_Wards> tbl_Wards { get; set; }
        
        #endregion
    }
}
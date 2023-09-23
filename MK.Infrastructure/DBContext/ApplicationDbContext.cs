
using Microsoft.EntityFrameworkCore.Design;
using MK.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Infrastructure.DBContext
{
    public class ApplicationDbContext : DbContext
    {
        protected ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(AppConfig.ConnectionStrings.DefaultConnection)
                            .UseSnakeCaseNamingConvention();
            base.OnConfiguring(optionsBuilder);
        }

        #region DbSet
        public DbSet<Area> Areas { get; set; }
        public DbSet<Conversation> Conversations { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Dish> Dish { get; set; }
        public DbSet<District> Districts { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<FavouriteKitchen> FavouriteKitchens { get; set; }

        public DbSet<Meal> Meals { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderPayment> OrderPayments { get; set; }

        public DbSet<Promotion> Promotions { get; set; }

        public DbSet<Province> Provinces { get; set; }

        public DbSet<Voucher> Vouchers { get; set; }

        public DbSet<Ward> Wards { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<Kitchen> Kitchens { get; set; }


        #endregion DbSet

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (CustomerStatus)Enum.Parse(typeof(CustomerStatus), v)
                );

            modelBuilder.Entity<Dish>()
                .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (DishStatus)Enum.Parse(typeof(DishStatus), v)
                );

            modelBuilder.Entity<Order>()
                .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v)
                );

            modelBuilder.Entity<OrderPayment>()
                .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), v)
                );

            modelBuilder.Entity<Promotion>()
                .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (PromotionStatus)Enum.Parse(typeof(PromotionStatus), v)
                );

            modelBuilder.Entity<Voucher>()
                .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (VoucherStatus)Enum.Parse(typeof(VoucherStatus), v)
                );

            modelBuilder.Entity<Kitchen>()
                .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (KitchenStatus)Enum.Parse(typeof(KitchenStatus), v)
                );

            modelBuilder.Entity<Notification>()
                .Property(e => e.NotificationType)
                .HasConversion(
                    e => e.ToString(),
                    e => (NotificationType)Enum.Parse(typeof(NotificationType), e)
                );
        }



    }
    /* Demo for EntityConfiguration
        public class AccountEntityConfiguration : IEntityTypeConfiguration<Account>
        {
            public void Configure(EntityTypeBuilder<Account> builder)
            {
                builder.ToTable("account");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Id).HasColumnName("id");
                builder.Property(x => x.Username).HasColumnName("username");
                builder.Property(x => x.Password).HasColumnName("password");
                builder.Property(x => x.Email).HasColumnName("email");
                builder.Property(x => x.Phone).HasColumnName("phone");
                builder.Property(x => x.Role).HasColumnName("role");
                builder.Property(x => x.Status).HasColumnName("status");
                builder.Property(x => x.CreatedAt).HasColumnName("created_at");
                builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            }
        }
    */

}


using Microsoft.EntityFrameworkCore.Design;
using MK.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
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

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderPayment> OrderPayments { get; set; }

        public DbSet<PaymentType> PaymentTypes { get; set; }

        public DbSet<Meal> Meals { get; set; }

        public DbSet<Tray> Trays { get; set; }

        public DbSet<Dish> Dish { get; set; }

        public DbSet<Kitchen> Kitchens { get; set; }

        public DbSet<Area> Areas { get; set; }

        public DbSet<Location> Locations { get; set; }
 
        public DbSet<FavouriteKitchen> FavouriteKitchens { get; set; }
        #endregion DbSet

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            #region Config for Relationship
            //config one to many for User and Role
            modelBuilder.Entity<User>()
                .HasOne(x => x.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(x => x.RoleId);

            //config one to one for User and Customer
            modelBuilder.Entity<Customer>()
                .HasOne(x => x.User)
                .WithOne(r => r.Customer)
                .HasForeignKey<Customer>(r => r.UserId);

            //config one to one for User and Kitchen
            modelBuilder.Entity<Kitchen>()
                .HasOne(x => x.Owner)
                .WithOne(r => r.Kitchen)
                .HasForeignKey<Kitchen>(r => r.OwnerId);

            //config one to many for Customer and FavouriteKitchen
            modelBuilder.Entity<Customer>()
                .HasMany(x => x.FavouriteKitchens)
                .WithOne(r => r.Customer)
                .HasForeignKey(x => x.CustomerId)
                .IsRequired();

            //config one to many for Kitchen and FavouriteKitchen
            modelBuilder.Entity<Kitchen>()
                .HasMany(x => x.FavoriteKitchens)
                .WithOne(r => r.Kitchen)
                .HasForeignKey(x => x.KitchenId)
                .IsRequired();

            //config one to one for kithen and location
            modelBuilder.Entity<Kitchen>()
                .HasOne(x => x.Location)
                .WithOne(r => r.Kitchen)
                .HasForeignKey<Kitchen>(r => r.LocationId)
                .IsRequired();

            //config one to many for Area and Kitchen
            modelBuilder.Entity<Kitchen>()
                .HasOne(x => x.Area)
                .WithMany(r => r.Kitchens)
                .HasForeignKey(x => x.AreaId)
                .IsRequired();

            //config one to one for Kitchen and location
            modelBuilder.Entity<Kitchen>()
                .HasOne(x => x.Location)
                .WithOne(r => r.Kitchen)
                .HasForeignKey<Kitchen>(r => r.LocationId)
                .IsRequired();

            //config data type for Boundaries
            modelBuilder.Entity<Area>()
                .Property(x => x.Boundaries)
                .HasColumnType("uuid[]")
                .IsRequired();

            //config one to many for Kitchen and Dish
            modelBuilder.Entity<Kitchen>()
                .HasMany(x => x.Dishes)
                .WithOne(r => r.Kitchen)
                .HasForeignKey(x => x.KitchenId)
                .IsRequired();

            //config one to many for Kitchen and Tray
            modelBuilder.Entity<Kitchen>()
                .HasMany(x => x.Trays)
                .WithOne(r => r.Kitchen)
                .HasForeignKey(x => x.KitchenId)
                .IsRequired();

            //config many to many for Dish and Tray
            modelBuilder.Entity<Dish>()
                .HasMany(x => x.Trays)
                .WithMany(r => r.Dishies)
                .UsingEntity(i => i.ToTable("dish_tray"));

            //config one to many for tray and meal
            modelBuilder.Entity<Tray>()
                .HasMany(x => x.Meals)
                .WithOne(r => r.Tray)
                .HasForeignKey(x => x.TrayId);

            //config one to many for Kitchen and Meal
            modelBuilder.Entity<Kitchen>()
                .HasMany(x => x.Meals)
                .WithOne(r => r.Kitchen)
                .HasForeignKey(x => x.KitchenId);

            //config one to many for Customer and Order
            modelBuilder.Entity<Customer>()
                .HasMany(x => x.Orders)
                .WithOne(r => r.Customer)
                .HasForeignKey(x => x.CustomerId);

            //config one to many for Meal and Order
            modelBuilder.Entity<Meal>()
                .HasMany(x => x.Orders)
                .WithOne(r => r.Meal)
                .HasForeignKey(x => x.MealId);

            //config one to many for Order and OrderPayment
            modelBuilder.Entity<Order>()
                .HasMany(x => x.OrderPayments)
                .WithOne(r => r.Order)
                .HasForeignKey(x => x.OrderId);

            //config one to many for PaymentType and OrderPayment
            modelBuilder.Entity<PaymentType>()
                .HasMany(x => x.OrderPayments)
                .WithOne(r => r.PaymentType)
                .HasForeignKey(x => x.PaymentTypeId);

            //config one to one for Feedback and Order
            modelBuilder.Entity<Order>()
                .HasOne(x => x.Feedback)
                .WithOne(r => r.Order)
                .HasForeignKey<Feedback>(x => x.OrderId)
                .IsRequired();
            //config one to many for Customer and Feedback
            modelBuilder.Entity<Customer>()
                .HasMany(x => x.Feedbacks)
                .WithOne(r => r.Customer)
                .HasForeignKey(x => x.CustomerId)
                .IsRequired();
            modelBuilder.Entity<Kitchen>()
                .HasMany(x => x.Feedbacks)
                .WithOne(r => r.Kitchen)
                .HasForeignKey(x => x.KitchenId)
                .IsRequired();
                

            #endregion Config for Relationship

            #region Config for mapping data type 
            //config for Notification status
            modelBuilder.Entity<Notification>()
               .Property(e => e.NotificationType)
               .HasConversion(
                   e => e.ToString(),
                   e => (NotificationType)Enum.Parse(typeof(NotificationType), e));

            modelBuilder.Entity<OrderPayment>()
              .Property(e => e.Status)
              .HasConversion(
                  v => v.ToString(),
                  v => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), v)
              );

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

            #endregion Config for mapping data type 
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

using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Data.DataAccess
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Area> Area { get; set; }
        public DbSet<Floor> Floor { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<ItemMaterial> ItemMaterial { get; set; }
        public DbSet<ManagerTask> ManagerTask { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<MaterialCategory> MaterialCategory { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Procedure> Procedure { get; set; }
        public DbSet<ProcedureItem> ProcedureItem { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<Resource> Resource { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Squad> Squad { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<WokerTask> WokerTask { get; set; }
        public DbSet<WokerTaskDetail> WokerTaskDetail { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationship between OrdersAssignTo and User
            modelBuilder.Entity<Order>()
                .HasOne(o => o.AssignTo)
                .WithMany(u => u.OrdersAssignTo)
                .HasForeignKey(o => o.assignToId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the relationship between OrdersCreatedBy and User
            modelBuilder.Entity<Order>()
                .HasOne(o => o.CreatedBy)
                .WithMany(u => u.OrdersCreatedBy)
                .HasForeignKey(o => o.createdById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}

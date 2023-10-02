using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Data.DataAccess
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<MaterialCategory> MaterialCategory { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<ItemCategory> ItemCategory { get; set; }
        public DbSet<ItemMaterial> ItemMaterial { get; set; }
        public DbSet<Floor> Floor { get; set; }
        public DbSet<Area> Area { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        //public DbSet<Notification> Notifications { get; set; }
        public DbSet<Entities.Task> Task { get; set; }
        public DbSet<TaskType> TaskType { get; set; }
        public DbSet<Procedure> Procedure { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}

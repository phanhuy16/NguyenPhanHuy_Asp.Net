using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NguyenPhanHuy_2122110062.Models.Context;

namespace NguyenPhanHuy_2122110062.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public DateTime CreatedDate { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Statistical> Statisticals { get; set; }
        public virtual DbSet<Adv> Advs { get; set; }
        public virtual DbSet<Posts> Posts { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<SystemSetting> SystemSettings { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Subscribe> Subscribes { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Wishlist> Wishlists { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
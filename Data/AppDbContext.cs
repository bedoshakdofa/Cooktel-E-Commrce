using Cooktel_E_commrece.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Cooktel_E_commrece.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions opt):base(opt)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cart>()
                .HasOne(x => x.User)
                .WithOne(x=>x.Cart);

            modelBuilder.Entity<CartItems>()
                .HasKey(x => new { x.ProductId,x.CartId });

            modelBuilder.Entity<CartItems>()
                .HasOne(p => p.Product)
                .WithMany(p => p.CartItems)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItems>()
                .HasOne(c => c.cart)
                .WithMany(c => c.cartItems);

            modelBuilder.Entity<OrderItems>()
                .HasKey(Fk => new {Fk.Product_ID,Fk.Order_ID });

            modelBuilder.Entity<OrderItems>()
                .HasOne(p=>p.Product)
                .WithMany(o=>o.OrderItems)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItems>()
                .HasOne(or => or.orders)
                .WithMany(ort => ort.OrderItems);

            modelBuilder.Entity<Orders>()
                .HasOne(py => py.payment)
                .WithOne(o => o.order_payment);

            modelBuilder.Entity<Orders>()
                .HasOne(u => u.User)
                .WithMany(o => o.Orders)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reviews>()
                .HasOne(u=>u.User)
                .WithMany(r=>r.Reviews);

            modelBuilder.Entity<Reviews>()
                .HasOne(p => p.Product)
                .WithMany(r => r.Reviews);

            modelBuilder.Entity<Product>()
                .HasOne(ctg => ctg.subcategory)
                .WithMany(p => p.products)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RefreshToken>()
                .HasOne(x => x.user)
                .WithMany(r=>r.RefreshTokens);
                
            modelBuilder.Entity<Subcategory>()
                .HasOne(cat=>cat.Category)
                .WithMany(sub=>sub.subcategories)
                .OnDelete(DeleteBehavior.Restrict);

        }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Orders> orders { get; set; }
        public DbSet<Reviews> reviews { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<OrderItems> orderItems { get; set; }
        public DbSet<CartItems> cartItems { get; set; }
        public DbSet<Subcategory> subcategories { get; set; }
    }
}

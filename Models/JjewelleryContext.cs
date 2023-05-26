using Microsoft.EntityFrameworkCore;
using OnlineJewelleryShop.Models;

namespace OnlineJewelleryShop.Models
{
    public partial class JjewelleryContext : DbContext
    {
        public JjewelleryContext()
        {
        }

        public JjewelleryContext(DbContextOptions<JjewelleryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<CartItem> CartItems { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductDetail> ProductDetails { get; set; } = null!;
        public virtual DbSet<ProductOrder> ProductOrders { get; set; } = null!;
        public virtual DbSet<ProductType> ProductTypes { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Jjewellery;Integrated Security=SSPI;trustServerCertificate=yes; user id=DESKTOP-OAQ879D\\\\\\\\jdneu;Trusted_Connection=True; MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("cart");

                entity.Property(e => e.CartId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("cart_id");

                entity.Property(e => e.CartOrderid).HasColumnName("cart_orderid");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("total");

                entity.Property(e => e.UserId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("user_id");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("product_id");

                entity.HasOne(e => e.Product)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_cart_productid");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__cart__user_id__48CFD27E");
            });


            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.Property(e => e.CategoryId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("category_id");

                entity.Property(e => e.CategoryDescription).HasColumnName("category_description");

                entity.Property(e => e.CategoryName).HasColumnName("category_name");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("order_id");

                entity.Property(e => e.DeliveredDate)
                    .HasColumnType("datetime")
                    .HasColumnName("delivered_date");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasColumnName("order_date");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("total");

                entity.Property(e => e.UserId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__orders__user_id__45F365D3");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("product_id");

                entity.Property(e => e.CategoryId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("category_id");

                entity.Property(e => e.ImagePath).HasColumnName("image_path");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("price");

                entity.Property(e => e.ProdTypeId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("prod_type_id");

                entity.Property(e => e.ProductDescription).HasColumnName("product_description");

                entity.Property(e => e.ProductName).HasColumnName("product_name");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__products__catego__3E52440B")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.ProdType)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProdTypeId)
                    .HasConstraintName("FK__products__prod_t__3F466844")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProductDetail>(entity =>
            {
                entity.HasKey(e => e.ProductDetailsId)
                    .HasName("PK__product___A0C27C440881675D");

                entity.ToTable("product_details");

                entity.Property(e => e.ProductDetailsId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("product_details_id");

                entity.Property(e => e.AvailQty).HasColumnName("avail_qty");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Dimension).HasColumnName("dimension");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("price");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("product_id");

                entity.Property(e => e.StatusId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("status_id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__product_d__produ__4222D4EF");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ProductDetails)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK__product_d__statu__4316F928");
            });

            modelBuilder.Entity<ProductOrder>(entity =>
            {
                entity.HasKey(e => e.ProdOrderId)
                    .HasName("PK__product___3D27750A363FD9BB");

                entity.ToTable("product_order");

                entity.Property(e => e.ProdOrderId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("prod_order_id");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("order_id");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("price");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("product_id");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ProductOrders)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__product_o__order__4BAC3F29");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductOrders)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__product_o__produ__4CA06362");
            });

            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.HasKey(e => e.ProdTypeId)
                    .HasName("PK__product___D7B875BF6B90430A");

                entity.ToTable("product_type");

                entity.Property(e => e.ProdTypeId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("prod_type_id");

                entity.Property(e => e.ProdTypeDescription).HasColumnName("prod_type_description");

                entity.Property(e => e.ProdTypeName).HasColumnName("prod_type_name");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("status");

                entity.Property(e => e.StatusId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("status_id");

                entity.Property(e => e.StatusName).HasColumnName("status_name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.UserId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("user_id");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.Country).HasColumnName("country");

                entity.Property(e => e.Email).HasColumnName("email");

                entity.Property(e => e.FirstName).HasColumnName("first_name");

                entity.Property(e => e.LastName).HasColumnName("last_name");

                entity.Property(e => e.Password).HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Province).HasColumnName("province");

                entity.Property(e => e.Role).HasColumnName("role");

                entity.Property(e => e.ZipCode).HasColumnName("zip_code");
            });

            //OnModelCreatingPartial(modelBuilder);
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        
    }
}

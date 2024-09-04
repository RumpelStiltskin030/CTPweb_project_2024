using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HomeWorks.Models;

public partial class RS0605Context : DbContext
{
    public RS0605Context()
    {
    }

    public RS0605Context(DbContextOptions<RS0605Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductDetail> ProductDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-NNRTTL7;Database=RS0605;TrustServerCertificate=True;User ID=A123;Password=000000");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MeId).HasName("PK__Member__1234DAA47EED9903");

            entity.ToTable("Member");

            entity.Property(e => e.MeId).HasMaxLength(50);
            entity.Property(e => e.MeEmail).HasMaxLength(40);
            entity.Property(e => e.MeName).HasMaxLength(20);
            entity.Property(e => e.MePassword).HasMaxLength(10);
            entity.Property(e => e.MeTel)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PhotoPath).HasMaxLength(200);
            entity.Property(e => e.PreId).HasMaxLength(2);

            entity.HasOne(d => d.Pre).WithMany(p => p.Members)
                .HasForeignKey(d => d.PreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Member_Permissions");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrdId).HasName("PK__Order__67A28336F487F66A");

            entity.ToTable("Order");

            entity.Property(e => e.OrdId).HasMaxLength(50);
            entity.Property(e => e.DateLine).HasColumnType("datetime");
            entity.Property(e => e.MeId).HasMaxLength(50);
            entity.Property(e => e.OrdDate).HasColumnType("datetime");
            entity.Property(e => e.SeleId)
                .HasMaxLength(50)
                .HasColumnName("SeleID");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .HasDefaultValue("A");

            entity.HasOne(d => d.Me).WithMany(p => p.OrderMes)
                .HasForeignKey(d => d.MeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__MeId__440B1D61");

            entity.HasOne(d => d.Sele).WithMany(p => p.OrderSeles)
                .HasForeignKey(d => d.SeleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Member");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__67A28336A019E1F8");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.OrdId).HasMaxLength(50);
            entity.Property(e => e.Pricing).HasColumnType("money");
            entity.Property(e => e.ProId).HasMaxLength(20);
            entity.Property(e => e.Specification).HasMaxLength(30);

            entity.HasOne(d => d.Ord).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrdId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Order");

            entity.HasOne(d => d.Pro).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__ProId__46E78A0C");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PreId).HasName("PK__Permissi__7024CEC91F7D01C3");

            entity.Property(e => e.PreId).HasMaxLength(2);
            entity.Property(e => e.PreName).HasMaxLength(10);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProId).HasName("PK__Product__62029590319D4EB1");

            entity.ToTable("Product");

            entity.Property(e => e.ProId).HasMaxLength(20);
            entity.Property(e => e.MeId).HasMaxLength(50);
            entity.Property(e => e.ProName).HasMaxLength(20);
            entity.Property(e => e.ProPrice).HasColumnType("money");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .HasDefaultValue("A");

            entity.HasOne(d => d.Me).WithMany(p => p.Products)
                .HasForeignKey(d => d.MeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__MeId__3E52440B");
        });

        modelBuilder.Entity<ProductDetail>(entity =>
        {
            entity.ToTable("ProductDetail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Fieldpath)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProId).HasMaxLength(20);

            entity.HasOne(d => d.Pro).WithMany(p => p.ProductDetails)
                .HasForeignKey(d => d.ProId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductDe__ProId__49C3F6B7");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

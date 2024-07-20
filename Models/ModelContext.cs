using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Gifts_Store_First_project.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<GiftAbout> GiftAbouts { get; set; }

    public virtual DbSet<GiftCategory> GiftCategories { get; set; }

    public virtual DbSet<GiftContact> GiftContacts { get; set; }

    public virtual DbSet<GiftGift> GiftGifts { get; set; }

    public virtual DbSet<GiftGiftsUser> GiftGiftsUsers { get; set; }

    public virtual DbSet<GiftHome> GiftHomes { get; set; }

    public virtual DbSet<GiftOrder> GiftOrders { get; set; }

    public virtual DbSet<GiftPayment> GiftPayments { get; set; }

    public virtual DbSet<GiftRole> GiftRoles { get; set; }

    public virtual DbSet<GiftTestimonial> GiftTestimonials { get; set; }

    public virtual DbSet<GiftUser> GiftUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("USER ID=JOR18_USER317;PASSWORD=mais2001;DATA SOURCE=94.56.229.181:3488/traindb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("JOR18_USER317")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<GiftAbout>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GIFT_ABOUT_PK");

            entity.ToTable("GIFT_ABOUT");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Content)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("CONTENT");
            entity.Property(e => e.HomeId)
                .HasColumnType("NUMBER")
                .HasColumnName("HOME_ID");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMAGE_PATH");

            entity.HasOne(d => d.Home).WithMany(p => p.GiftAbouts)
                .HasForeignKey(d => d.HomeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("GIFT_ABOUT_FK1");
        });

        modelBuilder.Entity<GiftCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GIFT_CATEGORY_PK");

            entity.ToTable("GIFT_CATEGORY");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMAGE_PATH");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<GiftContact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GIFT_CONTACT_PK");

            entity.ToTable("GIFT_CONTACT");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.HomeId)
                .HasColumnType("NUMBER")
                .HasColumnName("HOME_ID");
            entity.Property(e => e.Location)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("LOCATION");
            entity.Property(e => e.Message)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("MESSAGE");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("PHONE_NUMBER");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USER_EMAIL");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USER_NAME");

            entity.HasOne(d => d.Home).WithMany(p => p.GiftContacts)
                .HasForeignKey(d => d.HomeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("GIFT_CONTACT_FK1");
        });

        modelBuilder.Entity<GiftGift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GIFT_GIFTS_PK");

            entity.ToTable("GIFT_GIFTS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.CategoryId)
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORY_ID");
            entity.Property(e => e.Description)
                .HasMaxLength(600)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMAGE_PATH");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Price)
                .HasColumnType("FLOAT")
                .HasColumnName("PRICE");
            entity.Property(e => e.Sale)
                .HasColumnType("NUMBER")
                .HasColumnName("SALE");

            entity.HasOne(d => d.Category).WithMany(p => p.GiftGifts)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("GIFT_GIFTS_FK1");
        });

        modelBuilder.Entity<GiftGiftsUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GIFT_GIFTS_USERS_PK");

            entity.ToTable("GIFT_GIFTS_USERS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.DateFrom)
                .HasColumnType("DATE")
                .HasColumnName("DATE_FROM");
            entity.Property(e => e.DateTo)
                .HasColumnType("DATE")
                .HasColumnName("DATE_TO");
            entity.Property(e => e.GiftId)
                .HasColumnType("NUMBER")
                .HasColumnName("GIFT_ID");
            entity.Property(e => e.Quantity)
                .HasColumnType("NUMBER")
                .HasColumnName("QUANTITY");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Gift).WithMany(p => p.GiftGiftsUsers)
                .HasForeignKey(d => d.GiftId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("GIFT_GIFTS_USERS_FK2");

            entity.HasOne(d => d.User).WithMany(p => p.GiftGiftsUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("GIFT_GIFTS_USERS_FK1");
        });

        modelBuilder.Entity<GiftHome>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GIFT_HOME_PK");

            entity.ToTable("GIFT_HOME");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Content)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("CONTENT");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMAGE_PATH");
        });

        modelBuilder.Entity<GiftOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GIFT_ORDER_PK");

            entity.ToTable("GIFT_ORDER");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.AdminProfits)
                .HasColumnType("FLOAT")
                .HasColumnName("ADMIN_PROFITS");
            entity.Property(e => e.GiftId)
                .HasColumnType("NUMBER")
                .HasColumnName("GIFT_ID");
            entity.Property(e => e.MakerProfits)
                .HasColumnType("FLOAT")
                .HasColumnName("MAKER_PROFITS");
            entity.Property(e => e.OrderDate)
                .HasColumnType("DATE")
                .HasColumnName("ORDER_DATE");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("PHONE_NUMBER");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Gift).WithMany(p => p.GiftOrders)
                .HasForeignKey(d => d.GiftId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("GIFT_ORDER_FK2");

            entity.HasOne(d => d.User).WithMany(p => p.GiftOrders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("GIFT_ORDER_FK1");
        });

        modelBuilder.Entity<GiftPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GIFT_PAYMENT_PK");

            entity.ToTable("GIFT_PAYMENT");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Balance)
                .HasColumnType("FLOAT")
                .HasColumnName("BALANCE");
            entity.Property(e => e.CardNumber)
                .HasColumnType("NUMBER")
                .HasColumnName("CARD_NUMBER");
            entity.Property(e => e.Cvv)
                .HasColumnType("NUMBER")
                .HasColumnName("CVV");
            entity.Property(e => e.ExpiryDate)
                .HasColumnType("DATE")
                .HasColumnName("EXPIRY_DATE");
        });

        modelBuilder.Entity<GiftRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GIFT_ROLES_PK");

            entity.ToTable("GIFT_ROLES");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ROLE_NAME");
        });

        modelBuilder.Entity<GiftTestimonial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GIFT_TESTIMONIAL_PK");

            entity.ToTable("GIFT_TESTIMONIAL");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Message)
                .HasMaxLength(600)
                .IsUnicode(false)
                .HasColumnName("MESSAGE");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.User).WithMany(p => p.GiftTestimonials)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("GIFT_TESTIMONIAL_FK1");
        });

        modelBuilder.Entity<GiftUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GIFT_USERS_PK");

            entity.ToTable("GIFT_USERS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.CategoryId)
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORY_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Fname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("FNAME");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMAGE_PATH");
            entity.Property(e => e.Lname)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("LNAME");
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PHONE_NUMBER");
            entity.Property(e => e.RoleId)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLE_ID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.Username)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.Category).WithMany(p => p.GiftUsers)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("GIFT_USERS_FK2");

            entity.HasOne(d => d.Role).WithMany(p => p.GiftUsers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("GIFT_USERS_FK1");
        });
        modelBuilder.HasSequence("EMPLOYEE_SEQ");

        OnModelCreatingPartial(modelBuilder);
        
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

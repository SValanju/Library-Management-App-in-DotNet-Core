using System;
using System.Collections.Generic;
using Library_WebAPI.Helpers;
using Library_WebAPI.Helpers.Utils;
using Microsoft.EntityFrameworkCore;

namespace Library_WebAPI.Models;

public partial class DefaultContext : DbContext
{
    public DefaultContext() { }

    public DefaultContext(DbContextOptions<DefaultContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblBook> TblBooks { get; set; }

    public virtual DbSet<TblBookTransaction> TblBookTransactions { get; set; }

    public virtual DbSet<TblLogin> TblLogins { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(AppSettingsHelper.GetSetting("ConnectionStrings:DefaultConnection"));

            #region Old logic
            //IConfigurationRoot configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json")
            //    .Build();

            //optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            #endregion
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblBook>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__TBL_BOOK__054D27E4DC28275B");

            entity.ToTable("TBL_BOOKS");

            entity.Property(e => e.BookId).HasColumnName("BOOK_ID");
            entity.Property(e => e.BookCount).HasColumnName("BOOK_COUNT");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATED_AT");
            entity.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("DELETED_AT");
            entity.Property(e => e.DeletedBy).HasColumnName("DELETED_BY");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TITLE");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("UPDATED_AT");
            entity.Property(e => e.UpdatedBy).HasColumnName("UPDATED_BY");
        });

        modelBuilder.Entity<TblBookTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__TBL_BOOK__16998B61E4779713");

            entity.ToTable("TBL_BOOK_TRANSACTIONS");

            entity.Property(e => e.TransactionId).HasColumnName("TRANSACTION_ID");
            entity.Property(e => e.BookId).HasColumnName("BOOK_ID");
            entity.Property(e => e.ReturnDate).HasColumnName("RETURN_DATE");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("TRANSACTION_DATE");
            entity.Property(e => e.UserId).HasColumnName("USER_ID");

            entity.HasOne(d => d.Book).WithMany(p => p.TblBookTransactions)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookTransaction_BookId");

            entity.HasOne(d => d.User).WithMany(p => p.TblBookTransactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookTransaction_UserId");
        });

        modelBuilder.Entity<TblLogin>(entity =>
        {
            entity.HasKey(e => e.LoginId).HasName("PK__TBL_LOGI__476A024DB3775768");

            entity.ToTable("TBL_LOGINS");

            entity.Property(e => e.LoginId).HasColumnName("LOGIN_ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATED_AT");
            entity.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
            entity.Property(e => e.Token)
                .IsUnicode(false)
                .HasColumnName("TOKEN");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("UPDATED_AT");
            entity.Property(e => e.UpdatedBy).HasColumnName("UPDATED_BY");
            entity.Property(e => e.UserId).HasColumnName("USER_ID");

            entity.HasOne(d => d.User).WithMany(p => p.TblLogins)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Login_UserId");
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__TBL_ROLE__5AC4D222E25F3371");

            entity.ToTable("TBL_ROLES");

            entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATED_AT");
            entity.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("DELETED_AT");
            entity.Property(e => e.DeletedBy).HasColumnName("DELETED_BY");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.RoleDesc)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ROLE_DESC");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("UPDATED_AT");
            entity.Property(e => e.UpdatedBy).HasColumnName("UPDATED_BY");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__TBL_USER__F3BEEBFFE564C932");

            entity.ToTable("TBL_USERS");

            entity.HasIndex(e => e.EmailId, "UQ__TBL_USER__6BB22C4C1420AFE5").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__TBL_USER__E0B75F6F0F36BCD0").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("USER_ID");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CONTACT_NUMBER");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATED_AT");
            entity.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("DELETED_AT");
            entity.Property(e => e.DeletedBy).HasColumnName("DELETED_BY");
            entity.Property(e => e.EmailId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL_ID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("FIRST_NAME");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("LAST_NAME");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("UPDATED_AT");
            entity.Property(e => e.UpdatedBy).HasColumnName("UPDATED_BY");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USER_NAME");
            entity.Property(e => e.UserRole).HasColumnName("USER_ROLE");

            entity.HasOne(d => d.UserRoleNavigation).WithMany(p => p.TblUsers)
                .HasForeignKey(d => d.UserRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_RoleId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

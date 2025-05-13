using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VeriStore.Models;

public partial class DxnStoreContext : DbContext
{
    public DxnStoreContext()
    {
    }

    public DxnStoreContext(DbContextOptions<DxnStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Musteri> Musteris { get; set; }

    public virtual DbSet<Sipari> Siparis { get; set; }

    public virtual DbSet<Siparisdetay> Siparisdetays { get; set; }

    public virtual DbSet<Urun> Uruns { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;database=dxn_store;user id=root;password=EmadTaha11");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Musteri>(entity =>
        {
            entity.HasKey(e => e.MusteriId).HasName("PRIMARY");

            entity.ToTable("musteri");

            entity.Property(e => e.MusteriId).HasColumnName("MusteriID");
            entity.Property(e => e.Adres).HasColumnType("text");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Isim).HasMaxLength(50);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Soyisim).HasMaxLength(50);
            entity.Property(e => e.Telefon).HasMaxLength(15);
        });

        modelBuilder.Entity<Sipari>(entity =>
        {
            entity.HasKey(e => e.SiparisId).HasName("PRIMARY");

            entity.ToTable("siparis");

            entity.HasIndex(e => e.MusteriId, "MusteriID");

            entity.Property(e => e.SiparisId).HasColumnName("SiparisID");
            entity.Property(e => e.MusteriId).HasColumnName("MusteriID");
            entity.Property(e => e.SiparisTarihi).HasColumnType("date");
            entity.Property(e => e.ToplamTutar).HasPrecision(10);

            entity.HasOne(d => d.Musteri).WithMany(p => p.Siparis)
                .HasForeignKey(d => d.MusteriId)
                .HasConstraintName("siparis_ibfk_1");
        });

        modelBuilder.Entity<Siparisdetay>(entity =>
        {
            entity.HasKey(e => e.SiparisDetayId).HasName("PRIMARY");

            entity.ToTable("siparisdetay");

            entity.HasIndex(e => e.SiparisId, "SiparisID");

            entity.HasIndex(e => e.UrunId, "UrunID");

            entity.Property(e => e.SiparisDetayId).HasColumnName("SiparisDetayID");
            entity.Property(e => e.BirimFiyat).HasPrecision(10);
            entity.Property(e => e.SiparisId).HasColumnName("SiparisID");
            entity.Property(e => e.UrunId).HasColumnName("UrunID");

            entity.HasOne(d => d.Siparis).WithMany(p => p.Siparisdetays)
                .HasForeignKey(d => d.SiparisId)
                .HasConstraintName("siparisdetay_ibfk_1");

            entity.HasOne(d => d.Urun).WithMany(p => p.Siparisdetays)
                .HasForeignKey(d => d.UrunId)
                .HasConstraintName("siparisdetay_ibfk_2");
        });

        modelBuilder.Entity<Urun>(entity =>
        {
            entity.HasKey(e => e.UrunId).HasName("PRIMARY");

            entity.ToTable("urun");

            entity.Property(e => e.UrunId).HasColumnName("UrunID");
            entity.Property(e => e.Aciklama).HasColumnType("text");
            entity.Property(e => e.Fiyat).HasPrecision(10);
            entity.Property(e => e.Kategori).HasMaxLength(50);
            entity.Property(e => e.UrunAdi).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    // Retrieves all Urunler using the stored procedure sp_GetAllUruns
    public List<Urun> GetAllUruns()
    {
        return Uruns.FromSqlRaw("CALL sp_GetAllUruns()").ToList();
    }
}

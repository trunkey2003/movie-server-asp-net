using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TrunkeyWebApp.Models
{
    public partial class movie_server_cybersoftContext : DbContext
    {
        public movie_server_cybersoftContext()
        {
        }

        public movie_server_cybersoftContext(DbContextOptions<movie_server_cybersoftContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Banner> Banners { get; set; } = null!;
        public virtual DbSet<CumRap> CumRaps { get; set; } = null!;
        public virtual DbSet<Ghe> Ghes { get; set; } = null!;
        public virtual DbSet<HeThongRap> HeThongRaps { get; set; } = null!;
        public virtual DbSet<LichChieu> LichChieus { get; set; } = null!;
        public virtual DbSet<LoaiNguoiDung> LoaiNguoiDungs { get; set; } = null!;
        public virtual DbSet<NguoiDung> NguoiDungs { get; set; } = null!;
        public virtual DbSet<Phim> Phims { get; set; } = null!;
        public virtual DbSet<Rap> Raps { get; set; } = null!;

        //backup

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Banner>(entity =>
            {
                entity.HasKey(e => e.MaBanner)
                    .HasName("PRIMARY");

                entity.ToTable("Banner");

                entity.HasIndex(e => e.MaPhim, "fk_Banner_Phim1_idx");

                entity.HasIndex(e => e.MaBanner, "maBanner_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.MaBanner)
                    .ValueGeneratedNever()
                    .HasColumnName("maBanner");

                entity.Property(e => e.HinhAnh)
                    .HasColumnType("text")
                    .HasColumnName("hinhAnh");

                entity.Property(e => e.MaPhim).HasColumnName("maPhim");

                entity.HasOne(d => d.MaPhimNavigation)
                    .WithMany(p => p.Banners)
                    .HasForeignKey(d => d.MaPhim)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Banner_Phim1");
            });

            modelBuilder.Entity<CumRap>(entity =>
            {
                entity.HasKey(e => e.MaCumRap)
                    .HasName("PRIMARY");

                entity.ToTable("Cum_Rap");

                entity.HasIndex(e => e.MaHeThongRap, "fk_Cum_Rap_He_Thong_Rap_idx");

                entity.HasIndex(e => e.MaCumRap, "maCumRap_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.MaCumRap)
                    .HasMaxLength(100)
                    .HasColumnName("maCumRap");

                entity.Property(e => e.DiaChi)
                    .HasColumnType("text")
                    .HasColumnName("diaChi");

                entity.Property(e => e.MaHeThongRap)
                    .HasMaxLength(30)
                    .HasColumnName("maHeThongRap");

                entity.Property(e => e.TenCumRap)
                    .HasColumnType("text")
                    .HasColumnName("tenCumRap");

                entity.HasOne(d => d.MaHeThongRapNavigation)
                    .WithMany(p => p.CumRaps)
                    .HasForeignKey(d => d.MaHeThongRap)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Cum_Rap_He_Thong_Rap");
            });

            modelBuilder.Entity<Ghe>(entity =>
            {
                entity.HasKey(e => e.MaGhe)
                    .HasName("PRIMARY");

                entity.ToTable("Ghe");

                entity.HasIndex(e => e.MaLichChieu, "fk_Ghe_Lich_Chieu_idx");

                entity.HasIndex(e => e.TaiKhoanNguoiDat, "fk_Ghe_Nguoi_Dung_idx");

                entity.Property(e => e.MaGhe).HasColumnName("maGhe");

                entity.Property(e => e.DaDat)
                    .HasMaxLength(45)
                    .HasColumnName("daDat");

                entity.Property(e => e.LoaiGhe)
                    .HasMaxLength(45)
                    .HasColumnName("loaiGhe");

                entity.Property(e => e.MaLichChieu).HasColumnName("maLichChieu");

                entity.Property(e => e.SttGhe).HasColumnName("sttGhe");

                entity.Property(e => e.TaiKhoanNguoiDat)
                    .HasMaxLength(45)
                    .HasColumnName("taiKhoanNguoiDat");

                entity.HasOne(d => d.MaLichChieuNavigation)
                    .WithMany(p => p.Ghes)
                    .HasForeignKey(d => d.MaLichChieu)
                    .HasConstraintName("fk_Ghe_Lich_Chieu");

                entity.HasOne(d => d.TaiKhoanNguoiDatNavigation)
                    .WithMany(p => p.Ghes)
                    .HasForeignKey(d => d.TaiKhoanNguoiDat)
                    .HasConstraintName("fk_Ghe_Nguoi_Dung");
            });

            modelBuilder.Entity<HeThongRap>(entity =>
            {
                entity.HasKey(e => e.MaHeThongRap)
                    .HasName("PRIMARY");

                entity.ToTable("He_Thong_Rap");

                entity.HasIndex(e => e.MaHeThongRap, "maHeThongRap_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.MaHeThongRap)
                    .HasMaxLength(30)
                    .HasColumnName("maHeThongRap");

                entity.Property(e => e.BiDanh)
                    .HasColumnType("text")
                    .HasColumnName("biDanh");

                entity.Property(e => e.Logo)
                    .HasColumnType("text")
                    .HasColumnName("logo");

                entity.Property(e => e.TenHeThongRap)
                    .HasColumnType("text")
                    .HasColumnName("tenHeThongRap");
            });

            modelBuilder.Entity<LichChieu>(entity =>
            {
                entity.HasKey(e => e.MaLichChieu)
                    .HasName("PRIMARY");

                entity.ToTable("Lich_Chieu");

                entity.HasIndex(e => e.MaPhim, "fk_Lich_Chieu_Phim1_idx");

                entity.HasIndex(e => e.MaRap, "fk_Lich_Chieu_Rap1_idx");

                entity.HasIndex(e => e.MaLichChieu, "maLichChieu_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.MaLichChieu).HasColumnName("maLichChieu");

                entity.Property(e => e.GiaVe)
                    .HasColumnName("giaVe")
                    .HasDefaultValueSql("'75000'");

                entity.Property(e => e.MaPhim).HasColumnName("maPhim");

                entity.Property(e => e.MaRap).HasColumnName("maRap");

                entity.Property(e => e.NgayChieuGioChieu)
                    .HasColumnType("datetime")
                    .HasColumnName("ngayChieuGioChieu")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.MaPhimNavigation)
                    .WithMany(p => p.LichChieus)
                    .HasForeignKey(d => d.MaPhim)
                    .HasConstraintName("fk_Lich_Chieu_Phim");

                entity.HasOne(d => d.MaRapNavigation)
                    .WithMany(p => p.LichChieus)
                    .HasForeignKey(d => d.MaRap)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Lich_Chieu_Rap");
            });

            modelBuilder.Entity<LoaiNguoiDung>(entity =>
            {
                entity.HasKey(e => e.MaLoaiNguoiDung)
                    .HasName("PRIMARY");

                entity.ToTable("Loai_Nguoi_Dung");

                entity.Property(e => e.MaLoaiNguoiDung)
                    .HasMaxLength(10)
                    .HasColumnName("maLoaiNguoiDung");

                entity.Property(e => e.TenLoai)
                    .HasMaxLength(45)
                    .HasColumnName("tenLoai");
            });

            modelBuilder.Entity<NguoiDung>(entity =>
            {
                entity.HasKey(e => e.TaiKhoan)
                    .HasName("PRIMARY");

                entity.ToTable("Nguoi_Dung");

                entity.HasIndex(e => e.MaLoaiNguoiDung, "fk_Nguoi_Dung_LoaiNguoiDung_idx");

                entity.HasIndex(e => e.TaiKhoan, "taiKhoan_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.TaiKhoan)
                    .HasMaxLength(30)
                    .HasColumnName("taiKhoan");

                entity.Property(e => e.Email)
                    .HasColumnType("text")
                    .HasColumnName("email");

                entity.Property(e => e.HoTen)
                    .HasColumnType("text")
                    .HasColumnName("hoTen");

                entity.Property(e => e.MaLoaiNguoiDung)
                    .HasMaxLength(10)
                    .HasColumnName("maLoaiNguoiDung")
                    .HasDefaultValueSql("'user'");

                entity.Property(e => e.MatKhau)
                    .HasColumnType("text")
                    .HasColumnName("matKhau");

                entity.Property(e => e.SoDt)
                    .HasColumnType("text")
                    .HasColumnName("soDt");

                entity.HasOne(d => d.MaLoaiNguoiDungNavigation)
                    .WithMany(p => p.NguoiDungs)
                    .HasForeignKey(d => d.MaLoaiNguoiDung)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Nguoi_Dung_LoaiNguoiDung");
            });

            modelBuilder.Entity<Phim>(entity =>
            {
                entity.HasKey(e => e.MaPhim)
                    .HasName("PRIMARY");

                entity.ToTable("Phim");

                entity.Property(e => e.MaPhim)
                    .ValueGeneratedNever()
                    .HasColumnName("maPhim");

                entity.Property(e => e.BiDanh)
                    .HasColumnType("text")
                    .HasColumnName("biDanh");

                entity.Property(e => e.DangChieu)
                    .HasColumnType("text")
                    .HasColumnName("dangChieu");

                entity.Property(e => e.DanhGia).HasColumnName("danhGia");

                entity.Property(e => e.HinhAnh)
                    .HasColumnType("text")
                    .HasColumnName("hinhAnh");

                entity.Property(e => e.Hot)
                    .HasColumnType("text")
                    .HasColumnName("hot");

                entity.Property(e => e.MaNhom)
                    .HasColumnType("text")
                    .HasColumnName("maNhom");

                entity.Property(e => e.MoTa)
                    .HasColumnType("text")
                    .HasColumnName("moTa");

                entity.Property(e => e.NgayKhoiChieu)
                    .HasColumnType("text")
                    .HasColumnName("ngayKhoiChieu");

                entity.Property(e => e.SapChieu)
                    .HasColumnType("text")
                    .HasColumnName("sapChieu");

                entity.Property(e => e.TenPhim)
                    .HasColumnType("text")
                    .HasColumnName("tenPhim");

                entity.Property(e => e.Trailer)
                    .HasColumnType("text")
                    .HasColumnName("trailer");
            });

            modelBuilder.Entity<Rap>(entity =>
            {
                entity.HasKey(e => e.MaRap)
                    .HasName("PRIMARY");

                entity.ToTable("Rap");

                entity.HasIndex(e => e.MaCumRap, "fk_Rap_Cum_Rap1_idx");

                entity.Property(e => e.MaRap)
                    .ValueGeneratedNever()
                    .HasColumnName("maRap");

                entity.Property(e => e.MaCumRap)
                    .HasMaxLength(100)
                    .HasColumnName("maCumRap");

                entity.Property(e => e.TenRap)
                    .HasColumnType("text")
                    .HasColumnName("tenRap");

                entity.HasOne(d => d.MaCumRapNavigation)
                    .WithMany(p => p.Raps)
                    .HasForeignKey(d => d.MaCumRap)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Rap_Cum_Rap1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

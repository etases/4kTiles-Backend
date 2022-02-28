using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using _4kTiles_Backend.Entities;

namespace _4kTiles_Backend.Context
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<AccountRole> AccountRoles { get; set; } = null!;
        public virtual DbSet<AccountSong> AccountSongs { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Song> Songs { get; set; } = null!;
        public virtual DbSet<SongGenre> SongGenres { get; set; } = null!;
        public virtual DbSet<SongReport> SongReports { get; set; } = null!;
        public virtual DbSet<SongTag> SongTags { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Dob).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AccountRole>(entity =>
            {
                entity.HasKey(e => e.ArId)
                    .HasName("PK__AccountR__5DB4D0D31CB19BE9");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountRoles)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AccountRo__accou__6383C8BA");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AccountRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AccountRo__roleI__6477ECF3");
            });

            modelBuilder.Entity<AccountSong>(entity =>
            {
                entity.HasKey(e => e.AsId)
                    .HasName("PK__AccountS__5B76EEC8EC990C16");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountSongs)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AccountSo__accou__74AE54BC");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.AccountSongs)
                    .HasForeignKey(d => d.SongId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AccountSo__songI__75A278F5");
            });

            modelBuilder.Entity<Song>(entity =>
            {
                entity.Property(e => e.Bpm).HasDefaultValueSql("((100))");

                entity.Property(e => e.ReleaseDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<SongGenre>(entity =>
            {
                entity.HasKey(e => e.SgId)
                    .HasName("PK__SongGenr__2D0DA15BEEAA98BE");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.SongGenres)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SongGenre__genre__05D8E0BE");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.SongGenres)
                    .HasForeignKey(d => d.SongId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SongGenre__songI__04E4BC85");
            });

            modelBuilder.Entity<SongReport>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("PK__SongRepo__1C9B4E2D7E46E8B2");

                entity.Property(e => e.ReportDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.SongReports)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SongRepor__accou__7B5B524B");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.SongReports)
                    .HasForeignKey(d => d.SongId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SongRepor__songI__7A672E12");
            });

            modelBuilder.Entity<SongTag>(entity =>
            {
                entity.HasKey(e => e.StId)
                    .HasName("PK__SongTag__312C03EF9D0E351D");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.SongTags)
                    .HasForeignKey(d => d.SongId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SongTag__songId__6FE99F9F");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.SongTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SongTag__tagId__70DDC3D8");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

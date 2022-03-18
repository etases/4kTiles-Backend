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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasData(new Role {RoleId = 1, RoleName = "Admin"}, new Role {RoleId = 2, RoleName = "User"});
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.DeletedReason).HasDefaultValueSql("NULL::character varying");
                
                entity.Property(e => e.Dob).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<AccountRole>(entity =>
            {
                entity.HasKey(e => e.ArId)
                    .HasName("accountrole_pkey");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountRoles)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("accountrole_accountid_fkey");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AccountRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("accountrole_roleid_fkey");
            });

            modelBuilder.Entity<AccountSong>(entity =>
            {
                entity.HasKey(e => e.AsId)
                    .HasName("accountsong_pkey");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountSongs)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("accountsong_accountid_fkey");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.AccountSongs)
                    .HasForeignKey(d => d.SongId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("accountsong_songid_fkey");
            });

            modelBuilder.Entity<Song>(entity =>
            {
                entity.Property(e => e.Bpm).HasDefaultValueSql("100");

                entity.Property(e => e.DeletedReason).HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.ReleaseDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<SongGenre>(entity =>
            {
                entity.HasKey(e => e.SgId)
                    .HasName("songgenre_pkey");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.SongGenres)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("songgenre_genreid_fkey");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.SongGenres)
                    .HasForeignKey(d => d.SongId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("songgenre_songid_fkey");
            });

            modelBuilder.Entity<SongReport>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("songreport_pkey");

                entity.Property(e => e.ReportDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.SongReports)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("songreport_accountid_fkey");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.SongReports)
                    .HasForeignKey(d => d.SongId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("songreport_songid_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

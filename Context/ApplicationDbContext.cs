using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using _4kTiles_Backend.Entities;

namespace _4kTiles_Backend.Context
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Accountrole> Accountroles { get; set; } = null!;
        public virtual DbSet<Accountsong> Accountsongs { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Song> Songs { get; set; } = null!;
        public virtual DbSet<Songgenre> Songgenres { get; set; } = null!;
        public virtual DbSet<Songreport> Songreports { get; set; } = null!;
        public virtual DbSet<Songtag> Songtags { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Deletedreason).HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Dob).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Accountrole>(entity =>
            {
                entity.HasKey(e => e.Arid)
                    .HasName("accountrole_pkey");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Accountroles)
                    .HasForeignKey(d => d.Accountid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("accountrole_accountid_fkey");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accountroles)
                    .HasForeignKey(d => d.Roleid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("accountrole_roleid_fkey");
            });

            modelBuilder.Entity<Accountsong>(entity =>
            {
                entity.HasKey(e => e.Asid)
                    .HasName("accountsong_pkey");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Accountsongs)
                    .HasForeignKey(d => d.Accountid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("accountsong_accountid_fkey");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.Accountsongs)
                    .HasForeignKey(d => d.Songid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("accountsong_songid_fkey");
            });

            modelBuilder.Entity<Song>(entity =>
            {
                entity.Property(e => e.Bpm).HasDefaultValueSql("100");

                entity.Property(e => e.Deletedreason).HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Releasedate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Songgenre>(entity =>
            {
                entity.HasKey(e => e.Sgid)
                    .HasName("songgenre_pkey");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Songgenres)
                    .HasForeignKey(d => d.Genreid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("songgenre_genreid_fkey");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.Songgenres)
                    .HasForeignKey(d => d.Songid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("songgenre_songid_fkey");
            });

            modelBuilder.Entity<Songreport>(entity =>
            {
                entity.HasKey(e => e.Reportid)
                    .HasName("songreport_pkey");

                entity.Property(e => e.Reportdate).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Songreports)
                    .HasForeignKey(d => d.Accountid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("songreport_accountid_fkey");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.Songreports)
                    .HasForeignKey(d => d.Songid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("songreport_songid_fkey");
            });

            modelBuilder.Entity<Songtag>(entity =>
            {
                entity.HasKey(e => e.Stid)
                    .HasName("songtag_pkey");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.Songtags)
                    .HasForeignKey(d => d.Songid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("songtag_songid_fkey");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.Songtags)
                    .HasForeignKey(d => d.Tagid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("songtag_tagid_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

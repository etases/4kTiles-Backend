using Microsoft.EntityFrameworkCore;
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
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Song> Songs { get; set; } = null!;
        public virtual DbSet<SongReport> SongReports { get; set; } = null!;
        public virtual DbSet<SongTag> SongTags { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DevelopmentDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.AccountId).HasColumnName("accountId");

                entity.Property(e => e.DeletedReason)
                    .HasMaxLength(255)
                    .HasColumnName("deletedReason");

                entity.Property(e => e.Dob)
                    .HasColumnType("datetime")
                    .HasColumnName("dob")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.HashedPassword)
                    .HasMaxLength(255)
                    .HasColumnName("hashedPassword");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasColumnName("userName");
            });

            modelBuilder.Entity<AccountRole>(entity =>
            {
                entity.HasKey(e => e.ArId)
                    .HasName("PK__AccountR__5DB4D0D302D63A62");

                entity.ToTable("AccountRole");

                entity.Property(e => e.ArId).HasColumnName("arId");

                entity.Property(e => e.AccountId).HasColumnName("accountId");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountRoles)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AccountRo__accou__0880433F");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AccountRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AccountRo__roleI__09746778");
            });

            modelBuilder.Entity<AccountSong>(entity =>
            {
                entity.HasKey(e => e.AsId)
                    .HasName("PK__AccountS__5B76EEC8C59BB508");

                entity.ToTable("AccountSong");

                entity.Property(e => e.AsId).HasColumnName("asId");

                entity.Property(e => e.AccountId).HasColumnName("accountId");

                entity.Property(e => e.BestScore).HasColumnName("bestScore");

                entity.Property(e => e.SongId).HasColumnName("songId");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountSongs)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AccountSo__accou__19AACF41");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.AccountSongs)
                    .HasForeignKey(d => d.SongId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AccountSo__songI__1A9EF37A");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .HasColumnName("roleName");
            });

            modelBuilder.Entity<Song>(entity =>
            {
                entity.ToTable("Song");

                entity.Property(e => e.SongId).HasColumnName("songId");

                entity.Property(e => e.Author)
                    .HasMaxLength(255)
                    .HasColumnName("author");

                entity.Property(e => e.Bpm)
                    .HasColumnName("bpm")
                    .HasDefaultValueSql("((100))");

                entity.Property(e => e.DeletedReason)
                    .HasMaxLength(255)
                    .HasColumnName("deletedReason");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.IsPublic).HasColumnName("isPublic");

                entity.Property(e => e.Notes)
                    .HasMaxLength(1000)
                    .HasColumnName("notes");

                entity.Property(e => e.ReleaseDate)
                    .HasColumnType("datetime")
                    .HasColumnName("releaseDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SongName)
                    .HasMaxLength(255)
                    .HasColumnName("songName");
            });

            modelBuilder.Entity<SongReport>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("PK__SongRepo__1C9B4E2DD518774C");

                entity.ToTable("SongReport");

                entity.Property(e => e.ReportId).HasColumnName("reportId");

                entity.Property(e => e.AccountId).HasColumnName("accountId");

                entity.Property(e => e.ReportDate)
                    .HasColumnType("datetime")
                    .HasColumnName("reportDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ReportReason)
                    .HasMaxLength(255)
                    .HasColumnName("reportReason");

                entity.Property(e => e.ReportStatus).HasColumnName("reportStatus");

                entity.Property(e => e.ReportTitle)
                    .HasMaxLength(255)
                    .HasColumnName("reportTitle");

                entity.Property(e => e.SongId).HasColumnName("songId");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.SongReports)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SongRepor__accou__2057CCD0");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.SongReports)
                    .HasForeignKey(d => d.SongId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SongRepor__songI__1F63A897");
            });

            modelBuilder.Entity<SongTag>(entity =>
            {
                entity.HasKey(e => e.StId)
                    .HasName("PK__SongTag__312C03EFFD1DA4AA");

                entity.ToTable("SongTag");

                entity.Property(e => e.StId).HasColumnName("stId");

                entity.Property(e => e.SongId).HasColumnName("songId");

                entity.Property(e => e.TagId).HasColumnName("tagId");

                entity.HasOne(d => d.Song)
                    .WithMany(p => p.SongTags)
                    .HasForeignKey(d => d.SongId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SongTag__songId__14E61A24");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.SongTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SongTag__tagId__15DA3E5D");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");

                entity.Property(e => e.TagId).HasColumnName("tagId");

                entity.Property(e => e.TagName)
                    .HasMaxLength(50)
                    .HasColumnName("tagName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

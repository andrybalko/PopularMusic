using Microsoft.EntityFrameworkCore;
using PopularMusic.com.popularmusic.model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PopularMusic.com.popularmusic.db
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }

        public DbSet<Image> Images { get; set; }
        public DbSet<AlbumImage> AlbumImages { get; set; }


        private string _databasePath;

        public DatabaseContext(string databasePath)
        {
            _databasePath = databasePath;   
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>()
                .HasMany(b => b.Image).WithOne().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Album>()
                .HasMany(b => b.Image).WithOne().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Image>()
            .HasOne<Artist>()
            .WithMany();

            modelBuilder.Entity<AlbumImage>()
            .HasOne<Album>()
            .WithMany();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databasePath}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace FreeWheel.Models
{
    public class FreeWheelContext : DbContext
    {
        public FreeWheelContext(DbContextOptions<FreeWheelContext> options)
            : base(options)
        { }

        public DbSet<Movies> Movies { get; set; }
        public DbSet<Ratings> Ratings { get; set; }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<MoviesGenres> MoviesGenres { get; set; }
        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Map N2N relation for movies and genres
            modelBuilder.Entity<MoviesGenres>()
                .HasKey(t => new { t.GenresId, t.MoviesId });
        }
    }
}

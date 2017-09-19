using Microsoft.EntityFrameworkCore;
using FLVisualization.Models.Entities;

namespace FLVisualization.DAL.EF
{
    public class FLVisualizationContext : DbContext
    {
        private const string connectionString = @"Server=(localdb)\mssqllocaldb;Database=FLVisualization;Trusted_Connection=True;MultipleActiveResultSets=true;";

        public FLVisualizationContext()
        {
        }

        public FLVisualizationContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
            }
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerHistory> History { get; set; }
    }
}

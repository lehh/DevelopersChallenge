using Microsoft.EntityFrameworkCore;

namespace ChampionshipManager.Model
{
    public class ChampionshipManagerContext : DbContext
    {
        public ChampionshipManagerContext(DbContextOptions<ChampionshipManagerContext> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Mapping Team Championship 
            //Primary keys
            modelBuilder.Entity<TeamChampionship>()
                .HasKey(tc => new { tc.ChampionshipId, tc.TeamId });

            //Relationship
            modelBuilder.Entity<TeamChampionship>()
                .HasOne(tc => tc.Team)
                .WithMany(tc => tc.TeamChampionships)
                .HasForeignKey(tc => tc.TeamId);
            modelBuilder.Entity<TeamChampionship>()
                .HasOne(tc => tc.Championship)
                .WithMany(tc => tc.TeamsChampionship)
                .HasForeignKey(tc => tc.ChampionshipId);
        }

        public DbSet<Championship> Championship { get; set; }

        public DbSet<Team> Team { get; set; }

        public DbSet<TeamChampionship> TeamChampionship { get; set; }
    }
}
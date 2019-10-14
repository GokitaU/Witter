using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witter.Models;

namespace Witter.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {   }

        public DbSet<User> Users { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<UserInLeague> UsersInLeagues { get; set; }

        public async Task<bool> Commit()
        {
            return await SaveChangesAsync() > 0;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>().HasKey(m => m.Id);
            modelBuilder.Entity<Team>().HasKey(t => t.Id);
            modelBuilder.Entity<Score>().HasKey(s => s.MatchId);
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<UserInLeague>().HasKey(l => new { l.UserId, l.LeagueId });

            modelBuilder.Entity<UserInLeague>()
                .HasOne(x => x.User)
                .WithMany(y => y.Leagues)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserInLeague>()
                .HasOne(x => x.League)
                .WithMany(y => y.Users)
                .HasForeignKey(x => x.LeagueId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}

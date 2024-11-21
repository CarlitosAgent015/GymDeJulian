using Microsoft.EntityFrameworkCore;
using Gym.noPainNoGain.Models;

namespace Gym.noPainNoGain.Data
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Routine> Routines { get; set; }
        public DbSet<Exercise> Exercises { get; set; }

    }
}
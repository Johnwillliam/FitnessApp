using EntityFramework.Config;
using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Context
{
    public class FitnessAppContext : DbContext
    {
        public DbSet<FitnessExercise> FitnessExercises { get; set; }
        public DbSet<ExerciseProgress> ExcerciseProgresses { get; set; }
        public DbSet<ExerciseDescription> ExerciseDescriptions { get; set; }
        public DbSet<FitnessProgram> FitnessPrograms { get; set; }
        public DbSet<UserProgress> UserProgresses { get; set; }
        public DbSet<FitnessWorkout> FitnessWorkout { get; set; }
        public DbSet<WorkoutProgress> WorkoutProgresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationSettings.DefaultConnectionStrings);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

using EntityFramework.Config;
using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Context
{
    public class FitnessAppContext : DbContext
    {
        public DbSet<FitnessProgram> FitnessPrograms { get; set; }
        public DbSet<FitnessWorkout> FitnessWorkouts { get; set; }
        public DbSet<FitnessExercise> FitnessExercises { get; set; }
        public DbSet<UserProgress> UserProgresses { get; set; }
        public DbSet<UserProgressWorkout> UserProgressWorkouts { get; set; }
        public DbSet<UserProgressWorkoutExercise> UserProgressWorkoutExercises { get; set; }
        public DbSet<UserProgressWorkoutExerciseSet> UserProgressWorkoutExerciseSets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FavoriteExercise> FavoriteExercises { get; set; }
        public DbSet<ExerciseDescription> ExerciseDescriptions { get; set; }

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

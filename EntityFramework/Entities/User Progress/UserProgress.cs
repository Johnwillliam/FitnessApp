using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework.Entities
{
    public class UserProgress
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public List<UserProgressWorkout> Workouts { get; set; }

        public UserProgress(FitnessProgram fitnessProgram, User user)
        {
            Workouts = fitnessProgram.Workouts
            .Select(w => new UserProgressWorkout
            {
                Week = w.Week,
                Day = w.Day,
                UserProgressWorkoutExercises = w.Exercises.Select(e => new UserProgressWorkoutExercise
                (
                    e.ExcerciseId,
                    e.Reps,
                    e.Sets,
                    e.RPE,
                    string.Empty
                )).ToList()
            })
            .ToList();
            Name = $"{fitnessProgram.Id} - {fitnessProgram.Name} - {DateTime.Now:dd-MM-yyyy HH:mm:ss}";
            UserId = user.Id;
        }

        public UserProgress()
        {
        }
    }
}

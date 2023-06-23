using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework.Entities
{
    public class UserProgress
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<WorkoutProgress> Workouts { get; set; }

        public UserProgress(FitnessProgram fitnessProgram)
        {
            Workouts = fitnessProgram.Workouts
            .Select(w => new WorkoutProgress
            {
                Week = w.Week,
                Day = w.Day,
                Exercises = w.Exercises.Select(e => new ExerciseProgress
                {
                    ExcerciseId = e.ExcerciseId,
                    Reps = e.Reps,
                    Sets = e.Sets,
                    RPE = e.RPE,
                    RepsDone = 0,
                    SetsDone = 0,
                    WeightDone = 0,
                    Comment = ""
                }).ToList()
            })
            .ToList();
            Name = $"{fitnessProgram.Id} - {fitnessProgram.Name} - {DateTime.Now}";
        }

        public UserProgress()
        {
        }
    }
}

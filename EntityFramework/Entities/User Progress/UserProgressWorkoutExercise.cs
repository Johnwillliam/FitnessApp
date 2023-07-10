using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework.Entities
{
    public class UserProgressWorkoutExercise
    {
        public UserProgressWorkoutExercise() { }

        public UserProgressWorkoutExercise(int? excerciseId, int reps, int sets, int rpe, string comment)
        {
            ExcerciseId = excerciseId;
            Reps = reps;
            Sets = sets;
            RPE = rpe;
            Comment = comment;
            for (int i = 0; i < Sets; i++)
            {
                UserProgressWorkoutExerciseSets.Add(new UserProgressWorkoutExerciseSet(i));
            }
        }

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? ExcerciseId { get; set; }
        public ExerciseDescription? Excercise { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
        public int RPE { get; set; }
        public string Comment { get; set; }
        public List<UserProgressWorkoutExerciseSet> UserProgressWorkoutExerciseSets { get; set; } = new List<UserProgressWorkoutExerciseSet>();

        //parent
        public UserProgressWorkout UserProgressWorkout { get; set; }
    }
}

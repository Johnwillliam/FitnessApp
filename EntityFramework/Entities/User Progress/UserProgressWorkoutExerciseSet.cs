using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework.Entities
{
    public class UserProgressWorkoutExerciseSet
    {
        public UserProgressWorkoutExerciseSet() { }

        public UserProgressWorkoutExerciseSet(int setNumber)
        {
            SetNumber = setNumber;
        }

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SetNumber { get; set; }
        public int RepsDone { get; set; }
        public double WeightDone { get; set; }
        public bool IsCompleted { get; set; }

        //parent
        public UserProgressWorkoutExercise UserProgressWorkoutExercise { get; set; }
    }
}

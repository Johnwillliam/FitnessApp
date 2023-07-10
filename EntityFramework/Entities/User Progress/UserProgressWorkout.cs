using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework.Entities
{
    public class UserProgressWorkout
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool Skipped { get; set; }
        public DateTime? Date { get; set; }
        public int Week { get; set; }
        public int Day { get; set; }
        public bool IsCompleted { get; set; }
        public List<UserProgressWorkoutExercise> UserProgressWorkoutExercises { get; set; }

        //parent
        public UserProgress UserProgress { get; set; }

        public UserProgressWorkout()
        {
            UserProgressWorkoutExercises = new List<UserProgressWorkoutExercise>();
        }
    }
}

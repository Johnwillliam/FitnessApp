using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework.Entities
{
    public class FitnessWorkout
    {
        public FitnessWorkout()
        {
            Exercises = new List<FitnessExercise>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Week { get; set; }
        public int Day { get; set; }
        public List<FitnessExercise> Exercises { get; set; }
    }
}

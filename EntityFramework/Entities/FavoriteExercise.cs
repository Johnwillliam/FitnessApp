using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework.Entities
{
    public class FavoriteExercise
    {
        public FavoriteExercise()
        {
        }

        public FavoriteExercise(ExerciseDescription exerciseDescription)
        {
            ExerciseDescriptionId = exerciseDescription.Id;
        }

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ExerciseDescriptionId { get; set; }
        public ExerciseDescription ExerciseDescription { get; set; }
    }
}

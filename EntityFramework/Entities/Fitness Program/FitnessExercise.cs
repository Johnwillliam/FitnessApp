using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework.Entities
{
    public class FitnessExercise
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? ExcerciseId { get; set; }
        public ExerciseDescription? Excercise { get; set; }
        public string? Description => Excercise == null ? "Not Configured" : Excercise.Name;
        public int Reps { get; set; }
        public int Sets { get; set; }
        public int RPE { get; set; }
    }
}

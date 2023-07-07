using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework.Entities
{
    public class ExerciseProgress
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? ExcerciseId { get; set; }
        public ExerciseDescription? Excercise { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
        public int RPE { get; set; }
        public int RepsDone { get; set; }
        public int SetsDone { get; set; }
        public double WeightDone { get; set; }
        public string Comment { get; set; }

        //parent
        public WorkoutProgress WorkoutProgress { get; set; }
    }
}

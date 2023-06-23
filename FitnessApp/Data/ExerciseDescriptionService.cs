using EntityFramework.Context;
using EntityFramework.Entities;

namespace FitnessApp.Data
{
    public class ExerciseDescriptionService
    {
        public Task<List<ExerciseDescription>> GetExercises()
        {
            return Task.FromResult(new FitnessAppContext().ExerciseDescriptions.ToList());
        }

        public Task<ExerciseDescription> AddExercise(ExerciseDescription exercise)
        {
            var context = new FitnessAppContext();
            context.ExerciseDescriptions.Add(exercise);
            context.SaveChanges();
            return Task.FromResult(exercise);
        }
    }

}

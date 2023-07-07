using EntityFramework.Context;
using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Data
{
    public class ExerciseDescriptionService
    {
        public Task<List<ExerciseDescription>> GetAll()
        {
            return Task.FromResult(new FitnessAppContext().ExerciseDescriptions.ToList());
        }

        public Task<int> GetExercisesCount()
        {
            return Task.FromResult(new FitnessAppContext().ExerciseDescriptions.CountAsync().Result);
        }

        public Task<List<ExerciseDescription>> GetFilteredExercises(int start, int amountOfItems)
        {
            return Task.FromResult(new FitnessAppContext().ExerciseDescriptions.Skip(start).Take(amountOfItems).ToList());
        }

        public Task<ExerciseDescription> Add(ExerciseDescription exercise)
        {
            var context = new FitnessAppContext();
            context.ExerciseDescriptions.Add(exercise);
            context.SaveChanges();
            return Task.FromResult(exercise);
        }
    }

}

using EntityFramework.Context;
using EntityFramework.Entities;
using Logic;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Data
{
    public class FavoriteExerciseService
    {
        public Task<List<FavoriteExercise>> GetByUser(User user)
        {
            var favoriteExercises = new FitnessAppContext().FavoriteExercises.Include(x => x.ExerciseDescription).Where(x => x.UserId == user.Id).ToList();
            favoriteExercises.AsParallel().ForAll(FavoriteExercisesLogic.CalculateMaxWeight);
            return Task.FromResult(favoriteExercises);
        }

        public void Remove(FavoriteExercise favorite)
        {
            var context = new FitnessAppContext();
            context.FavoriteExercises.Remove(favorite);
            context.SaveChanges();
        }
    }
}

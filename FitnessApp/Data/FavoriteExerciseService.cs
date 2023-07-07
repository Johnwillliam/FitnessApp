using EntityFramework.Context;
using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Data
{
    public class FavoriteExerciseService
    {
        public Task<List<FavoriteExercise>> GetByUser(User user)
        {
            return Task.FromResult(new FitnessAppContext().FavoriteExercises.Include(x => x.ExerciseDescription).Where(x => x.UserId == user.Id).ToList());
        }

        public void Remove(FavoriteExercise favorite)
        {
            var context = new FitnessAppContext();
            context.FavoriteExercises.Remove(favorite);
            context.SaveChanges();
        }
    }
}

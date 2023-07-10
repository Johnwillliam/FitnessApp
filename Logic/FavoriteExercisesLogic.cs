using EntityFramework.Context;
using EntityFramework.Entities;

namespace Logic
{
    public static class FavoriteExercisesLogic
    {
        public static void CalculateMaxWeight(FavoriteExercise favoriteExercise)
        {
            var context = new FitnessAppContext();
            var doneExercises = context.UserProgressWorkoutExercises.Where(x => x.UserProgressWorkout.UserProgress.UserId == favoriteExercise.User.Id && x.ExcerciseId == favoriteExercise.ExerciseDescriptionId);
            var weightsDone = doneExercises.SelectMany(x => x.UserProgressWorkoutExerciseSets.Select(x => x.WeightDone));
            favoriteExercise.ExerciseDescription.MaxWeightUsed = doneExercises.Any() ? weightsDone.Max() : 0;
        }
    }
}
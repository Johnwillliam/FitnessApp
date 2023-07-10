using EntityFramework.Context;
using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Data
{
    public class UserProgressService
    {
        public async Task<UserProgress?> GetById(int id)
        {
            return await Task.FromResult(new FitnessAppContext().UserProgresses.Include(z => z.User)
                .Include(x => x.Workouts)
                .ThenInclude(z => z.UserProgressWorkoutExercises)
                .ThenInclude(q => q.UserProgressWorkoutExerciseSets)
                .Include(z => z.Workouts)
                .ThenInclude(z => z.UserProgressWorkoutExercises)
                .ThenInclude(q => q.Excercise).FirstOrDefault(x => x.Id == id));
        }

        public async Task<List<UserProgress>> GetByUser(User user)
        {
            return user == null ? await Task.FromResult(new List<UserProgress>()) : await Task.FromResult(new FitnessAppContext().UserProgresses
                .Include(z => z.User).Where(x => x.User.Id == user.Id)
                .Include(x => x.Workouts)
                    .ThenInclude(z => z.UserProgressWorkoutExercises)
                    .ThenInclude(q => q.UserProgressWorkoutExerciseSets)
                .Include(z => z.Workouts)
                    .ThenInclude(z => z.UserProgressWorkoutExercises)
                    .ThenInclude(q => q.Excercise)
                .ToList());
        }

        public async Task<List<UserProgress>> GetUserProgresses()
        {
            return await Task.FromResult(new FitnessAppContext().UserProgresses.Include(z => z.User)
                .Include(x => x.Workouts)
                    .ThenInclude(z => z.UserProgressWorkoutExercises)
                    .ThenInclude(q => q.UserProgressWorkoutExerciseSets)
                .Include(z => z.Workouts)
                    .ThenInclude(z => z.UserProgressWorkoutExercises)
                    .ThenInclude(q => q.Excercise)
                .ToList());
        }

        public async Task Delete(UserProgress userProgress)
        {
            var context = new FitnessAppContext();
            context.UserProgresses.Remove(userProgress);
            context.SaveChanges();
            await Task.CompletedTask;
        }

        public async Task<UserProgressWorkoutExercise?> GetPrevious(UserProgressWorkoutExercise exerciseProgress)
        {
            var context = new FitnessAppContext();
            return await Task.FromResult(context.UserProgressWorkoutExercises.OrderByDescending(x => x.UserProgressWorkout.Date).FirstOrDefault(x => x.UserProgressWorkout.UserProgress.UserId == exerciseProgress.UserProgressWorkout.UserProgress.UserId
                                                                                && x.ExcerciseId == exerciseProgress.ExcerciseId
                                                                                && (exerciseProgress.UserProgressWorkout.Date == null || x.UserProgressWorkout.Date < exerciseProgress.UserProgressWorkout.Date)));
        }

        public async Task<int> Update(UserProgress userProgress)
        {
            var context = new FitnessAppContext();
            var existingUserProgress = await GetById(userProgress.Id);
            if(existingUserProgress != null)
            {
                context.UserProgresses.Update(userProgress);
                return await context.SaveChangesAsync();
            }
            else
            {
                context.UserProgresses.Add(userProgress);
                return await context.SaveChangesAsync();
            }
        }
    }
}

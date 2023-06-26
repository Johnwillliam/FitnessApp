using EntityFramework.Context;
using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Data
{
    public class UserProgressService
    {
        public async Task<UserProgress> GetUserProgress(int id)
        {
            return await Task.FromResult(new FitnessAppContext().UserProgresses.Include(z => z.User).Include(x => x.Workouts).ThenInclude(z => z.Exercises).ThenInclude(y => y.Excercise).FirstOrDefault(x => x.Id == id));
        }

        public async Task<List<UserProgress>> GetUserProgresses(User user)
        {
            return user == null ? await Task.FromResult(new List<UserProgress>()) : await Task.FromResult(new FitnessAppContext().UserProgresses.Include(z => z.User).Where(x => x.User.Id == user.Id).Include(x => x.Workouts).ThenInclude(z => z.Exercises).ThenInclude(y => y.Excercise).ToList());
        }

        public async Task<List<UserProgress>> GetUserProgresses()
        {
            return await Task.FromResult(new FitnessAppContext().UserProgresses.Include(z => z.User).Include(x => x.Workouts).ThenInclude(z => z.Exercises).ThenInclude(y => y.Excercise).ToList());
        }

        public async Task UpdateUserProgress(UserProgress userProgress)
        {
            var existingUserProgress = await GetUserProgress(userProgress.Id);
            if (existingUserProgress != null)
            {
                foreach (var workout in userProgress.Workouts)
                {
                    var existingWorkout = existingUserProgress.Workouts.FirstOrDefault(w => w.Week == workout.Week && w.Day == workout.Day);
                    if (existingWorkout != null)
                    {
                        foreach (var exercise in workout.Exercises)
                        {
                            var existingExercise = existingWorkout.Exercises.FirstOrDefault(e => e.ExcerciseId == exercise.ExcerciseId);
                            if (existingExercise != null)
                            {
                                existingExercise.RepsDone = exercise.RepsDone;
                                existingExercise.SetsDone = exercise.SetsDone;
                                existingExercise.WeightDone = exercise.WeightDone;
                                existingExercise.Comment = exercise.Comment;
                            }
                            else
                            {
                                existingWorkout.Exercises.Add(new ExerciseProgress
                                {
                                    ExcerciseId = exercise.ExcerciseId,
                                    RepsDone = exercise.RepsDone,
                                    SetsDone = exercise.SetsDone,
                                    WeightDone = exercise.WeightDone,
                                    Comment = exercise.Comment
                                });
                            }
                        }
                    }
                    else
                    {
                        existingUserProgress.Workouts.Add(new WorkoutProgress
                        {
                            Week = workout.Week,
                            Day = workout.Day,
                            Exercises = workout.Exercises.Select(e => new ExerciseProgress
                            {
                                ExcerciseId = e.ExcerciseId,
                                RepsDone = e.RepsDone,
                                SetsDone = e.SetsDone,
                                WeightDone = e.WeightDone,
                                Comment = e.Comment
                            }).ToList()
                        });
                    }
                }
                var context = new FitnessAppContext();
                context.UserProgresses.Update(userProgress);
                context.SaveChanges();
            }
            else
            {
                var context = new FitnessAppContext();
                context.UserProgresses.Add(userProgress);
                context.SaveChanges();
            }
        }
    }
}

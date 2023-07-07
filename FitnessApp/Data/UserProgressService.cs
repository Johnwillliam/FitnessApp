using EntityFramework.Context;
using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Data
{
    public class UserProgressService
    {
        public async Task<UserProgress> GetById(int id)
        {
            return await Task.FromResult(new FitnessAppContext().UserProgresses.Include(z => z.User).Include(x => x.Workouts).ThenInclude(z => z.ExerciseProgresses).ThenInclude(y => y.Excercise).FirstOrDefault(x => x.Id == id));
        }

        public async Task<List<UserProgress>> GetByUser(User user)
        {
            return user == null ? await Task.FromResult(new List<UserProgress>()) : await Task.FromResult(new FitnessAppContext().UserProgresses.Include(z => z.User).Where(x => x.User.Id == user.Id).Include(x => x.Workouts).ThenInclude(z => z.ExerciseProgresses).ThenInclude(y => y.Excercise).ToList());
        }

        public async Task<List<UserProgress>> GetUserProgresses()
        {
            return await Task.FromResult(new FitnessAppContext().UserProgresses.Include(z => z.User).Include(x => x.Workouts).ThenInclude(z => z.ExerciseProgresses).ThenInclude(y => y.Excercise).ToList());
        }

        public async Task Delete(UserProgress userProgress)
        {
            var context = new FitnessAppContext();
            context.UserProgresses.Remove(userProgress);
            context.SaveChanges();
            await Task.CompletedTask;
        }

        public async Task<ExerciseProgress?> GetPrevious(ExerciseProgress exerciseProgress)
        {
            var context = new FitnessAppContext();
            return await Task.FromResult(context.ExcerciseProgresses.OrderByDescending(x => x.WorkoutProgress.Date).FirstOrDefault(x => x.WorkoutProgress.UserProgress.UserId == exerciseProgress.WorkoutProgress.UserProgress.UserId
                                                                                && x.ExcerciseId == exerciseProgress.ExcerciseId
                                                                                && (exerciseProgress.WorkoutProgress.Date == null || x.WorkoutProgress.Date < exerciseProgress.WorkoutProgress.Date)));
        }

        public async Task Update(UserProgress userProgress)
        {
            var existingUserProgress = await GetById(userProgress.Id);
            var context = new FitnessAppContext();
            if (existingUserProgress != null)
            {
                foreach (var workout in userProgress.Workouts)
                {
                    var existingWorkout = existingUserProgress.Workouts.FirstOrDefault(w => w.Week == workout.Week && w.Day == workout.Day);
                    if (existingWorkout != null)
                    {
                        foreach (var exercise in workout.ExerciseProgresses)
                        {
                            var existingExercise = existingWorkout.ExerciseProgresses.FirstOrDefault(e => e.ExcerciseId == exercise.ExcerciseId);
                            if (existingExercise != null)
                            {
                                existingExercise.RepsDone = exercise.RepsDone;
                                existingExercise.SetsDone = exercise.SetsDone;
                                existingExercise.WeightDone = exercise.WeightDone;
                                existingExercise.Comment = exercise.Comment;
                            }
                            else
                            {
                                existingWorkout.ExerciseProgresses.Add(new ExerciseProgress
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
                            ExerciseProgresses = workout.ExerciseProgresses.Select(e => new ExerciseProgress
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
                context.UserProgresses.Update(userProgress);
                context.SaveChanges();
            }
            else
            {
                context.UserProgresses.Add(userProgress);
                context.SaveChanges();
            }
        }
    }
}

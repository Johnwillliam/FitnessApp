using Microsoft.AspNetCore.Components;
using FitnessApp.Shared;
using EntityFramework.Entities;
using FitnessApp.Data;

namespace FitnessApp.Pages
{
    public partial class Index
    {
        private int _selectedWeek;
        private User? _user;
        private UserProgress? _userProgress;
        private List<UserProgress> _userProgresses = new();
        private List<UserProgressWorkout> _workouts = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _user = await Sesssion.GetUser(NavMenu.UserKey);
                await OnInitializedAsync();
                StateHasChanged();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if (_user != null)
            {
                _userProgresses = await UserProgressService.GetByUser(_user);
            }
        }

        private void SelectedUserProgress(ChangeEventArgs e)
        {
            var value = e.Value?.ToString();
            if(string.IsNullOrEmpty(value))
            {
                return;
            }

            var userProgresId = int.Parse(value);
            _userProgress = UserProgressService.GetById(userProgresId).Result;
            if(_userProgress != null)
            {
                var week = _userProgress.Workouts.OrderBy(x => x.Week).First(x => !x.Date.HasValue && !x.Skipped).Week;
                _selectedWeek = week;
                GetWorkouts(week);
            }
        }

        private void WeekChanged(ChangeEventArgs e)
        {
            GetWorkouts(int.TryParse(e.Value?.ToString(), out var week) ? week : null);
        }

        private void GetWorkouts(int? week)
        {
            if (_userProgress != null)
            {
                _workouts = week.HasValue ? _userProgress.Workouts.Where(x => x.Week == week).ToList() : new List<UserProgressWorkout>();
                StateHasChanged();
            }
        }

        private async Task Save()
        {
            if (_userProgress != null)
            {
                await UserProgressService.Update(_userProgress);
            }

            NavigationManager.NavigateTo("/");
        }

        private void Cancel()
        {
            _userProgress = null;
            NavigationManager.NavigateTo("/");
        }

        private async Task<UserProgressWorkoutExercise?> GetPreviousExerciseData(UserProgressWorkoutExercise exerciseProgress)
        {
            return await UserProgressService.GetPrevious(exerciseProgress);
        }

        private void ToggleSetIsCompleted(UserProgressWorkoutExerciseSet userProgressWorkoutExerciseSet)
        {
            userProgressWorkoutExerciseSet.IsCompleted = !userProgressWorkoutExerciseSet.IsCompleted;
        }

        private async Task CopySetTargets(UserProgressWorkoutExercise userProgressWorkoutExercise)
        {
            userProgressWorkoutExercise.UserProgressWorkoutExerciseSets.AsParallel().ForAll(x =>
            {
                x.RepsDone = x.UserProgressWorkoutExercise.Reps;
            });
            _ = Save();
            await Task.CompletedTask;
        }

        private async Task CopyFirstWeight(UserProgressWorkoutExercise userProgressWorkoutExercise)
        {
            var firstWeight = userProgressWorkoutExercise.UserProgressWorkoutExerciseSets.First()?.WeightDone;
            userProgressWorkoutExercise.UserProgressWorkoutExerciseSets.AsParallel().ForAll(x =>
            {
                x.WeightDone = firstWeight ?? 0;
            });
            _ = Save();
            await Task.CompletedTask;
        }

        private async Task AllSetsDone(UserProgressWorkoutExercise userProgressWorkoutExercise)
        {
            userProgressWorkoutExercise.UserProgressWorkoutExerciseSets.AsParallel().ForAll(x =>
            {
                x.IsCompleted = true;
            });
            _ = Save();
            await Task.CompletedTask;
        }

        private async Task AllSetsToggleDone(UserProgressWorkoutExercise userProgressWorkoutExercise)
        {
            var firstDone = userProgressWorkoutExercise.UserProgressWorkoutExerciseSets.First().IsCompleted;
            userProgressWorkoutExercise.UserProgressWorkoutExerciseSets.AsParallel().ForAll(x =>
            {
                x.IsCompleted = !firstDone;
            });
            _ = Save();
            await Task.CompletedTask;
        }
    }
}
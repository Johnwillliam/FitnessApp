using Microsoft.AspNetCore.Components;
using FitnessApp.Shared;
using EntityFramework.Entities;
using FitnessApp.Data;
using EntityFramework.Context;

namespace FitnessApp.Pages
{
    public partial class FitnessProgramPage
    {
        private List<ExerciseDescription> _exerciseDescriptions = new();
        private string _programName = string.Empty;
        private int _daysPerWeek;
        private int _numberOfWeeks;
        private List<FitnessWorkout> _workouts = new();
        private bool CanGenerateWorkouts => !string.IsNullOrEmpty(_programName) && _daysPerWeek > 0 && _numberOfWeeks > 0;
        private List<FitnessProgram> _fitnessPrograms = new();
        private FitnessProgram? _fitnessProgram;
        private User? _user;
        private readonly FitnessAppContext _context = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _user = await Sesssion.GetUser(NavMenu.UserKey);
                StateHasChanged();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            _fitnessPrograms = await FitnessProgramService.GetAll();
            _exerciseDescriptions = await ExerciseDescriptionService.GetAll();
        }

        private async Task SelectFitnessProgram(ChangeEventArgs e)
        {
            var value = e.Value?.ToString();
            if (string.IsNullOrEmpty(value))
            {
                _fitnessProgram = null;
                _workouts = new List<FitnessWorkout>();
                StateHasChanged();
                return;
            }
            var fitnessProgramId = int.Parse(value);
            _fitnessProgram = await FitnessProgramService.GetById(fitnessProgramId);
            _workouts = _fitnessProgram.Workouts;
            StateHasChanged();
        }

        private async void GenerateWorkouts()
        {
            if (await FitnessProgramService.NameExists(_programName) || _user == null)
            {
                return;
            }

            _workouts = new List<FitnessWorkout>();
            for (int week = 1; week <= _numberOfWeeks; week++)
            {
                for (int day = 1; day <= _daysPerWeek; day++)
                {
                    var workout = new FitnessWorkout
                    {
                        Week = week,
                        Day = day,
                        Exercises = new List<FitnessExercise>()
                    };
                    _workouts.Add(workout);
                }
            }

            await Save();
        }

        private void AddExercise(FitnessWorkout workout)
        {
            if (_fitnessProgram == null)
            {
                return;
            }

            _context.FitnessExercises.Add(new FitnessExercise());
            workout.Exercises.Add(new FitnessExercise());
            _fitnessProgram.Workouts = _workouts;
        }

        private void RemoveExercise(FitnessWorkout workout, FitnessExercise exercise)
        {
            if (_fitnessProgram == null)
            {
                return;
            }

            _context.FitnessExercises.Remove(exercise);
            workout.Exercises.Remove(exercise);
            _fitnessProgram.Workouts = _workouts;
        }

        private async Task UpdateFitnessProgram()
        {
            if (_fitnessProgram == null)
            {
                return;
            }

            _fitnessProgram.Workouts = _workouts;
            await FitnessProgramService.Update(_fitnessProgram);
            _context.SaveChanges();
        }

        private async Task Save()
        {
            if (_user == null)
            {
                return;
            }

            _fitnessProgram = new FitnessProgram
            {
                Name = _programName,
                Workouts = _workouts,
                UserId = _user.Id
            };
            await FitnessProgramService.Save(_fitnessProgram);
            await OnInitializedAsync();
            StateHasChanged();
        }

        private async Task Delete()
        {
            if (_fitnessProgram != null)
            {
                await FitnessProgramService.Delete(_fitnessProgram);
            }

            NavigationManager.NavigateTo("/fitnessprogram", true);
        }

        private void Cancel()
        {
            NavigationManager.NavigateTo("/fitnessprogram", true);
        }
    }
}
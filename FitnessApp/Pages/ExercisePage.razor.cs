using Microsoft.AspNetCore.Components;
using FitnessApp.Shared;
using EntityFramework.Context;
using EntityFramework.Entities;
using FitnessApp.Data;

namespace FitnessApp.Pages
{
    public partial class ExercisePage
    {
        private List<ExerciseDescription> _filteredExerciseDescriptions = new List<ExerciseDescription>();
        private string _exerciseName = string.Empty;
        private User? _user;
        private static readonly int _amountOfItemsPerPage = 10;
        private static readonly int _amountOfNextPages = 3;
        private int _startDisplayPageNumber;
        private int _allExerciseDescriptionsCount = 0;
        private int AmountOfPages => _allExerciseDescriptionsCount / _amountOfItemsPerPage;

        [Parameter]
        public int CurrentPage { get; set; } = 1;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _allExerciseDescriptionsCount = await ExerciseService.GetExercisesCount();
                _user = await Sesssion.GetUser(NavMenu.UserKey);
                await OnInitializedAsync();
                StateHasChanged();
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await LoadExerciseData();
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadExerciseData();
        }

        private async Task LoadExerciseData()
        {
            if (_user == null)
            {
                return;
            }

            if (CurrentPage == 0)
            {
                CurrentPage = 1;
            }

            _filteredExerciseDescriptions = await ExerciseService.GetFilteredExercises((CurrentPage - 1) * _amountOfItemsPerPage, _amountOfItemsPerPage);
            _user.FavoriteExercises = await FavoriteExerciseService.GetByUser(_user);
        }

        public async Task Favorite(ExerciseDescription exercise)
        {
            if (_user == null)
            {
                return;
            }

            _user.FavoriteExercises.Add(new FavoriteExercise(exercise));
            await UserService.Update(_user);
            await OnInitializedAsync();
        }

        public async Task Unfavorite(ExerciseDescription exercise)
        {
            if (_user == null)
            {
                return;
            }

            var favorite = _user.FavoriteExercises.First(x => x.ExerciseDescriptionId == exercise.Id);
            FavoriteExerciseService.Remove(favorite);
            await OnInitializedAsync();
        }

        private async Task CreateExercise()
        {
            if (!string.IsNullOrEmpty(_exerciseName))
            {
                var exercise = new ExerciseDescription
                {
                    Name = _exerciseName
                };
                await ExerciseService.Add(exercise);
                _exerciseName = string.Empty;
                NavigationManager.NavigateTo("/exercises");
            }
        }
    }
}
using Microsoft.AspNetCore.Components;
using FitnessApp.Shared;
using EntityFramework.Entities;
using FitnessApp.Data;

namespace FitnessApp.Pages
{
    public partial class UserProgressPage
    {
        private List<ExerciseDescription> _exerciseDescriptions = new();
        private List<FitnessProgram> _fitnessPrograms = new();
        private List<UserProgress> _userProgresses = new();
        private int _selectedFitnessProgramId;
        private UserProgress? _userProgress;
        private User? _user;

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
            _fitnessPrograms = await FitnessProgramService.GetAll();
            _exerciseDescriptions = await ExerciseDescriptionService.GetAll();
            if(_user != null)
            {
                _userProgresses = await UserProgressService.GetByUser(_user);
            }
        }

        private async Task SelectUserProgress(ChangeEventArgs e)
        {
            var value = e.Value?.ToString();
            if(string.IsNullOrEmpty(value) )
            {
                return;
            }

            var userProgresId = int.Parse(value);
            _userProgress = await UserProgressService.GetById(userProgresId);
        }

        private async Task GenerateUserProgress()
        {
            if (_selectedFitnessProgramId == 0)
            {
                return;
            }

            var fitnessProgram = await FitnessProgramService.GetById(_selectedFitnessProgramId);
            if (fitnessProgram == null)
            {
                return;
            }

            _userProgress = _user == null ? null : new UserProgress(fitnessProgram, _user);
            if (_userProgress == null)
            {
                return;
            }

            await Save();
            await OnInitializedAsync();
        }

        private async Task Save()
        {
            if (_userProgress != null)
            {
                await UserProgressService.Update(_userProgress);
            }

            NavigationManager.NavigateTo("/userprogress");
        }

        private async Task Delete()
        {
            if (_userProgress != null)
            {
                await UserProgressService.Delete(_userProgress);
            }

            NavigationManager.NavigateTo("/userprogress", true);
        }

        private void Cancel()
        {
            NavigationManager.NavigateTo("/userprogress", true);
        }
    }
}
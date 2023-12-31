using FitnessApp.Shared;
using EntityFramework.Context;
using EntityFramework.Entities;
using FitnessApp.Data;

namespace FitnessApp.Pages
{
    public partial class AccountPage
    {
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
            await base.OnInitializedAsync();
            if (_user == null)
            {
                return;
            }

            _user.FavoriteExercises = await FavoriteExerciseService.GetByUser(_user);
            
        }

        public async Task Logout()
        {
            _user = null;
            await Sesssion.Delete(NavMenu.UserKey);
        }
    }
}
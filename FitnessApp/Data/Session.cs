using Blazored.LocalStorage;
using EntityFramework.Entities;

namespace FitnessApp.Data
{
    public class Session
    {
        protected readonly ILocalStorageService _localStorage;

        public Session(ILocalStorageService LocalStorage)
        {
            _localStorage = LocalStorage;
        }

        public async Task Update(string key, object value)
        {
            await _localStorage.SetItemAsync(key, value);
            await Task.CompletedTask;
        }

        public async Task Delete(string key)
        {
            await _localStorage.RemoveItemAsync(key);
            await Task.CompletedTask;
        }

        public async Task<string> GetString(string key)
        {
            return await _localStorage.GetItemAsStringAsync(key);
        }

        public async Task<User?> GetUser(string key)
        {
            return await _localStorage.ContainKeyAsync(key) ? await _localStorage.GetItemAsync<User?>(key) : null;
        }
    }
}

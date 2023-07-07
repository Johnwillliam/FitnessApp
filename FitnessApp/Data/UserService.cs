using EntityFramework.Context;
using EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Data
{
    public class UserService
    {
        public Task<User?> GetUser(string userName)
        {
            return Task.FromResult(new FitnessAppContext().Users.Include(x => x.FavoriteExercises).ThenInclude(z => z.ExerciseDescription).FirstOrDefault(x => x.UserName == userName));
        }

        public Task<bool> ValidPassword(string userName, string password)
        {
            var user = GetUser(userName).Result;
            return Task.FromResult(user != null && user.Password == password);
        }

        public Task<User> AddUser(User user)
        {
            var context = new FitnessAppContext();
            context.Users.Add(user);
            context.SaveChanges();
            return Task.FromResult(user);
        }

        public async Task Update(User user)
        {
            var context = new FitnessAppContext();
            context.Users.Update(user);
            context.SaveChanges();
            await Task.CompletedTask;
        }
    }
}

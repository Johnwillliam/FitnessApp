using EntityFramework.Entities;
using EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FitnessApp.Data
{
    public class FitnessProgramService
    {
        public async Task<bool> NameExists(string programName)
        {
            return await Task.FromResult(GetAll().Result.Any(x => x.Name == programName));
        }

        public async Task<FitnessProgram> GetById(int programId)
        {
            return await Task.FromResult(GetAll().Result.Find(x => x.Id == programId));
        }

        public async Task<List<FitnessProgram>> GetAll()
        {
            return await Task.FromResult(new FitnessAppContext().FitnessPrograms.Include(x => x.User).Include(x => x.Workouts).ThenInclude(z => z.Exercises).ThenInclude(y => y.Excercise).ToList());
        }

        public async Task Save(FitnessProgram fitnessProgram)
        {
            var context = new FitnessAppContext();
            context.FitnessPrograms.Add(fitnessProgram);
            context.SaveChanges();
            await Task.CompletedTask;
        }

        public async Task Update(FitnessProgram fitnessProgram)
        {
            var existingProgram = GetById(fitnessProgram.Id).Result;
            if (existingProgram != null)
            {
                existingProgram.Name = fitnessProgram.Name;
                existingProgram.Workouts = fitnessProgram.Workouts;
                var context = new FitnessAppContext();
                context.FitnessPrograms.Update(existingProgram);
                context.SaveChanges();
            }
            else
            {
                await Save(fitnessProgram);
            }

            await Task.CompletedTask;
        }

        public async Task DeleteFitnessProgram(int programId)
        {
            var fitnessProgram = new FitnessAppContext().FitnessPrograms.Find(programId);
            if (fitnessProgram != null)
            {
                var context = new FitnessAppContext();
                context.FitnessPrograms.Remove(fitnessProgram);
                context.SaveChanges();
            }
            await Task.CompletedTask;
        }

        public async Task Delete(FitnessProgram fitnessProgram)
        {
            var context = new FitnessAppContext();
            context.FitnessPrograms.Remove(fitnessProgram);
            context.SaveChanges();
            await Task.CompletedTask;
        }
    }
}
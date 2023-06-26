using EntityFramework.Entities;
using EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

public class FitnessProgramService
{
    public async Task<bool> NameExists(string programName)
    {
        return await Task.FromResult(GetFitnessPrograms().Result.Any(x => x.Name == programName));
    }

    public async Task<FitnessProgram> GetFitnessProgram(int programId)
    {
        return await Task.FromResult(GetFitnessPrograms().Result.Find(x => x.Id == programId));
    }

    public async Task<List<FitnessProgram>> GetFitnessPrograms()
    {
        return await Task.FromResult(new FitnessAppContext().FitnessPrograms.Include(x => x.User).Include(x => x.Workouts).ThenInclude(z => z.Exercises).ThenInclude(y => y.Excercise).ToList());
    }

    public async Task SaveFitnessProgram(FitnessProgram fitnessProgram)
    {
        var context = new FitnessAppContext();
        context.FitnessPrograms.Add(fitnessProgram);
        context.SaveChanges();
        await Task.CompletedTask;
    }

    public async Task UpdateFitnessProgram(FitnessProgram fitnessProgram)
    {
        var existingProgram = GetFitnessProgram(fitnessProgram.Id).Result;
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
            await SaveFitnessProgram(fitnessProgram);
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
}

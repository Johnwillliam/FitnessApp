using EntityFramework.Context;
using EntityFramework.Entities;
using FitnessApp.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;

var context = new FitnessAppContext();
var databaseIsNew = !context.Database.GetService<IRelationalDatabaseCreator>().HasTables();
context.Database.EnsureCreated();

if(databaseIsNew)
{
    LoadExercises();
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<FitnessProgramService>();
builder.Services.AddSingleton<UserProgressService>();
builder.Services.AddSingleton<ExerciseDescriptionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();


void LoadExercises()
{
    // Get the current working directory
    string currentDirectory = Directory.GetCurrentDirectory();

    // Combine the current directory with the desired subdirectory name
    string subdirectoryPath = Path.Combine(currentDirectory, "Exercises");


    // Get all subdirectories in the root folder
    string[] exerciseFolders = Directory.GetDirectories(subdirectoryPath);
    var context = new FitnessAppContext();

    // Loop through each exercise folder
    foreach (string exerciseFolder in exerciseFolders)
    {
        // Get the path to the exercise.json file
        string exerciseJsonPath = Path.Combine(exerciseFolder, "exercise.json");

        // Check if the exercise.json file exists
        if (File.Exists(exerciseJsonPath))
        {
            try
            {
                // Read the contents of the exercise.json file
                string exerciseJson = File.ReadAllText(exerciseJsonPath);

                // Deserialize the JSON to an Exercise object
                var exercise = JsonConvert.DeserializeObject<ExerciseDescription>(exerciseJson);

                if(exercise != null && !context.ExerciseDescriptions.Any(x => x.Name == exercise.Name))
                {
                    context.ExerciseDescriptions.Add(exercise);
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during deserialization
                Console.WriteLine($"Error deserializing exercise.json in folder {exerciseFolder}: {ex.Message}");
            }
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine($"exercise.json not found in folder {exerciseFolder}");
        }
    }
}
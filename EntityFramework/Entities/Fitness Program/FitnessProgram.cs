﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework.Entities
{
    public class FitnessProgram
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<FitnessWorkout> Workouts { get; set; }
    }
}

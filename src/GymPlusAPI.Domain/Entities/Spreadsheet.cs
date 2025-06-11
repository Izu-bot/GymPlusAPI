using System;

namespace GymPlusAPI.Domain.Entities;

public class Spreadsheet
{
    public int Id { get; init; }
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;

    // Referencia para o usuário
    public Guid UserId { get; init; } // Fk
    public User? User { get; private set; }

    // Referencia para os treinos
    public List<Workout> Workouts { get; set; } = [];
    public List<CustomMuscleGroup> CustomMuscleGroups { get; set; } = [];

    // Contructor
    public Spreadsheet(string name, Guid userId)
    {
        Name = name;
        UserId = userId;
    }
}

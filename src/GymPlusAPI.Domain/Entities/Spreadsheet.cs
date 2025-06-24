using System;
using System.ComponentModel;

namespace GymPlusAPI.Domain.Entities;

public class Spreadsheet
{
    public int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;// Data do inicio da recorrência
    
    public bool IsRecurring { get; set; } = false; // Marca se é ou recorrente
    public List<DayOfWeek> DaysOfWeek { get; set; } = new(); // Dias da semana

    // Referencia para o usuário
    public Guid UserId { get; init; } // Fk
    public User? User { get; private set; }

    // Referencia para os treinos
    public List<Workout> Workouts { get; set; } = [];
    public List<CustomMuscleGroup> CustomMuscleGroups { get; set; } = [];
    public List<TrainingCompleted> TrainingCompleteds { get; set; } = [];

    // Contructor
    public Spreadsheet() {} // Para o EF Core

    public Spreadsheet(string name, string? description, bool isRecurring, List<DayOfWeek> dayOfWeeks, Guid userId)
    {
        Name = name;
        Description = description ?? string.Empty;
        IsRecurring = isRecurring;
        DaysOfWeek = dayOfWeeks;
        UserId = userId;
    }
}

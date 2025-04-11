using System;

namespace GymPlusAPI.Domain.Entities;

public class Spreadsheet
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Referencia para o usuário
    public Guid UserId { get; set; } // Fk
    public User User { get; set; } = new();

    // Referencia para os treinos
    public List<Workout> Workouts { get; set; } = [];

    // Contructor
    public Spreadsheet(string name, Guid userId)
    {
        Name = name;
        UserId = userId;
    }
}

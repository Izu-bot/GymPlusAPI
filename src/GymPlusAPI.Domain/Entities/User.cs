using System;

namespace GymPlusAPI.Domain.Entities;

public class User
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; init; } = string.Empty; // Admin, User, etc.

    // Constructor
    public User(string username, string password, string name, string role)
    {
        Username = username;
        Password = password;
        Name = name;
        Role = role;
    }

    // Faz comunicação com planilhas
    public List<Spreadsheet> Spreadsheets { get; init; } = []; // Usuario tem várias planilhas
    public ICollection<CustomMuscleGroup> CustomMuscleGroups { get; init; } = new List<CustomMuscleGroup>(); // Usuario tem varios grupos musculares
    public ICollection<RecurrentTraining> RecurrentTrainings { get; init; } = new List<RecurrentTraining>(); // Usuario tem varios treinos recorrente
}

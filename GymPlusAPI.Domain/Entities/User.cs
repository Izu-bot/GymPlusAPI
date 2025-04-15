using System;

namespace GymPlusAPI.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // Admin, User, etc.

    // Constructor
    public User(string username, string password, string name, string role)
    {
        Username = username;
        Password = password;
        Name = name;
        Role = role;
    }

    // Faz comunicação com planilhas
    public List<Spreadsheet> Spreadsheets { get; set; } = []; // Usuario tem várias planilhas
}

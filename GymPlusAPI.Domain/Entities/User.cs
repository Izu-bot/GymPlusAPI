using System;

namespace GymPlusAPI.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    // Faz comunicação com planilhas
    public int SpreadsheetId { get; set; }
    public Spreadsheet Spreadsheet { get; set; } = new();
}

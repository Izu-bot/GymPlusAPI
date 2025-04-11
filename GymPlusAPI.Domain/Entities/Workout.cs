using System;

namespace GymPlusAPI.Domain.Entities;

public class Workout
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Reps { get; set; }
    public int Series { get; set; }
    public int Weight { get; set; } // Peso em kg
    
    // Referencia para a planilha
    public int SpreadsheetId { get; set; } // Fk
    public Spreadsheet Spreadsheet { get; set; } = null!; // Referencia para a planilha
}

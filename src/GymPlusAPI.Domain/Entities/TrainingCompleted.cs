namespace GymPlusAPI.Domain.Entities;

public class TrainingCompleted
{
    public int Id { get; set; }
    public DateTime Date { get; set; } // Dia que o treino foi concluido
    public bool IsCompleted { get; set; }
    public int SpreadsheetId { get; set; }
    public Spreadsheet Spreadsheet { get; init; } = null!; // Referencia para a planilha
}
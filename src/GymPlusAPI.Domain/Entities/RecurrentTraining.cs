namespace GymPlusAPI.Domain.Entities;

public class RecurrentTraining
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime Date { get; init; } = DateTime.Now; // Dia que o treino foi concluido
    public bool IsCompleted { get; init; }
    public string Observations { get; init; }
    public int SpreadsheetId { get; init; }
    public Guid UserId { get; init; } // FK
    public Spreadsheet Spreadsheet { get; init; } = null!; // Referencia para a planilha
    public User User { get; init; } = null!; // Referencia para usuarios

    public RecurrentTraining(bool isCompleted, Guid userId ,int spreadsheetId, string? observations)
    {
        IsCompleted = isCompleted;
        UserId = userId;
        SpreadsheetId = spreadsheetId;
        Observations = observations ?? string.Empty;
    }
}
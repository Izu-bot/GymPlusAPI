using System.Text.Json.Serialization;

namespace GymPlusAPI.Domain.Entities;

public class CustomMuscleGroup
{
    public int Id { get; init; }
    public string Name { get; set; }
    public int BitValue { get; init; }


    public Guid UserId { get; init; } // FK
    [JsonIgnore]
    public User? User { get; private set; }

    public int? SpreadsheetId { get; init; } // FK
    [JsonIgnore]
    public Spreadsheet? Spreadsheet { get; init; }

    public CustomMuscleGroup(string name, int bitValue, Guid userId)
    {
        Name = name;
        BitValue = bitValue;
        UserId = userId;
    }
}
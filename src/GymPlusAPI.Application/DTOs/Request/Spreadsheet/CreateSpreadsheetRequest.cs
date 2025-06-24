using System;

namespace GymPlusAPI.Application.DTOs.Request.Spreadsheet;

public record CreateSpreadsheetRequest(
    string Name,
    string? Description,
    bool IsRecurring,
    List<DayOfWeek> DaysOfWeek
);

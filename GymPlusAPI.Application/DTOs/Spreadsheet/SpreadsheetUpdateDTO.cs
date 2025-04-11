using System;

namespace GymPlusAPI.Application.DTOs.Spreadsheet;

public record SpreadsheetUpdateDTO(
    int Id,
    string Name
);
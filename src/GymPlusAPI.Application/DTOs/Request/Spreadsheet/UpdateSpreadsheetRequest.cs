using System;

namespace GymPlusAPI.Application.DTOs.Request.Spreadsheet;

public record UpdateSpreadsheetRequest(
    int Id,
    string Name
);

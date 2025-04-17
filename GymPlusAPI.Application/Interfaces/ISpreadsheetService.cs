using System;
using GymPlusAPI.Application.DTOs.Request.Spreadsheet;
using GymPlusAPI.Application.DTOs.Response.Spreadsheet;

namespace GymPlusAPI.Application.Interfaces;

public interface ISpreadsheetService
{
    Task<SpreadsheetResponse> CreateAsync(CreateSpreadsheetRequest dto, Guid userId);
    Task UpdateAsync(UpdateSpreadsheetRequest dto, Guid userId);
    Task DeleteAsync(int spreadsheetId, Guid userId);
    Task<IEnumerable<SpreadsheetResponse>> GetAllAsync(Guid userId);
    Task<SpreadsheetResponse> GetByIdAsync(int spreadsheetId, Guid userId);
}

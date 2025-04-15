using System;
using GymPlusAPI.Application.DTOs.Spreadsheet;

namespace GymPlusAPI.Application.Interfaces;

public interface ISpreadsheetService
{
    Task<int> CreateAsync(SpreadsheetCreateDTO dto, Guid userId);
    Task UpdateAsync(SpreadsheetUpdateDTO dto, Guid userId);
    Task DeleteAync(int spreadsheetId, Guid userId);
    Task<IEnumerable<SpreadsheetViewDTO>> GetAllAsync(Guid userId);
    Task<SpreadsheetViewDTO> GetByIdAsync(int spreadsheetId, Guid userId);
}

using System;
using GymPlusAPI.Domain.Entities;

namespace GymPlusAPI.Domain.Interfaces;

public interface ISpreadsheetRepository
{
    Task<Spreadsheet?> GetSpreadsheetByIdAsync(int id, Guid userId);
    Task<IEnumerable<Spreadsheet>> GetSpreadsheetsByUserAsync(Guid userId);
    Task<IEnumerable<Spreadsheet>> TodaySpreadsheet(DayOfWeek dayOfWeek, Guid userId);
    Task AddAsync(Spreadsheet spreadsheet);
    Task UpdateAsync(Spreadsheet spreadsheet);
    Task DeleteAsync(Spreadsheet spreadsheet);
    
}

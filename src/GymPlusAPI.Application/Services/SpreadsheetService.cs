using System;
using GymPlusAPI.Application.DTOs.Request.Spreadsheet;
using GymPlusAPI.Application.DTOs.Response.Spreadsheet;
using GymPlusAPI.Application.DTOs.Response.Workout;
using GymPlusAPI.Application.Interfaces;
using GymPlusAPI.Domain.Entities;
using GymPlusAPI.Domain.Interfaces;

namespace GymPlusAPI.Application.Services;

public class SpreadsheetService : ISpreadsheetService
{
    private readonly ISpreadsheetRepository _spreadsheetRepository;
    // private readonly AppDbContext _context;
    public SpreadsheetService(ISpreadsheetRepository spreadsheetRepository)
    {
        _spreadsheetRepository = spreadsheetRepository;
        // _context = context;
    }

    public async Task<SpreadsheetResponse> CreateAsync(CreateSpreadsheetRequest dto, Guid userId)
    {
        // var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        // if (!userExists)
        //     throw new Exception("Usuário não encontrado.");
            
        var spreadsheet = new Spreadsheet(dto.Name, userId);

        await _spreadsheetRepository.AddAsync(spreadsheet);

        return new SpreadsheetResponse(
            spreadsheet.Id,
            spreadsheet.Name,
            spreadsheet.Workouts.Select(w => new WorkoutResponse(
                w.Id,
                w.Name,
                w.Reps,
                w.Series,
                w.Weight
            )).ToList()
        );
    }

    public async Task UpdateAsync(UpdateSpreadsheetRequest dto, Guid userId)
    {
        var spreadsheetToUpdate = await _spreadsheetRepository.GetSpreadsheetByIdAsync(dto.Id, userId) ?? throw new Exception("Planilha não encontrada.");

        spreadsheetToUpdate.Name = dto.Name;

        await _spreadsheetRepository.UpdateAsync(spreadsheetToUpdate);
    }

    public async Task DeleteAsync(int spreadsheetId, Guid userId)
    {
        var spreadsheetToDelete = await _spreadsheetRepository.GetSpreadsheetByIdAsync(spreadsheetId, userId) ?? throw new Exception("Planilha não encontrada.");

        await _spreadsheetRepository.DeleteAsync(spreadsheetToDelete);
    }

    public async Task<IEnumerable<SpreadsheetResponse>> GetAllAsync(Guid userId)
    {
        var spreadsheets = await _spreadsheetRepository.GetSpreadsheetsByUserAsync(userId);

        return spreadsheets.Select(s => new SpreadsheetResponse(
            s.Id,
            s.Name,
            s.Workouts.Select(w => new WorkoutResponse(
                w.Id,
                w.Name,
                w.Reps,
                w.Series,
                w.Weight
            )).ToList()
        ));
    }

    public async Task<SpreadsheetResponse> GetByIdAsync(int spreadsheetId, Guid userId)
    {        
        var spreadsheet = await _spreadsheetRepository.GetSpreadsheetByIdAsync(spreadsheetId, userId);
        if (spreadsheet == null)
            return null!;

        return new SpreadsheetResponse(
            spreadsheet.Id,
            spreadsheet.Name,
            spreadsheet.Workouts.Select(w => new WorkoutResponse(
                w.Id,
                w.Name,
                w.Reps,
                w.Series,
                w.Weight
            )).ToList()
        );
    }
}

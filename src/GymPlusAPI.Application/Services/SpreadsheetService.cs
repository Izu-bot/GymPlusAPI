using System;
using GymPlusAPI.Application.DTOs.Request.Spreadsheet;
using GymPlusAPI.Application.DTOs.Response.Spreadsheet;
using GymPlusAPI.Application.DTOs.Response.Workout;
using GymPlusAPI.Application.Interfaces;
using GymPlusAPI.Domain.Entities;
using GymPlusAPI.Domain.Exceptions;
using GymPlusAPI.Domain.Interfaces;

namespace GymPlusAPI.Application.Services;

public class SpreadsheetService(ISpreadsheetRepository spreadsheetRepository) : ISpreadsheetService
{
    public async Task<SpreadsheetResponse> CreateAsync(CreateSpreadsheetRequest dto, Guid userId)
    {
        var spreadsheet = new Spreadsheet(dto.Name, userId);

        await spreadsheetRepository.AddAsync(spreadsheet);

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
        var spreadsheetToUpdate = await spreadsheetRepository.GetSpreadsheetByIdAsync(dto.Id, userId)
                                  ?? throw new EntityNotFoundException("Planilha");

        spreadsheetToUpdate.Name = dto.Name;
        await spreadsheetRepository.UpdateAsync(spreadsheetToUpdate);
    }

    public async Task DeleteAsync(int spreadsheetId, Guid userId)
    {
        var spreadsheetToDelete = await spreadsheetRepository.GetSpreadsheetByIdAsync(spreadsheetId, userId)
            ?? throw new EntityNotFoundException("Planilha");

        await spreadsheetRepository.DeleteAsync(spreadsheetToDelete);
    }

    public async Task<IEnumerable<SpreadsheetResponse>> GetAllAsync(Guid userId)
    {
        var spreadsheets = (await spreadsheetRepository.GetSpreadsheetsByUserAsync(userId)).ToList();
        
        if(!spreadsheets.Any())
            throw new EntityNotFoundException("Planilha");
        
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
        var spreadsheet = await spreadsheetRepository.GetSpreadsheetByIdAsync(spreadsheetId, userId)
            ?? throw new EntityNotFoundException("Planilha");       

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

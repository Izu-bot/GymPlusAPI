using GymPlusAPI.Application.DTOs.Request.RecurrentTraining;
using GymPlusAPI.Application.DTOs.Response.RecurrentTraining;
using GymPlusAPI.Application.DTOs.Response.Spreadsheet;

namespace GymPlusAPI.Application.Interfaces;

public interface IRecurrentTrainingService
{
    Task<RecurrentTrainingResponse> CreateAsync(int spreadsheetId, RecurrentTrainingRequest dto, Guid userId);
    Task<List<KeyValuePair<SpreadsheetResponse, RecurrentTrainingResponse>>> GetRecurrentTraining(Guid userId);
}
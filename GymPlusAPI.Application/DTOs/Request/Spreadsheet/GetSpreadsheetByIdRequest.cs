using System;
using GymPlusAPI.Application.DTOs.Request.Workout;

namespace GymPlusAPI.Application.DTOs.Request.Spreadsheet;

public record GetSpreadsheetByIdRequest(
    int Id
);

using GymPlusAPI.Application.Exceptions;
using GymPlusAPI.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GymPlusAPI.API.Filters;

public class CustomExceptionFilter(ILogger<CustomExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var (statusCode, message) = context.Exception switch
        {
            EntityNotFoundException entityNotFoundException => (
                StatusCodes.Status404NotFound,
                entityNotFoundException.Message
            ),
            InvalidCredentialsException invalidCredentialsException => (
                StatusCodes.Status400BadRequest,
                invalidCredentialsException.Message
            ),
            UserExistsException userExistsException => (
                StatusCodes.Status409Conflict,
                userExistsException.Message
            ),
            UserNotFoundException userNotFoundException => (
                StatusCodes.Status404NotFound,
                userNotFoundException.Message
            ),
            ApplicationException appException => (
                StatusCodes.Status400BadRequest,
                appException.Message
            ),
            _ => (StatusCodes.Status500InternalServerError, "Um erro interno ocorreu. Por favor, tente novamente mais tarde.")
        };

        context.HttpContext.Response.StatusCode = statusCode;

        var result = new ObjectResult(new
        {
            Message = message,
            StatusCode = statusCode
        });
        
        logger.LogError(context.Exception, $"Erro {message}");
        context.Result = result;
        context.ExceptionHandled = true;
    }
}
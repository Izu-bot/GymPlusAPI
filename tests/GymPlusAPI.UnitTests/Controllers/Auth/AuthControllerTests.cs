using GymPlusAPI.API.Controllers.Auth;
using GymPlusAPI.Application.Auth;
using GymPlusAPI.Application.DTOs.Request.Login;
using GymPlusAPI.Application.DTOs.Response.Login;
using GymPlusAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GymPlusAPI.UnitTests.Controllers.Auth;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<IJwtGenerator> _jwtGeneratorMock;
    private readonly AuthController _authController;

    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _jwtGeneratorMock = new Mock<IJwtGenerator>();
        _authController = new AuthController(_authServiceMock.Object, _jwtGeneratorMock.Object);
    }

    [Fact]
    public async Task Login_ShouldReturnOk_WhenCredentialsAreValid()
    {
        // Arrange
        var request = new LoginRequest("email@email.com", "123456");
        var response = new LoginResponse("valid-token");
        
        _authServiceMock.Setup(s => s.LoginAsync(request)).ReturnsAsync(response);
        
        // Act
        var result = await _authController.Login(request);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        
        var myObject = (LoginResponse) okResult.Value;
        var token = myObject.AccessToken;
        
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(response.AccessToken, token);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var request = new LoginRequest("email@email.com", "123456");

        _authServiceMock.Setup(s => s.LoginAsync(request)).ReturnsAsync(value: null);
        
        // Act
        var result = await _authController.Login(request);
        
        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal(401, unauthorizedResult.StatusCode);
    }

    [Fact]
    public async Task Login_ShouldReturnBadRequest_WhenRequestIsNull()
    {
        // Arrange
        var request = new LoginRequest("", "");
        
        _authController.ModelState.AddModelError("Email", "O campo de email não deve ficar vazio.");
        _authController.ModelState.AddModelError("Password", "O campo da senha não deve ficar vazio.");
        
        // Act
        var result = await _authController.Login(request);
        
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }
}

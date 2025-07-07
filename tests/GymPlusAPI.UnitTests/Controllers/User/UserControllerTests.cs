using System.Security.Claims;
using GymPlusAPI.API.Controllers;
using GymPlusAPI.Application.DTOs.Request.User;
using GymPlusAPI.Application.DTOs.Response.User;
using GymPlusAPI.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GymPlusAPI.UnitTests.Controllers.User;

public class UserControllerTests
{
    private readonly UserController _userController;
    private readonly Mock<IUserService> _userServiceMock;

    public UserControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _userController = new UserController(_userServiceMock.Object);
    }
    
    [Fact]
    public async Task Create_ShouldReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateUserRequest("email@email.com", "123456", "Teste");
        var response = new UserResponse(Guid.Empty, "email@email.com", "Teste");
        
        _userServiceMock.Setup(s => s.AddAsync(request)).ReturnsAsync(response);
        
        // Act
        var result = await _userController.Create(request);
        
        // Assert
        var createdAtAction = Assert.IsType<CreatedAtActionResult>(result);
        Assert.NotNull(createdAtAction.Value);
        
        var myObject = (UserResponse) createdAtAction.Value;
        var id = myObject.Id;
        
        Assert.Equal(201, createdAtAction.StatusCode);
        Assert.Equal(response.Id, id);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenRequestIsNull()
    {
        // Arrange
        var request = new CreateUserRequest("", "", "");
        
        _userController.ModelState.AddModelError("", "");
        
        // Act
        var result = await _userController.Create(request);
        
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnOk_WhenUserIdIsRetrievedFromToken()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var response = new UserResponse(userId, "email@email.com", "Teste");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString())
        };
        
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        // Força o controller a ter esse User no HttpContext
        _userController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };

        _userServiceMock
            .Setup(s => s.GetByIdAsync(userId))
            .ReturnsAsync(response);

        // Act
        var result = await _userController.GetById();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        
        Assert.Equal(userId, okResult.Value);
    }
}
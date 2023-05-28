using AutoMapper;
using DatingApp.Controllers;
using DatingApp.DB.Models.Chats;
using DatingApp.DB.Models.Recommendations;
using DatingApp.DTOs.Recommendations;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DatingApp.UnitTests
{
    [TestFixture]
    public class RecommendationsControllerTests
    {
        private RecommendationsController _controller;
        private Mock<IRecommendationsService> _recommendationsServiceMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void Setup()
        {
            _recommendationsServiceMock = new Mock<IRecommendationsService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new RecommendationsController(_recommendationsServiceMock.Object, _mapperMock.Object);
        }

        [Test]
        public void GetRecommendedUsers_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var filters = new FiltersDto();
            var recommendedUsers = new List<RecommendedUser>(); // Provide sample data
            var mappedUsers = new List<RecommendedUserDto>(); // Provide sample mapped data
            _recommendationsServiceMock.Setup(s => s.GetRecommendedUsers(filters)).Returns(recommendedUsers);
            _mapperMock.Setup(m => m.Map<IEnumerable<RecommendedUserDto>>(recommendedUsers)).Returns(mappedUsers);

            // Act
            var result = _controller.GetRecommendedUsers(filters) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mappedUsers, result.Value);
        }

        [Test]
        public void GetRecommendedUsers_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            var filters = new FiltersDto();
            var exceptionMessage = "Sample exception";
            _recommendationsServiceMock.Setup(s => s.GetRecommendedUsers(filters)).Throws(new Exception(exceptionMessage));

            // Act
            var result = _controller.GetRecommendedUsers(filters) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(exceptionMessage, result.Value);
        }

        [Test]
        public async Task AddUserLikeAsync_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new UserLikeDto();
            var result = true; // Provide sample result
            _recommendationsServiceMock.Setup(s => s.AddUserLikeAsync(request.LikingUserId, request.LikedUserId)).ReturnsAsync(result);

            // Act
            var actionResult = await _controller.AddUserLikeAsync(request);
            var okResult = actionResult as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(result, okResult.Value);
        }

        [Test]
        public void AddUserLikeAsync_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Sample error");
            var request = new UserLikeDto();

            // Act
            var result = _controller.AddUserLikeAsync(request).GetAwaiter().GetResult() as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetUserLikesAsync_ValidUserId_ReturnsOkResult()
        {
            // Arrange
            var userId = "123";
            var userLikes = new List<RecommendedUser>(); // Provide sample data
            var mappedLikes = new List<RecommendedUserDto>(); // Provide sample mapped data
            _recommendationsServiceMock.Setup(s => s.GetUserLikesAsync(userId)).ReturnsAsync(userLikes);
            _mapperMock.Setup(m => m.Map<IEnumerable<RecommendedUserDto>>(userLikes)).Returns(mappedLikes);

            // Act
            var actionResult = await _controller.GetUserLikesAsync(userId);
            var okResult = actionResult as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(mappedLikes, okResult.Value);
        }

        [Test]
        public async Task GetUserLikesAsync_InvalidUserId_ReturnsBadRequest()
        {
            // Arrange
            var userId = ""; // Empty userId
            var errorMessage = "Invalid userId";
            _controller.ModelState.AddModelError("Error", errorMessage);

            // Act
            var result = await _controller.GetUserLikesAsync(userId) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetUserMutualLikesAsync_ValidUserId_ReturnsOkResult()
        {
            // Arrange
            var userId = "123";
            var userMutualLikes = new List<RecommendedUser>(); // Provide sample data
            var mappedMutualLikes = new List<RecommendedUserDto>(); // Provide sample mapped data
            _recommendationsServiceMock.Setup(s => s.GetUserMutualLikesAsync(userId)).ReturnsAsync(userMutualLikes);
            _mapperMock.Setup(m => m.Map<IEnumerable<RecommendedUserDto>>(userMutualLikes)).Returns(mappedMutualLikes);

            // Act
            var actionResult = await _controller.GetUserMutualLikesAsync(userId);
            var okResult = actionResult as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(mappedMutualLikes, okResult.Value);
        }

        [Test]
        public async Task GetUserMutualLikesAsync_InvalidUserId_ReturnsBadRequest()
        {
            // Arrange
            var userId = ""; // Empty userId
            var errorMessage = "Invalid userId";
            _controller.ModelState.AddModelError("Error", errorMessage);

            // Act
            var result = await _controller.GetUserMutualLikesAsync(userId) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteUserLikeAsync_ValidLikeId_ReturnsOkResult()
        {
            // Arrange
            var likeId = 1; // Provide sample likeId

            // Act
            var actionResult = await _controller.DeleteUserLikeAsync(likeId);
            var okResult = actionResult as OkResult;

            // Assert
            Assert.IsNotNull(okResult);
        }

        [Test]
        public async Task DeleteMutualLikeAsync_ValidLikeId_ReturnsOkResult()
        {
            // Arrange
            var likeId = 1; // Provide sample likeId

            // Act
            var actionResult = await _controller.DeleteMutualLikeAsync(likeId);
            var okResult = actionResult as OkResult;

            // Assert
            Assert.IsNotNull(okResult);
        }

        [Test]
        public async Task GetUserChatsAsync_ValidUserId_ReturnsOkResult()
        {
            // Arrange
            var userId = "123";
            var userChats = new List<Tuple<UsersChat, int>>(); // Provide sample data
            var mappedChats = new List<UsersChatDto>(); // Provide sample mapped data
            _recommendationsServiceMock.Setup(s => s.GetUserChatsAsync(userId)).ReturnsAsync(userChats);
            _mapperMock.Setup(m => m.Map<IEnumerable<UsersChatDto>>(userChats)).Returns(mappedChats);

            // Act
            var actionResult = await _controller.GetUserChatsAsync(userId);
            var okResult = actionResult as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(mappedChats, okResult.Value);
        }

        [Test]
        public async Task CreateChatAsync_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new CreateChatDto();
            var chatId = 1; // Provide sample chatId
            _recommendationsServiceMock.Setup(s => s.CreateChatAsync(request.MutualLikeId)).ReturnsAsync(chatId);

            // Act
            var actionResult = await _controller.CreateChatAsync(request);
            var okResult = actionResult as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(chatId, okResult.Value);
        }

        [Test]
        public async Task CreateChatAsync_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateChatDto();
            var exceptionMessage = "Sample exception";
            _recommendationsServiceMock.Setup(s => s.CreateChatAsync(request.MutualLikeId)).Throws(new Exception(exceptionMessage));

            // Act
            var result = await _controller.CreateChatAsync(request) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(exceptionMessage, result.Value);
        }

        [Test]
        public async Task DeleteChatAsync_ValidChatId_ReturnsOkResult()
        {
            // Arrange
            var chatId = 1; // Provide sample chatId

            // Act
            var actionResult = await _controller.DeleteChatAsync(chatId);
            var okResult = actionResult as OkResult;

            // Assert
            Assert.IsNotNull(okResult);
        }

        [Test]
        public async Task GetChatInfoAsync_ValidChatId_ReturnsOkResult()
        {
            // Arrange
            var chatId = 1; // Provide sample chatId
            var chat = new UsersChat(); // Provide sample chat
            var mappedChat = new UsersChatDto(); // Provide sample mapped chat
            _recommendationsServiceMock.Setup(s => s.GetChatAsync(chatId)).ReturnsAsync(chat);
            _mapperMock.Setup(m => m.Map<UsersChatDto>(chat)).Returns(mappedChat);

            // Act
            var actionResult = await _controller.GetChatInfoAsync(chatId);
            var okResult = actionResult as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(mappedChat, okResult.Value);
        }

        [Test]
        public async Task GetChatInfoAsync_InvalidChatId_ReturnsBadRequest()
        {
            // Arrange
            var chatId = 0; // Invalid chatId
            var errorMessage = "No chat with such id";
            var chat = null as UsersChat; // Provide sample null chat
            _recommendationsServiceMock.Setup(s => s.GetChatAsync(chatId)).ReturnsAsync(chat);

            // Act
            var result = await _controller.GetChatInfoAsync(chatId) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(errorMessage, result.Value);
        }
    }
}

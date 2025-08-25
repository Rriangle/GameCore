using System.Security.Claims;
using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace GameCore.Tests.UnitTests
{
    public class WalletControllerTests
    {
        [Fact]
        public async Task GetBalance_ReturnsBalance_ForAuthenticatedUser()
        {
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.UserRepository.GetWalletAsync(1))
               .ReturnsAsync(new UserWallet { UserId = 1, UserPoint = 1234 });

            var logger = Mock.Of<ILogger<WalletController>>();
            var controller = new WalletController(uow.Object, logger);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            var identity = new ClaimsIdentity(new[] { new Claim("UserId", "1") }, "TestAuth");
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(identity);

            var result = await controller.GetBalance() as OkObjectResult;

            result.Should().NotBeNull();
            var body = result!.Value as dynamic;
            int balance = (int)body.balance;
            balance.Should().Be(1234);
        }
    }
}


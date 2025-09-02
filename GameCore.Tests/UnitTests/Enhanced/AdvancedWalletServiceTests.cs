using Xunit;
using Moq;
using FluentAssertions;
using GameCore.Core.Services.Enhanced;
using GameCore.Core.Interfaces;
using GameCore.Core.Entities;
using Microsoft.Extensions.Logging;

namespace GameCore.Tests.UnitTests.Enhanced
{
    public class AdvancedWalletServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IWalletRepository> _mockWalletRepository;
        private readonly Mock<ILogger<AdvancedWalletService>> _mockLogger;
        private readonly AdvancedWalletService _service;

        public AdvancedWalletServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockWalletRepository = new Mock<IWalletRepository>();
            _mockLogger = new Mock<ILogger<AdvancedWalletService>>();
            
            _mockUnitOfWork.Setup(x => x.Wallets).Returns(_mockWalletRepository.Object);
            
            // Service would be injected with additional dependencies
            // _service = new AdvancedWalletService(...);
        }

        [Fact]
        public async Task ExecuteAtomicTransactionAsync_WithValidSteps_ShouldSucceed()
        {
            // Arrange
            var request = new AtomicTransactionRequest
            {
                InitiatorUserId = 1,
                Steps = new List<TransactionStep>
                {
                    new TransactionStep { UserId = 1, Amount = -100, Type = TransactionType.Transfer },
                    new TransactionStep { UserId = 2, Amount = 100, Type = TransactionType.Receive }
                }
            };

            var wallet1 = new Wallet { UserId = 1, Points = 500 };
            var wallet2 = new Wallet { UserId = 2, Points = 200 };

            _mockWalletRepository.Setup(x => x.GetByUserIdAsync(1)).ReturnsAsync(wallet1);
            _mockWalletRepository.Setup(x => x.GetByUserIdAsync(2)).ReturnsAsync(wallet2);

            // Act
            var result = await _service.ExecuteAtomicTransactionAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.StepResults.Should().HaveCount(2);
            result.StepResults.Should().AllSatisfy(step => step.Success.Should().BeTrue());
        }

        [Fact]
        public async Task ExecuteAtomicTransactionAsync_WithInsufficientBalance_ShouldFail()
        {
            // Arrange
            var request = new AtomicTransactionRequest
            {
                InitiatorUserId = 1,
                Steps = new List<TransactionStep>
                {
                    new TransactionStep { UserId = 1, Amount = -1000, Type = TransactionType.Transfer }
                }
            };

            var wallet1 = new Wallet { UserId = 1, Points = 500 };
            _mockWalletRepository.Setup(x => x.GetByUserIdAsync(1)).ReturnsAsync(wallet1);

            // Act
            var result = await _service.ExecuteAtomicTransactionAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task AnalyzeTransactionAsync_WithHighRiskTransaction_ShouldFlagAsRisky()
        {
            // Arrange
            var request = new TransactionAnalysisRequest
            {
                UserId = 1,
                Amount = 10000, // Large amount
                RecipientId = 999, // Unknown recipient
                TransactionType = TransactionType.Transfer
            };

            // Act
            var result = await _service.AnalyzeTransactionAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.RiskLevel.Should().BeOneOf(FraudRiskLevel.High, FraudRiskLevel.Critical);
            result.RiskScore.Should().BeGreaterThan(0.7);
            result.RequiresManualReview.Should().BeTrue();
        }

        [Fact]
        public async Task CreateEscrowAsync_WithValidRequest_ShouldCreateEscrow()
        {
            // Arrange
            var request = new EscrowRequest
            {
                BuyerId = 1,
                SellerId = 2,
                Amount = 500,
                Description = "Test escrow transaction"
            };

            // Act
            var result = await _service.CreateEscrowAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.EscrowId.Should().BeGreaterThan(0);
        }
    }
} 
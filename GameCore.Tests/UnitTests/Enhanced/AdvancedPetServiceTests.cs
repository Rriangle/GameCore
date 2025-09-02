using Xunit;
using Moq;
using FluentAssertions;
using GameCore.Core.Services.Enhanced;
using GameCore.Core.Interfaces;
using GameCore.Core.Entities;
using Microsoft.Extensions.Logging;

namespace GameCore.Tests.UnitTests.Enhanced
{
    public class AdvancedPetServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IPetRepository> _mockPetRepository;
        private readonly Mock<ILogger<AdvancedPetService>> _mockLogger;
        private readonly AdvancedPetService _service;

        public AdvancedPetServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockPetRepository = new Mock<IPetRepository>();
            _mockLogger = new Mock<ILogger<AdvancedPetService>>();
            
            _mockUnitOfWork.Setup(x => x.Pets).Returns(_mockPetRepository.Object);
            
            // Service would be injected with additional dependencies
            // _service = new AdvancedPetService(...);
        }

        [Fact]
        public async Task GeneratePetPersonalityAsync_WithNewPet_ShouldCreateBalancedPersonality()
        {
            // Arrange
            var petId = 1;
            var pet = new Pet 
            { 
                Id = petId, 
                UserId = 1, 
                Level = 5,
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            };

            _mockPetRepository.Setup(x => x.GetByIdAsync(petId)).ReturnsAsync(pet);

            // Act
            var result = await _service.GeneratePetPersonalityAsync(petId);

            // Assert
            result.Should().NotBeNull();
            result.PetId.Should().Be(petId);
            result.PrimaryType.Should().BeOneOf(Enum.GetValues<PersonalityType>());
            result.Traits.Should().NotBeEmpty();
            result.TraitScores.Should().NotBeEmpty();
            result.StabilityScore.Should().BeInRange(0.0, 1.0);
        }

        [Fact]
        public async Task ProcessPetInteractionAsync_WithFeedingInteraction_ShouldReturnAppropriateResponse()
        {
            // Arrange
            var context = new PetInteractionContext
            {
                PetId = 1,
                UserId = 1,
                Type = InteractionType.Feed,
                Context = "User feeding pet with premium food",
                CurrentState = new PetCurrentState
                {
                    Level = 10,
                    Stats = new PetStats { Hunger = 30, Mood = 70, Health = 85 },
                    EmotionalState = new EmotionalState
                    {
                        CurrentMood = MoodType.Content,
                        HappinessLevel = 0.7,
                        StressLevel = 0.2
                    }
                }
            };

            // Act
            var result = await _service.ProcessPetInteractionAsync(context);

            // Assert
            result.Should().NotBeNull();
            result.Response.Should().NotBeNullOrEmpty();
            result.Type.Should().BeOneOf(Enum.GetValues<ResponseType>());
            result.EmotionalReaction.Should().NotBeNull();
            result.ConfidenceScore.Should().BeGreaterThan(0.5);
            result.SuggestedActions.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CheckEvolutionEligibilityAsync_WithHighLevelPet_ShouldReturnEvolutionPaths()
        {
            // Arrange
            var petId = 1;
            var pet = new Pet 
            { 
                Id = petId, 
                Level = 25, 
                Experience = 5000,
                Hunger = 90,
                Mood = 95,
                Health = 100
            };

            _mockPetRepository.Setup(x => x.GetByIdAsync(petId)).ReturnsAsync(pet);

            // Act
            var result = await _service.CheckEvolutionEligibilityAsync(petId);

            // Assert
            result.Should().NotBeNull();
            if (result.CanEvolve)
            {
                result.AvailablePaths.Should().NotBeEmpty();
                result.Requirements.Should().AllSatisfy(req => req.IsMet.Should().BeTrue());
            }
            result.EvolutionProgress.Should().BeInRange(0.0, 1.0);
        }
    }
} 
using FluentAssertions;

using Xunit;

namespace CardGameDurak.Logic.Tests;

public class GameSessionConfigurationValidatorTests
{
    [Fact(DisplayName = "Can be created.")]
    [Trait("Category", "Constructors")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() => new GameSessionConfigurationValidator());

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = "Can be validated.")]
    [Trait("Category", "Methods")]
    public void CanBeValidated()
    {
        // Arrange
        var configuration = new GameSessionConfiguration()
        {
            MinPlayersCount = 2,
            MaxPlayersCount = 3,
            DeckSize = 20
        };

        // Act
        var result = new GameSessionConfigurationValidator().Validate(null!, configuration);

        // Assert
        result.Succeeded.Should().BeTrue();
    }

    [Fact(DisplayName = "Can't validated when min players count less than 1.")]
    [Trait("Category", "Constructors")]
    public void CanNotBeValidatedWhenMinPlayersCountLessThanOne()
    {
        // Arrange
        var configuration = new GameSessionConfiguration()
        {
            MinPlayersCount = 0,
            MaxPlayersCount = 2,
            DeckSize = 36
        };

        // Act
        var result = new GameSessionConfigurationValidator().Validate(null!, configuration);

        // Assert
        result.Succeeded.Should().BeFalse();
    }

    [Fact(DisplayName = "Can't validated when max players count less than 1.")]
    [Trait("Category", "Methods")]
    public void CanNotBeValidatedWhenMaxPlayersCountLessThanOne()
    {
        // Arrange
        var configuration = new GameSessionConfiguration()
        {
            MinPlayersCount = 1,
            MaxPlayersCount = -1,
            DeckSize = 36
        };

        // Act
        var result = new GameSessionConfigurationValidator().Validate(null!, configuration);

        // Assert
        result.Succeeded.Should().BeFalse();
    }

    [Fact(DisplayName = "Can't validated when deck size less than 1.")]
    [Trait("Category", "Methods")]
    public void CanNotBeValidatedWhenDeckSizeLessThanOne()
    {
        // Arrange
        var configuration = new GameSessionConfiguration()
        {
            MinPlayersCount = 2,
            MaxPlayersCount = 3,
            DeckSize = 0
        };

        // Act
        var result = new GameSessionConfigurationValidator().Validate(null!, configuration);

        // Assert
        result.Succeeded.Should().BeFalse();
    }
}

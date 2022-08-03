using Microsoft.Extensions.Options;

namespace CardGameDurak.Logic;

/// <summary xml:lang = "ru">
/// Валидатор конфигурации игровой сессии.
/// </summary>
public class GameSessionConfigurationValidator : IValidateOptions<GameSessionConfiguration>
{
    /// <inheritdoc/>
    public ValidateOptionsResult Validate(string name, GameSessionConfiguration options)
    {
        if (options.MinPlayersCount < 1)
            return ValidateOptionsResult.Fail("Min players count can't be less than 1.");
        if (options.MaxPlayersCount < 1)
            return ValidateOptionsResult.Fail("Max players count can't be less than 1.");
        if (options.MaxPlayersCount < options.MinPlayersCount)
            return ValidateOptionsResult.Fail("Max players count can't be less than min count.");
        if (options.DeckSize < 1)
            return ValidateOptionsResult.Fail("Deck size can't be less than 1.");

        return ValidateOptionsResult.Success;
    }
}

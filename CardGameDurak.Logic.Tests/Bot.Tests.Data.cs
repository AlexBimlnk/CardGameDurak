using System.Collections.Generic;

using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Enums;

using Xunit;

using Moq;

namespace CardGameDurak.Logic.Tests;
public class BotTestsData
{
    private static Mock<IPlayer> s_playerStub1 = new(MockBehavior.Strict);
    private static Mock<IPlayer> s_playerStub2 = new(MockBehavior.Strict);


    static BotTestsData()
    {
        s_playerStub1.Setup(x => x.Id).Returns(1);
        s_playerStub2.Setup(x => x.Id).Returns(2);
    }


    //Todo: Тестовые данные для проверки логики бота.
    public static readonly TheoryData<IReadOnlyCollection<ICard>, ICard> CanAttackData = new()
    {
        {
            // Карты на столе.
            new[]
            {
                new Card(Suit.Clubs, 6)
                {
                    IsCloseOnDesktop = true,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Clubs, 6)
                {
                    IsCloseOnDesktop = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Clubs, 6)
                {
                    IsCloseOnDesktop = false,
                    Owner = s_playerStub1.Object
                }
            },

            // Ожидаемая карта
            new Card(Suit.Diamonds, 8)
            {
                IsCloseOnDesktop = false,
            }
        },
    };
    public static readonly TheoryData<IReadOnlyCollection<ICard>, ICard, ICard> CanDefenceData = new()
    {
        {
            // Карты на столе.
            new[]
            {
                new Card(Suit.Clubs, 6)
                {
                    IsCloseOnDesktop = true,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Clubs, 6)
                {
                    IsCloseOnDesktop = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Clubs, 6)
                {
                    IsCloseOnDesktop = false,
                    Owner = s_playerStub1.Object
                }
            },

            // Ожидаемая карта
            new Card(Suit.Diamonds, 8)
            {
                IsCloseOnDesktop = false,
            },

            // Побитая карта
            new Card(Suit.Spades, 10)
            {
                IsCloseOnDesktop = false,
            }
        },
    };
}

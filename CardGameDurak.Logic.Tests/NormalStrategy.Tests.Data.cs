using System.Collections.Generic;

using CardGameDurak.Abstractions;
using CardGameDurak.Abstractions.Enums;

using Moq;

using Xunit;

namespace CardGameDurak.Logic.Tests;
public class NormalStrategyTestsData
{
    // "Заглушки" на бота (s_playerStub1) и игрока (s_playerStub2)
    private static Mock<IPlayer> s_playerStub1 = new(MockBehavior.Strict);
    private static Mock<IPlayer> s_playerStub2 = new(MockBehavior.Strict);


    static NormalStrategyTestsData()
    {
        s_playerStub1.Setup(x => x.Id).Returns(1);
        s_playerStub2.Setup(x => x.Id).Returns(2);
    }

    public static readonly TheoryData<List<ICard>, List<ICard>, ICard, bool> TryAttackData = new()
    {
        {
            // Карты в руке у бота
            new List<ICard>()
            {
                new Card(Suit.Clubs, 8)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Clubs, 9)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Diamonds, 8)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Spades, 7)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Hearts, 7)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = true,
                    Owner = s_playerStub1.Object
                }
            },

            // Карты на столе.
            new List<ICard>()
            {
                new Card(Suit.Clubs, 6)
                {
                    IsCloseOnDesktop = true,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Clubs, 7)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub2.Object
                }
            },

            // Ожидаемая карта
            new Card(Suit.Spades, 7)
            {
                IsCloseOnDesktop = false,
                IsTrump = false,
                Owner = s_playerStub1.Object
            },

            true
        },
        {
            // Карты в руке у бота
            new List<ICard>()
            {
                new Card(Suit.Clubs, 7)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Clubs, 9)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Diamonds, 10)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Hearts, 9)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = true,
                    Owner = s_playerStub1.Object
                }
            },

            // Карты на столе.
            new List<ICard>()
            {
                new Card(Suit.Diamonds, 6)
                {
                    IsCloseOnDesktop = true,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Diamonds, 9)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub2.Object
                },
                new Card(Suit.Spades, 9)
                {
                    IsCloseOnDesktop = true,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Hearts, 7)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = true,
                    Owner = s_playerStub2.Object
                }
            },

            // Ожидаемая карта
            new Card(Suit.Clubs, 7)
            {
                IsCloseOnDesktop = false,
                IsTrump = false,
                Owner = s_playerStub1.Object
            },

            true
        },
        {
            // Карты в руке у бота
            new List<ICard>()
            {
                new Card(Suit.Diamonds, 11)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Hearts, 12)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = true,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Hearts, 13)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = true,
                    Owner = s_playerStub1.Object
                }
            },

            // Карты на столе.
            new List<ICard>()
            {
                new Card(Suit.Spades, 8)
                {
                    IsCloseOnDesktop = true,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Spades, 9)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub2.Object
                },
                new Card(Suit.Clubs, 9)
                {
                    IsCloseOnDesktop = true,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Clubs, 10)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub2.Object
                },
                new Card(Suit.Diamonds, 10)
                {
                    IsCloseOnDesktop = true,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Diamonds, 12)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub2.Object
                }
            },

            // Ожидаемая карта
            new Card(Suit.Hearts, 12)
            {
                IsCloseOnDesktop = false,
                IsTrump = true,
                Owner = s_playerStub1.Object
            },

            true
        },
        {
            // Карты в руке у бота
            new List<ICard>()
            {
                new Card(Suit.Diamonds, 12)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Hearts, 12)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = true,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Hearts, 13)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = true,
                    Owner = s_playerStub1.Object
                }
            },

            // Карты на столе.
            new List<ICard>()
            {
                new Card(Suit.Spades, 8)
                {
                    IsCloseOnDesktop = true,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Spades, 9)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub2.Object
                },
                new Card(Suit.Clubs, 9)
                {
                    IsCloseOnDesktop = true,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Clubs, 10)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub2.Object
                },
                new Card(Suit.Diamonds, 10)
                {
                    IsCloseOnDesktop = true,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Diamonds, 11)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub2.Object
                }
            },

            // Ожидаемая карта
            null!,

            false
        }
    };
    public static readonly TheoryData<List<ICard>, List<ICard>, ICard, ICard, bool> TryDefenceData = new()
    {
        {
            // Карты на столе.
            new List<ICard>()
            {
                new Card(Suit.Clubs, 6)
                {
                    IsCloseOnDesktop = true,
                    IsTrump = false,
                    Owner = s_playerStub2.Object
                },
                new Card(Suit.Clubs, 7)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Spades, 7)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub2.Object
                }
            },

            // Карты у бота
            new List<ICard>()
            {
                new Card(Suit.Spades, 7)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                }
            },

            // Побитая карта
            new Card(Suit.Spades, 7)
            {
                IsCloseOnDesktop = true,
                IsTrump = false,
                Owner = s_playerStub2.Object
            },

            // Ожидаемая карта
            new Card(Suit.Spades, 8)
            {
                IsCloseOnDesktop = false,
                IsTrump = false,
                Owner = s_playerStub1.Object
            },

            true
        },
        {
            // Карты в руке у бота
            new List<ICard>()
            {
                new Card(Suit.Diamonds, 11)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Spades, 11)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Hearts, 8)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = true,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Hearts, 10)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = true,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Clubs, 11)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                }
            },

            // Карты на столе.
            new List<ICard>()
            {
                new Card(Suit.Clubs, 10)
                {
                    IsCloseOnDesktop = true,
                    IsTrump = false,
                    Owner = s_playerStub2.Object
                },
                new Card(Suit.Hearts, 7)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = true,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Spades, 10)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub2.Object
                }
            },

            // Побитая карта
            new Card(Suit.Spades, 10)
            {
                IsCloseOnDesktop = true,
                IsTrump = true,
                Owner = s_playerStub2.Object
            },

            // Ожидаемая карта
            new Card(Suit.Spades, 11)
            {
                IsCloseOnDesktop = false,
                IsTrump = false,
                Owner = s_playerStub1.Object
            },

            true
        },
        {
            // Карты в руке у бота
            new List<ICard>()
            {
                new Card(Suit.Clubs, 8)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Clubs, 9)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Spades, 8)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Hearts, 6)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = true,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Hearts, 7)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = true,
                    Owner = s_playerStub1.Object
                }
            },

            // Карты на столе.
            new List<ICard>()
            {
                new Card(Suit.Clubs, 6)
                {
                    IsCloseOnDesktop = true,
                    IsTrump = false,
                    Owner = s_playerStub2.Object
                },
                new Card(Suit.Clubs, 7)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Diamonds, 7)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub2.Object
                }
            },

            // Побитая карта
            new Card(Suit.Diamonds, 7)
            {
                IsCloseOnDesktop = true,
                IsTrump = false,
                Owner = s_playerStub2.Object
            },

            // Ожидаемая карта
            new Card(Suit.Hearts, 6)
            {
                IsCloseOnDesktop = false,
                IsTrump = true,
                Owner = s_playerStub1.Object
            },

            true
        },
        {
            // Карты в руке у бота
            new List<ICard>()
            {
                new Card(Suit.Diamonds, 13)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Hearts, 10)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = true,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Hearts, 13)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = true,
                    Owner = s_playerStub1.Object
                }
            },

            // Карты на столе.
            new List<ICard>()
            {
                new Card(Suit.Diamonds, 6)
                {
                    IsCloseOnDesktop = true,
                    IsTrump = false,
                    Owner = s_playerStub2.Object
                },
                new Card(Suit.Diamonds, 8)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Clubs, 6)
                {
                    IsCloseOnDesktop = true,
                    IsTrump = false,
                    Owner = s_playerStub2.Object
                },
                new Card(Suit.Clubs, 7)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Spades, 6)
                {
                    IsCloseOnDesktop = true,
                    IsTrump = false,
                    Owner = s_playerStub2.Object
                },
                new Card(Suit.Spades, 10)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Hearts, 6)
                {
                    IsCloseOnDesktop = true,
                    IsTrump = true,
                    Owner = s_playerStub2.Object
                }
            },

            // Побитая карта
            new Card(Suit.Hearts, 6)
            {
                IsCloseOnDesktop = true,
                IsTrump = true,
                Owner = s_playerStub2.Object
            },

            // Ожидаемая карта
            new Card(Suit.Hearts, 10)
            {
                IsCloseOnDesktop = false,
                IsTrump = true,
                Owner = s_playerStub1.Object
            },

            true
        },
        {
            // Карты в руке у бота
            new List<ICard>()
            {
                new Card(Suit.Diamonds, 13)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Spades, 14)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = true,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Clubs, 14)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = true,
                    Owner = s_playerStub1.Object
                }
            },

            // Карты на столе.
            new List<ICard>()
            {
                new Card(Suit.Diamonds, 6)
                {
                    IsCloseOnDesktop = true,
                    IsTrump = false,
                    Owner = s_playerStub2.Object
                },
                new Card(Suit.Diamonds, 8)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Clubs, 6)
                {
                    IsCloseOnDesktop = true,
                    IsTrump = false,
                    Owner = s_playerStub2.Object
                },
                new Card(Suit.Clubs, 7)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Spades, 6)
                {
                    IsCloseOnDesktop = true,
                    IsTrump = false,
                    Owner = s_playerStub2.Object
                },
                new Card(Suit.Spades, 10)
                {
                    IsCloseOnDesktop = false,
                    IsTrump = false,
                    Owner = s_playerStub1.Object
                },
                new Card(Suit.Hearts, 6)
                {
                    IsCloseOnDesktop = true,
                    IsTrump = true,
                    Owner = s_playerStub2.Object
                }
            },

            // Побитая карта
            null!,

            // Ожидаемая карта
            null!,

            false
        }
    };
}

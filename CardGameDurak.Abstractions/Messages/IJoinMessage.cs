namespace CardGameDurak.Abstractions.Messages;

public interface IJoinMessage
{
    public int AwaitPlayersCount { get; }
    public IPlayer Player { get; }
}

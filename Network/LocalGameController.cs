namespace CardGameDurak.Network;

internal class LocalGameController : IGameController
{
    private readonly List<string> _addresses = new List<string>();

    public LocalGameController(IEnumerable<string> addresses) =>
        _addresses.AddRange(addresses ?? throw new ArgumentNullException(nameof(addresses)));
}

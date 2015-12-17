namespace DraftBot.Protocol
{
    public interface ICommand
    {
        string Id { get; }

        string Name { get; }

        bool IsImmediate { get; }
    }
}
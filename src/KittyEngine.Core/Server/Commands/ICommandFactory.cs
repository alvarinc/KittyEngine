
namespace KittyEngine.Core.Server.Commands
{
    internal interface ICommandFactory
    {
        ICommandFactory Register<TCommand>(string name) where TCommand : IGameCommand, new();
        IGameCommand Create(string name);
    }
}

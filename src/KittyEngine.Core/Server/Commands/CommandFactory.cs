
namespace KittyEngine.Core.Server.Commands
{
    internal class CommandFactory : ICommandFactory
    {
        private Dictionary<string, Func<IGameCommand>> _commands = new Dictionary<string, Func<IGameCommand>>();

        public CommandFactory() 
        {
            Register<NopCommand>("nop");
            Register<JoinCommand>("join");
            Register<ExitCommand>("exit");
            Register<MoveCommand>("move");
        }

        public IGameCommand Create(string command)
        {
            if (_commands.ContainsKey(command))
            {
                return _commands[command].Invoke();
            }

            return null;
        }

        public ICommandFactory Register<TCommand>(string name) where TCommand : IGameCommand, new()
        {
            _commands[name] = () => new TCommand();
            return this;
        }
    }
}

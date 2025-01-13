namespace KittyEngine.Core.Server.Behaviors
{
    public struct GameCommandResult
    {
        private static GameCommandResult _noneResult = new GameCommandResult();
        public static GameCommandResult None => _noneResult;

        public bool StateUpdated { get; set; }
        public bool PeerInitializated { get; set; }

        public GameCommandResult Append(GameCommandResult newResult)
        {
            return new GameCommandResult
            {
                StateUpdated = StateUpdated || newResult.StateUpdated,
                PeerInitializated = PeerInitializated || newResult.PeerInitializated
            };
        }
    }
}

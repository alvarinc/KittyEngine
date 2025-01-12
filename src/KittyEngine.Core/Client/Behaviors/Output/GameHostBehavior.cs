using KittyEngine.Core.Client.Outputs;
using KittyEngine.Core.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KittyEngine.Core.Client.Behaviors.Output
{
    public class GameHostBehavior : ClientBehavior
    {
        private IGameHost _gameHost;
        public GameHostBehavior(IGameHost gameHost)
        {
            _gameHost = gameHost;
        }

        public override void OnRenderOutput()
        {
            _gameHost.Synchronize();
        }
    }
}

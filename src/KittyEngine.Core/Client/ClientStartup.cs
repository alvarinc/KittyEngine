﻿using KittyEngine.Core.Services.IoC;

namespace KittyEngine.Core.Client
{
    public static class ClientStartup
    {
        public static IServiceContainer ConfigureGameClient(this IServiceContainer container)
        {
            container.Register<IClientGameLogic, ClientGameLogic>(ServiceBehavior.Scoped);

            container.Register<Client>(ServiceBehavior.Scoped);

            return container;
        }
    }
}
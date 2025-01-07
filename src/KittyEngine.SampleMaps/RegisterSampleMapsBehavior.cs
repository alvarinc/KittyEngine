using KittyEngine.Core;
using KittyEngine.Core.GameEngine.Graphics.Assets;
using KittyEngine.Core.Services.IoC;

namespace KittyEngine.SampleMaps
{
    public class RegisterSampleMapsBehavior : CompositionBehavior
    {
        public override void OnStartup(IServiceContainer container)
        {
            var contentService = container.Get<IContentService>();
            contentService.RegisterContentFromSampleMaps();
        }
    }
}

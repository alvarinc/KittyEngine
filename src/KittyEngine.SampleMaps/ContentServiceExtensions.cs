using KittyEngine.Core.GameEngine.Graphics.Assets;

namespace KittyEngine.SampleMaps
{
    public static class ContentServiceExtensions
    {
        public static void RegisterContentFromSampleMaps(this IContentService contentService)
        {
            contentService.RegisterSource(new EmbeddedContentSource(typeof(KittyEngine.SampleMaps.Maze.MazeMapBuilder)));
        }
    }
}

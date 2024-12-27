using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;

namespace KittyEngine.Core.GameEngine.Graphics.Assets
{
    public class ContentService : IContentService
    {
        private List<IContentSource> _sources;
        public ContentService()
        {
            _sources = new List<IContentSource>();

            RegisterSource(new EmbeddedContentSource());
            RegisterSource(new FolderContentSource("Content"));
        }

        public void RegisterSource(IContentSource source)
        {
            if (!_sources.Any(x => x.Description.Type == source.Description.Type && x.Description.Path == source.Description.Path))
            {
                _sources.Add(source);
            }
        }

        public ContentSourceDescription[] GetSources()
        {
            return _sources.Select(x => x.Description).ToArray();
        }

        public void RemoveSource(ContentSourceDescription sourceDescription)
        {
            for(int i = 0; i < _sources.Count(); i++)
            {
                if (_sources[i].Description.Type == sourceDescription.Type && _sources[i].Description.Path == sourceDescription.Path)
                {
                    _sources.RemoveAt(i);
                    break;
                }
            }
        }

        public string[] GetResources(AssetType contentType)
        {
            var results = new List<string>();

            foreach(var source in _sources)
            {
                results.AddRange(source.GetResources(contentType));
            }

            results.Sort();

            return results.Distinct().ToArray();
        }

        public Stream GetStream(AssetType contentType, string resourceName)
        {
            foreach (var source in _sources)
            {
                var result = source.GetStream(contentType, resourceName);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public BitmapImage Get(AssetType contentType, string resourceName)
        {
            foreach (var source in _sources)
            {
                var result = source.Get(contentType, resourceName);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public Bitmap GetBitmap(AssetType contentType, string resourceName)
        {
            foreach (var source in _sources)
            {
                var result = source.GetBitmap(contentType, resourceName);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}

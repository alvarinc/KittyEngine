using KittyEngine.Core.Graphics.Assets.Maps;
using KittyEngine.Core.Graphics.Models.Definitions;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KittyEngine.SampleMaps.Maze
{
    public class MazeMapBuilder : IMapBuilder
    {
        public string MapName => $"Random Maze {SizeX} x {SizeZ}";

        public Point3D PlayerPosition => new Point3D(20, 2, 20);

        public Vector3D PlayerLookDirection => new Vector3D(1, 0, 1);

        public int SizeX { get; set; }
        public int SizeZ { get; set; }

        private MazeNode[,] _nodes;

        public MazeMapBuilder()
        {
            SizeX = 8;
            SizeZ = 12;
        }

        public MazeMapBuilder(int sizeX, int sizeZ, MazeNode[,] nodes)
        {
            SizeX = sizeX;
            SizeZ = sizeZ;
            _nodes = nodes;
        }

        public MapDefinition CreateMap()
        {
            var level = new MapDefinition();

            // Lights
            level.Lights = new List<LightDefinition>();
            level.Lights.Add(new LightDefinition
            {
                Color = Colors.Transparent,
                Metadata = new LightMetadata
                {
                    Direction = new Vector3D(-3, -4, -5)
                }
            });

            level.Lights.Add(new LightDefinition
            {
                Color = Colors.Transparent,
                Metadata = new LightMetadata
                {
                    Direction = new Vector3D(3, 4, 5)
                }
            });

            // Sky box
            level.Skyboxes = new List<SkyboxDefinition>();
            level.Skyboxes.Add(new SkyboxDefinition
            {
                Color = Colors.AntiqueWhite,
                Position = new Point3D(-5000, -5000, -5000),
                Metadata = new SkyboxMetadata { Normal = new Vector3D(-3, -4, -5), Size = 10000 }
            });

            // Maze
            var mazeBuilder = new MazeBuilder();

            if (_nodes == null)
            {
                _nodes = mazeBuilder.MakeNodes(SizeX, SizeZ);
                mazeBuilder.FindSpanningTree(_nodes[0, 0]);
            }

            level.Volumes = new List<VolumeDefinition>();
            level.Volumes.AddRange(mazeBuilder.CreateMazeVolumes(_nodes));

            return level;
        }
    }
}

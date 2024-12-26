using KittyEngine.Core.Graphics.Models.Definitions;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics.Models.Builders
{
    public class ModelBuilderFromDefinition
    {
        public LayeredModel3D Build(ModelBaseDefinition definition, bool addTextures = true, Transform3D transform = null)
        {
            var level = new List<LayeredModel3D>();

            if (definition is LightDefinition)
            {
                BuildLight((LightDefinition)definition, level);
            }
            else if (definition is SkyboxDefinition)
            {
                BuildSkybox((SkyboxDefinition)definition, level, addTextures);
            }
            else if (definition is VolumeDefinition)
            {
                BuildVolume((VolumeDefinition)definition, level, addTextures, transform);
            }
            else
            {
                throw new NotSupportedException($"Definition of type {definition.GetType().Name} are not supported");
            }

            return level.First();
        }

        private void BuildLight(LightDefinition definition, List<LayeredModel3D> level)
        {
            var builder = new LightBuilder(definition.Color, level);
            builder.Create(definition.Metadata.Direction);
        }

        private void BuildSkybox(SkyboxDefinition definition, List<LayeredModel3D> level, bool addTextures)
        {
            var builder = new SkyboxBuilder(definition.Color, level);
            builder.UseBackMaterial = definition.Metadata.UseBackMaterial;
            var model = builder.Create((int)definition.Position.X, (int)definition.Position.Y, (int)definition.Position.Z, definition.Metadata.Size, definition.Metadata.Normal, addTextures? definition.Metadata.Name : string.Empty);
            model.RotateByAxisX(definition.RotationAxisX);
            model.RotateByAxisY(definition.RotationAxisY);
            model.RotateByAxisZ(definition.RotationAxisZ);
        }

        private void ApplyVolumeTexture(VolumeBuilder builder, LayeredModel3D model, VolumeFace face, TextureMetadata texture)
        {
            if (!string.IsNullOrEmpty(texture?.Name))
            {
                builder.ApplyTexture(model, face, texture.Name, texture.TileMode, texture.Stretch, texture.RatioX, texture.RatioY);
            }
        }

        private void BuildVolume(VolumeDefinition definition, List<LayeredModel3D> level, bool addTextures, Transform3D transform = null)
        {
            var builder = new VolumeBuilder(definition.Color, level);
            builder.UseBackMaterial = definition.Metadata.UseBackMaterial;
            builder.CalibrationTransform = transform;

            LayeredModel3D model = null;
            if (definition.Metadata.Opacity.HasValue)
            {
                var materialGroup = new MaterialGroup();
                var brush = new SolidColorBrush(definition.Color);
                brush.Opacity = definition.Metadata.Opacity.Value;
                materialGroup.Children.Add(new DiffuseMaterial(brush));

                model = builder.Create(definition.Position.X, definition.Position.Y, definition.Position.Z, definition.Metadata.XSize, definition.Metadata.YSize, definition.Metadata.ZSize, materialGroup);
            }
            else
            {
                model = builder.Create(definition.Position.X, definition.Position.Y, definition.Position.Z, definition.Metadata.XSize, definition.Metadata.YSize, definition.Metadata.ZSize);
            }

            if (addTextures)
            {
                ApplyVolumeTexture(builder, model, VolumeFace.Front, definition.Metadata.TextureFront);
                ApplyVolumeTexture(builder, model, VolumeFace.Back, definition.Metadata.TextureBack);
                ApplyVolumeTexture(builder, model, VolumeFace.Top, definition.Metadata.TextureTop);
                ApplyVolumeTexture(builder, model, VolumeFace.Bottom, definition.Metadata.TextureBottom);
                ApplyVolumeTexture(builder, model, VolumeFace.Left, definition.Metadata.TextureLeft);
                ApplyVolumeTexture(builder, model, VolumeFace.Right, definition.Metadata.TextureRight);
            }

            model.RotateByAxisX(definition.RotationAxisX);
            model.RotateByAxisY(definition.RotationAxisY);
            model.RotateByAxisZ(definition.RotationAxisZ);
        }
    }
}

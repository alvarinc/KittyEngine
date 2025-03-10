﻿using KittyEngine.Core.GameEngine.Graphics.Assets;
using KittyEngine.Core.Graphics.Models.Definitions;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics.Models.Builders
{
    public class LightBuilder : ModelBuilder
    {
        private List<LayeredModel3D> models;

        public LightBuilder(IImageAssetProvider imageAssetProvider, Color color, List<LayeredModel3D> models) 
            : base(imageAssetProvider, color)
        {
            Color = color;
            this.models = models;
        }

        public LayeredModel3D Create(Vector3D direction)
        {
            var layeredModel = LayeredModel3D.Create(ModelType.Light);
            SetGenericMetadata(layeredModel);
            SetSpecificMetadata(layeredModel, new LightMetadata { Direction = direction });

            var light = new DirectionalLight();
            light.Color = Color;
            light.Direction = direction;

            Model3DGroup group = new Model3DGroup();
            group.Children.Add(light);

            layeredModel.Children.Add(group);

            models.Add(layeredModel);
            return layeredModel;
        }
    }
}

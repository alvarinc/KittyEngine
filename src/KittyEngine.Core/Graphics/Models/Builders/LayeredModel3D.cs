using KittyEngine.Core.Graphics.Models.Definitions;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace KittyEngine.Core.Graphics.Models.Builders
{
    public class LayeredModel3D
    {
        private ModelVisual3D _model;
        private Model3DGroup _model3DLayer;
        private Model3DGroup _calibrationLayer;
        private Model3DGroup _scaleLayer;
        private Model3DGroup _axisXRotationLayer;
        private Model3DGroup _axisYRotationLayer;
        private Model3DGroup _axisZRotationLayer;
        private Model3DGroup _translationLayer;

        private double _axisXRotationAngle;
        private double _axisYRotationAngle;
        private double _axisZRotationAngle;

        private ModelType _type;
        private Dictionary<string, object> _metadata;

        private LayeredModel3D(ModelType type, ModelVisual3D model, Model3DGroup model3DLayer, Model3DGroup calibrationLayer, Model3DGroup scaleLayer, Model3DGroup axisXRotationLayer, Model3DGroup axisYRotationLayer, Model3DGroup axisZRotationLayer, Model3DGroup translationLayer)
        {
            _type = type;
            _metadata = new Dictionary<string, object>();
            _model = model;
            _model3DLayer = model3DLayer;
            _calibrationLayer = calibrationLayer;
            _scaleLayer = scaleLayer;
            _axisXRotationLayer = axisXRotationLayer;
            _axisYRotationLayer = axisYRotationLayer;
            _axisZRotationLayer = axisZRotationLayer;
            _translationLayer = translationLayer;
        }

        public static LayeredModel3D Create(ModelType type)
        {
            var model3DLayer = new Model3DGroup();

            // Add Calibration layer
            var calibrationLayer = new Model3DGroup();
            calibrationLayer.Children.Add(model3DLayer);

            // Add Scale layer
            var scaleLayer = new Model3DGroup();
            scaleLayer.Children.Add(calibrationLayer);

            // Add X Rotation layer
            var axisXRotationLayer = new Model3DGroup();
            axisXRotationLayer.Children.Add(scaleLayer);

            // Add Y Rotation layer
            var axisYRotationLayer = new Model3DGroup();
            axisYRotationLayer.Children.Add(axisXRotationLayer);

            // Add Z Rotation layer
            var axisZRotationLayer = new Model3DGroup();
            axisZRotationLayer.Children.Add(axisYRotationLayer);

            // Add Translation layer
            var translationLayer = new Model3DGroup();
            translationLayer.Children.Add(axisZRotationLayer);

            var model = new ModelVisual3D();
            model.Content = translationLayer;

            return new LayeredModel3D(type, model, model3DLayer, calibrationLayer, scaleLayer, axisXRotationLayer, axisYRotationLayer, axisZRotationLayer, translationLayer);
        }

        public Color GetColorFromMetadata()
        {
            Color color = Colors.Transparent;

            var key = "Generic";
            if (Metadata.ContainsKey(key))
            {
                var metadata = (ModelMetadata)Metadata[key];
                color = metadata.Color;
            }

            return color;
        }

        public Color GetColorFromModel3D()
        {
            Color selectedColor = Colors.Transparent;
            foreach (var children in Children)
            {
                var childrens = ((Model3DGroup)children).Children;
                foreach (var child in childrens)
                {
                    if (child is DirectionalLight)
                    {
                        var model = (DirectionalLight)child;
                        return model.Color;
                    }
                    else if (child is GeometryModel3D)
                    {
                        var model = (GeometryModel3D)child;
                        if (model.Material is MaterialGroup)
                        {
                            var materialGroup = (MaterialGroup)model.Material;
                            foreach (var material in materialGroup.Children)
                            {
                                if (material is DiffuseMaterial)
                                {
                                    var brush = ((DiffuseMaterial)material).Brush;
                                    if (brush is SolidColorBrush)
                                    {
                                        selectedColor = ((SolidColorBrush)brush).Color;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return selectedColor;
        }

        public void SetColor(Color color)
        {
            var materiaGroup = new MaterialGroup();
            materiaGroup.Children.Add(new DiffuseMaterial(new SolidColorBrush(color)));

            foreach (var children in Children)
            {
                var childrens = ((Model3DGroup)children).Children;
                foreach (var child in childrens)
                {
                    if (child is DirectionalLight)
                    {
                        ((DirectionalLight)child).Color = color;
                    }
                    else
                    {
                        ((GeometryModel3D)child).Material = materiaGroup;
                    }
                }
            }
        }

        public List<string> Tags
        {
            get
            {
                if (!Metadata.ContainsKey("Tags"))
                {
                    Metadata["Tags"] = new List<string>();
                }

                return (List<string>)Metadata["Tags"];
            }
        }

        public Point3D GetPosition()
        {
            var matrix = Translation.Value;
            return new Point3D(matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ);
        }

        public Vector3D GetPositionAsVector()
        {
            var matrix = Translation.Value;
            return new Vector3D(matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ);
        }

        public ModelType Type { get { return _type; } }

        public Dictionary<string, object> Metadata { get { return _metadata; } }

        public ModelVisual3D GetModel()
        {
            return _model;
        }

        public Model3DGroup WrapModel()
        {
            // Add Translation layer
            var wrapperLayer = new Model3DGroup();
            wrapperLayer.Children.Add(_translationLayer);

            return wrapperLayer;
        }

        public Model3DCollection Children
        {
            get { return _model3DLayer.Children; }
        }

        public double AxisXRotationAngle
        {
            get { return _axisXRotationAngle; }
        }

        public void Transform(Point3D[] points)
        {
            _calibrationLayer.Transform.Transform(points);
            _scaleLayer.Transform.Transform(points);
            _axisXRotationLayer.Transform.Transform(points);
            _axisYRotationLayer.Transform.Transform(points);
            _axisZRotationLayer.Transform.Transform(points);
            _translationLayer.Transform.Transform(points);
        }

        public void Calibrate(double scale, Vector3D vector)
        {
            var translateTransform = new TranslateTransform3D(vector.X, vector.Y, vector.Z);
            var scaleTransform = new ScaleTransform3D(scale, scale, scale);

            var transformGroup = new Transform3DGroup();
            transformGroup.Children.Add(scaleTransform);
            transformGroup.Children.Add(translateTransform);

            _calibrationLayer.Transform = transformGroup;
        }

        public void Scale(double scale)
        {
            _scaleLayer.Transform = TransformHelper.Scale(scale, GetPosition());
        }

        public void RotateByAxisX(double angle)
        {
            _axisXRotationAngle = TransformHelper.NormalizeAngle(angle);
            _axisXRotationLayer.Transform = TransformHelper.TransformByXAxisRotation(_axisXRotationAngle, this);
        }

        public void RotateByAxisX(double angle, Point3D origin)
        {
            _axisXRotationAngle = TransformHelper.NormalizeAngle(angle);
            _axisXRotationLayer.Transform = TransformHelper.TransformByXAxisRotation(_axisXRotationAngle, origin);
        }

        public double AxisYRotationAngle
        {
            get { return _axisYRotationAngle; }
        }

        public void RotateByAxisY(double angle)
        {
            _axisYRotationAngle = TransformHelper.NormalizeAngle(angle);
            _axisYRotationLayer.Transform = TransformHelper.TransformByYAxisRotation(_axisYRotationAngle, this);
        }

        public void RotateByAxisY(double angle, Point3D origin)
        {
            _axisYRotationAngle = TransformHelper.NormalizeAngle(angle);
            _axisYRotationLayer.Transform = TransformHelper.TransformByYAxisRotation(_axisYRotationAngle, origin);
        }

        public double AxisZRotationAngle
        {
            get { return _axisZRotationAngle; }
        }

        public void RotateByAxisZ(double angle)
        {
            _axisZRotationAngle = TransformHelper.NormalizeAngle(angle);
            _axisZRotationLayer.Transform = TransformHelper.TransformByZAxisRotation(_axisZRotationAngle, this);
        }

        public void RotateByAxisZ(double angle, Point3D origin)
        {
            _axisZRotationAngle = TransformHelper.NormalizeAngle(angle);
            _axisZRotationLayer.Transform = TransformHelper.TransformByZAxisRotation(_axisZRotationAngle, origin);
        }

        public Transform3D Translation
        {
            get { return _translationLayer.Transform; }
            set { _translationLayer.Transform = value; }
        }

        public void TranslateOnAxisX(double offset)
        {
            TranslateDelta(new Vector3D(offset, 0, 0));
        }

        public void TranslateOnAxisY(double offset)
        {
            TranslateDelta(new Vector3D(0, offset, 0));
        }

        public void TranslateOnAxisZ(double offset)
        {
            TranslateDelta(new Vector3D(0, 0, offset));
        }

        private void TranslateDelta(Vector3D newVector)
        {
            var matrix = Translation.Value;
            var vector = new Vector3D(matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ);
            vector += newVector;

            matrix.OffsetX = Math.Round(vector.X, 4);
            matrix.OffsetY = Math.Round(vector.Y, 4);
            matrix.OffsetZ = Math.Round(vector.Z, 4);

            Translation = new MatrixTransform3D(matrix);
        }

        public void Translate(Vector3D vector)
        {
            var matrix = Matrix3D.Identity;

            matrix.OffsetX = Math.Round(vector.X, 4);
            matrix.OffsetY = Math.Round(vector.Y, 4);
            matrix.OffsetZ = Math.Round(vector.Z, 4);

            Translation = new MatrixTransform3D(matrix);
        }
    }
}

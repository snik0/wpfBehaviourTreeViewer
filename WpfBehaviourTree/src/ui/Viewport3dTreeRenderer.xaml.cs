using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfBehaviourTree.src.ui
{
    /// <summary>
    /// Interaction logic for Viewport3d.xaml
    /// </summary>
    public partial class Viewport3dTreeRenderer : UserControl
    {
        public Viewport3dTreeRenderer()
        {
            InitializeComponent();

            var position = new Point3D(0, 0, 0);
            float height = 0.125f;
            var labelModel = CreateTextLabel3D("texttext", new SolidColorBrush(Colors.Black), height, position);

            // add geometry to viewport
            ui_viewport.Children.Add(labelModel);
        }

        /// <summary>
        /// Creates a ModelVisual3D containing a text label.
        /// Based on tutorial by Eric Sink https://ericsink.com/wpf3d
        /// </summary>
        /// <param name="in_text">Display string</param>
        /// <param name="in_textColor">Text foreground colour</param>
        /// <param name="in_height">Character height</param>
        /// <param name="in_centerPoint">Label center</param>
        /// <param name="in_horizontalDir">Horizontal direction of the label</param>
        /// <param name="in_verticalDir">Vertical direction of the label</param>
        /// <returns>Model to be added to Viewport3D</returns>
        public static ModelVisual3D CreateTextLabel3D(
            string in_text,
            Brush in_textColor,
            double in_height,
            Point3D in_centerPoint,
            Vector3D in_horizontalDir,
            Vector3D in_verticalDir)
        {
            // build textblock containing label text
            TextBlock tb = new TextBlock(new Run(in_text));
            tb.Foreground = in_textColor;
            tb.FontFamily = new FontFamily("Arial");

            // use TextBlock as the brush for a material
            DiffuseMaterial mat = new DiffuseMaterial();
            mat.Brush = new VisualBrush(tb);

            // assume characters are square
            double width = in_text.Length * in_height;

            in_horizontalDir.Normalize();
            in_verticalDir.Normalize();

            // p0 lower left corner
            // p1 upper left
            // p2 lower right
            // p3 upper right
            Point3D p0 = in_centerPoint - width / 2 * in_horizontalDir - in_height / 2 * in_verticalDir;
            Point3D p1 = p0 + in_verticalDir * 1 * in_height;
            Point3D p2 = p0 + in_horizontalDir * width;
            Point3D p3 = p0 + in_verticalDir * 1 * in_height + in_horizontalDir * width;
            
            MeshGeometry3D mg = new MeshGeometry3D();
            mg.Positions = new Point3DCollection();
            mg.Positions.Add(p0);
            mg.Positions.Add(p1);
            mg.Positions.Add(p2);
            mg.Positions.Add(p3);
            
            mg.TriangleIndices.Add(0);
            mg.TriangleIndices.Add(1);
            mg.TriangleIndices.Add(3);
            mg.TriangleIndices.Add(0);
            mg.TriangleIndices.Add(3);
            mg.TriangleIndices.Add(2);
            
            mg.TextureCoordinates.Add(new Point(1, 1));
            mg.TextureCoordinates.Add(new Point(1, 0));
            mg.TextureCoordinates.Add(new Point(0, 1));
            mg.TextureCoordinates.Add(new Point(0, 0));

            ModelVisual3D mv3d = new ModelVisual3D();
            mv3d.Content = new GeometryModel3D(mg, mat);
            return mv3d;
        }

        // utility overload
        public static ModelVisual3D CreateTextLabel3D(
            string in_text,
            Brush in_textColor,
            double in_height,
            Point3D in_centerPoint)
        {
            return CreateTextLabel3D(in_text, in_textColor, in_height, in_centerPoint, new Vector3D(1, 0, 0), new Vector3D(0, 1, 0));
        }
}

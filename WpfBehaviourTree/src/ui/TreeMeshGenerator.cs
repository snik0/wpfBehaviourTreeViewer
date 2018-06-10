using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfBehaviourTree.src.ui
{
    static class TreeMeshGenerator
    {
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

            // assume characters are 0.75 x 1.0
            double width = in_text.Length * in_height * 0.75f;

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
            mg.TriangleIndices.Add(3);
            mg.TriangleIndices.Add(1);
            mg.TriangleIndices.Add(0);
            mg.TriangleIndices.Add(2);
            mg.TriangleIndices.Add(3);

            mg.TextureCoordinates.Add(new Point(0, 1));
            mg.TextureCoordinates.Add(new Point(0, 0));
            mg.TextureCoordinates.Add(new Point(1, 1));
            mg.TextureCoordinates.Add(new Point(1, 0));

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

        public static ModelVisual3D CreateCubeMesh3D(Point3D in_centerPoint, double in_size)
        {
            DiffuseMaterial mat = new DiffuseMaterial(new SolidColorBrush(Colors.White));

            MeshGeometry3D mg = new MeshGeometry3D();
            mg.Positions = new Point3DCollection();
            mg.Positions.Add(new Point3D(-in_size + in_centerPoint.X, -in_size + in_centerPoint.Y, in_size + in_centerPoint.Z));
            mg.Positions.Add(new Point3D(in_size + in_centerPoint.X, -in_size + in_centerPoint.Y, in_size + in_centerPoint.Z));
            mg.Positions.Add(new Point3D(in_size + in_centerPoint.X, in_size + in_centerPoint.Y, in_size + in_centerPoint.Z));
            mg.Positions.Add(new Point3D(-in_size + in_centerPoint.X, in_size + in_centerPoint.Y, in_size + in_centerPoint.Z));
            mg.Positions.Add(new Point3D(-in_size + in_centerPoint.X, -in_size + in_centerPoint.Y, -in_size + in_centerPoint.Z));
            mg.Positions.Add(new Point3D(in_size + in_centerPoint.X, -in_size + in_centerPoint.Y, -in_size + in_centerPoint.Z));
            mg.Positions.Add(new Point3D(in_size + in_centerPoint.X, in_size + in_centerPoint.Y, -in_size + in_centerPoint.Z));
            mg.Positions.Add(new Point3D(-in_size + in_centerPoint.X, in_size + in_centerPoint.Y, -in_size + in_centerPoint.Z));

            mg.TriangleIndices.Add(0); mg.TriangleIndices.Add(1); mg.TriangleIndices.Add(2);
            mg.TriangleIndices.Add(2); mg.TriangleIndices.Add(3); mg.TriangleIndices.Add(0);
            mg.TriangleIndices.Add(1); mg.TriangleIndices.Add(5); mg.TriangleIndices.Add(6);
            mg.TriangleIndices.Add(6); mg.TriangleIndices.Add(2); mg.TriangleIndices.Add(1);
            mg.TriangleIndices.Add(7); mg.TriangleIndices.Add(6); mg.TriangleIndices.Add(5);
            mg.TriangleIndices.Add(5); mg.TriangleIndices.Add(4); mg.TriangleIndices.Add(7);
            mg.TriangleIndices.Add(4); mg.TriangleIndices.Add(0); mg.TriangleIndices.Add(3);
            mg.TriangleIndices.Add(3); mg.TriangleIndices.Add(7); mg.TriangleIndices.Add(4);
            mg.TriangleIndices.Add(4); mg.TriangleIndices.Add(5); mg.TriangleIndices.Add(1);
            mg.TriangleIndices.Add(1); mg.TriangleIndices.Add(0); mg.TriangleIndices.Add(4);
            mg.TriangleIndices.Add(3); mg.TriangleIndices.Add(2); mg.TriangleIndices.Add(6);
            mg.TriangleIndices.Add(6); mg.TriangleIndices.Add(7); mg.TriangleIndices.Add(3);
            /*
            mg.TextureCoordinates.Add(new Point(1, 1));
            mg.TextureCoordinates.Add(new Point(1, 0));
            mg.TextureCoordinates.Add(new Point(0, 1));
            mg.TextureCoordinates.Add(new Point(0, 0));*/

            ModelVisual3D mv3d = new ModelVisual3D();
            mv3d.Content = new GeometryModel3D(mg, mat);
            return mv3d;
        }


        public static ModelVisual3D TestCreateCubeMesh3D(double in_size)
        {
            DiffuseMaterial mat = new DiffuseMaterial(new SolidColorBrush(Colors.White));

            MeshGeometry3D mg = new MeshGeometry3D();
            mg.Positions = new Point3DCollection();
            mg.Positions.Add(new Point3D(-in_size, -in_size, in_size));
            mg.Positions.Add(new Point3D(in_size, -in_size, in_size));
            mg.Positions.Add(new Point3D(in_size, in_size, in_size));
            mg.Positions.Add(new Point3D(-in_size, in_size, in_size));
            mg.Positions.Add(new Point3D(-in_size, -in_size, -in_size));
            mg.Positions.Add(new Point3D(in_size, -in_size, -in_size));
            mg.Positions.Add(new Point3D(in_size, in_size, -in_size));
            mg.Positions.Add(new Point3D(-in_size, in_size, -in_size));

            mg.TriangleIndices.Add(0); mg.TriangleIndices.Add(1); mg.TriangleIndices.Add(2);
            mg.TriangleIndices.Add(2); mg.TriangleIndices.Add(3); mg.TriangleIndices.Add(0);
            mg.TriangleIndices.Add(1); mg.TriangleIndices.Add(5); mg.TriangleIndices.Add(6);
            mg.TriangleIndices.Add(6); mg.TriangleIndices.Add(2); mg.TriangleIndices.Add(1);
            mg.TriangleIndices.Add(7); mg.TriangleIndices.Add(6); mg.TriangleIndices.Add(5);
            mg.TriangleIndices.Add(5); mg.TriangleIndices.Add(4); mg.TriangleIndices.Add(7);
            mg.TriangleIndices.Add(4); mg.TriangleIndices.Add(0); mg.TriangleIndices.Add(3);
            mg.TriangleIndices.Add(3); mg.TriangleIndices.Add(7); mg.TriangleIndices.Add(4);
            mg.TriangleIndices.Add(4); mg.TriangleIndices.Add(5); mg.TriangleIndices.Add(1);
            mg.TriangleIndices.Add(1); mg.TriangleIndices.Add(0); mg.TriangleIndices.Add(4);
            mg.TriangleIndices.Add(3); mg.TriangleIndices.Add(2); mg.TriangleIndices.Add(6);
            mg.TriangleIndices.Add(6); mg.TriangleIndices.Add(7); mg.TriangleIndices.Add(3);

            ModelVisual3D mv3d = new ModelVisual3D();
            mv3d.Content = new GeometryModel3D(mg, mat);
            return mv3d;
        }

        public static List<ModelVisual3D> BuildModelVisual3DTree(TreeNode in_rootNode, float in_nodeX, float in_nodeY, float in_zPos)
        {
            var list = new List<ModelVisual3D>();

            if (in_rootNode == null)
                return list;

            // build label and mesh
            var position = new Point3D(in_nodeX, in_nodeY+0.15f, in_zPos);
            float height = 0.06f;
            var labelModel = CreateTextLabel3D(in_rootNode.type, new SolidColorBrush(Colors.Black), height, position);

            //var cube = CreateCubeMesh3D(new Point3D(in_nodeX, in_nodeY, in_zPos), 0.1f);
            var cube = TestCreateCubeMesh3D( 0.1f);

            
            cube.Transform = new TranslateTransform3D(new Vector3D(in_nodeX, in_nodeY, in_zPos));

            list.Add(labelModel);
            list.Add(cube);

            // handle subnode models
            if (in_rootNode.children.Count == 0)
                return list;

            const float k_stepFactor = 0.6f;
            float childXStep = (0.5f * in_rootNode.children.Count - 0.5f) * k_stepFactor;
            float childXStart = in_nodeX - childXStep;
            
            float childYStart = in_nodeY - 0.3f;

            // recurse for children
            for(int i = 0; i < in_rootNode.children.Count; ++i)
            {
                list.AddRange(BuildModelVisual3DTree(in_rootNode.children[i], childXStart + (i * childXStep), childYStart, in_zPos));
            }
            
            return list;
        }
    }
}

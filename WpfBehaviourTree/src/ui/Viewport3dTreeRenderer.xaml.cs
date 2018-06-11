using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

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
        }

        /// <summary>
        /// Adds ModelVisual3D object to the viewport so it can render. Applies rotation animation if the element isn't a 3d text object.
        /// </summary>
        /// <returns>deepest Y value of any child node</returns>
        internal float BuildTreeMesh()
        {
            float minY = 0.35f;
            var meshList = TreeMeshGenerator.BuildModelVisual3DTree(((MainWindow)Application.Current.MainWindow).RootTreeNode, 0f, minY, 0f, ref minY);
            
            ui_viewport.Children.Clear();

            // re-add light as we just burned it            
            ui_viewport.Children.Add(new ModelVisual3D() { Content = new DirectionalLight(Colors.White, new Vector3D(3, -4, -5)) });
            
            // add children to viewport and apply animation
            foreach (var entry in meshList)
            {
                ui_viewport.Children.Add(entry);

                // sneaky sneaky $10, we're world transforming the text manually for now, skip anims
                if (entry.Transform == Transform3D.Identity)                
                    continue;                

                Transform3D currentTransform = entry.Transform.Clone();

                DoubleAnimation angleAnimation = new DoubleAnimation(180, TimeSpan.FromSeconds(2));
                angleAnimation.RepeatBehavior = RepeatBehavior.Forever;
                
                AxisAngleRotation3D rotateAxis = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);
                RotateTransform3D rotationTransform = new RotateTransform3D(rotateAxis);

                // use a transform group to reorder the rotation to be local axis
                Transform3DGroup transform3DGroup = new Transform3DGroup();
                
                transform3DGroup.Children.Add(rotationTransform);
                transform3DGroup.Children.Add(currentTransform);

                entry.Transform = transform3DGroup;
                rotateAxis.BeginAnimation(AxisAngleRotation3D.AngleProperty, angleAnimation);
            }

            return minY;
        }
    }
}

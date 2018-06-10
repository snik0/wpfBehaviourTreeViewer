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
using System.Windows.Media.Animation;
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
        }

        /// <summary>
        /// Adds ModelVisual3D object to the viewport so it can render. Applies rotation animation if the element isn't a 3d text object.
        /// </summary>
        internal void BuildTreeMesh()
        {
            var meshList = TreeMeshGenerator.BuildModelVisual3DTree(((MainWindow)Application.Current.MainWindow).RootTreeNode, 0f, 0.35f, 0f);

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
        }
    }
}

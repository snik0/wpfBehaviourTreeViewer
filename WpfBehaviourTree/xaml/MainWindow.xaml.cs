using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using WpfBehaviourTree.src;

namespace WpfBehaviourTree
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal TreeNode RootTreeNode { get; set; }


        public MainWindow()
        {
            InitializeComponent();
        }

        private TreeNode ReadRootNode(string in_filename)
        {
            try
            {
                using (StreamReader reader = new StreamReader(in_filename))
                {
                    string content = reader.ReadToEnd();

                    return TreeParser.Parse(content);                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[Error] Reading file: " + e.Message);
            }

            return null;
        }

        private void buttonViewer_Click(object sender, RoutedEventArgs e)
        {
            ui_paneStart.Visibility = Visibility.Hidden;
            ui_paneJsonViewer.Visibility = Visibility.Visible;
        }

        private void buttonCreator_Click(object sender, RoutedEventArgs e)
        {
            //ui_paneStart.Visibility = Visibility.Hidden;
        }

        private void buttonLoadJson_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".json";
            dlg.Filter = "JSON Files (*.json)|*.json|TXT Files (*.txt)|*.txt";

            // display
            var result = dlg.ShowDialog();

            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;

                RootTreeNode = ReadRootNode(filename);

                ui_mainWindowTitle.Title = System.IO.Path.GetFileName(filename);

                if (RootTreeNode != null)
                {
                    // not found a classier way of displaying the root node as items source needs an ienumerable
                    List<TreeNode> rootContainer = new List<TreeNode>(1);
                    rootContainer.Add(RootTreeNode);
                    ui_treeView.ItemsSource = rootContainer;
                }

                // build the 3d graph
                float minY = ui_treeRenderer.BuildTreeMesh();

                // clunky zoom out for now
                if (minY < -0.6)
                    ui_treeRenderer.ui_3dCamera.Position = new System.Windows.Media.Media3D.Point3D(0, 0, 4.5);
            }
        }

        private void buttonReturn_Click(object sender, RoutedEventArgs e)
        {
            ui_paneStart.Visibility = Visibility.Visible;
            ui_paneJsonViewer.Visibility = Visibility.Hidden;
        }
    }
}

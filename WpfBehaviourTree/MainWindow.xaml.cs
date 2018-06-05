using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace WpfBehaviourTree
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click_Open(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            
            // Set filter for file extension and default file extension 
            //dlg.DefaultExt = ".";
            //dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            // display
            var result = dlg.ShowDialog();
            
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;

                string fileContent = ReadFile(filename);

                MainWindowTitle.Title = System.IO.Path.GetFileName(filename);
            }
        }

        private string ReadFile(string in_filename)
        {
            string content = string.Empty;

            try
            {
                using (StreamReader reader = new StreamReader(in_filename))
                {
                    content = reader.ReadToEnd();
                    Console.WriteLine(content);

                    TreeParser.Parse(content);                   
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[Error] Reading file: " + e.Message);
            }

            return content;
        }
    }
}

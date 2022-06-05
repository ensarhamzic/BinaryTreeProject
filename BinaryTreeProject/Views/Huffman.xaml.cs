using BinaryTreeProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace BinaryTreeProject.Views
{
    /// <summary>
    /// Interaction logic for Huffman.xaml
    /// </summary>
    public partial class Huffman : UserControl
    {
        public Huffman()
        {
            InitializeComponent();
            DataContext = this.Resources["vm"];
        }

        private void ChangeViewClick(object sender, RoutedEventArgs e)
        {
            Window win = Window.GetWindow(this);
            // Changes to view for Binary Tree
            (win.DataContext as MainViewModel).SelectedViewModel = new BinaryTreeViewModel();
        }
    }
}

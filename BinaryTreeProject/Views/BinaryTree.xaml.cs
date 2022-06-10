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
    /// Interaction logic for BinaryTree.xaml
    /// </summary>
    public partial class BinaryTree : UserControl
    {
        public BinaryTree()
        {
            InitializeComponent();
            DataContext = this.Resources["vm"];
        }

        private void NodeClick(object sender, MouseButtonEventArgs e)
        {
            var binaryTreeViewModel = DataContext as BinaryTreeViewModel;
            Grid clickedGrid = sender as Grid;
            string tagString = clickedGrid.Tag.ToString();
            int.TryParse(tagString, out int tag);
            binaryTreeViewModel.NodeClick(tag);

        }

        private void NullNodeClick(object sender, MouseButtonEventArgs e)
        {
            var binaryTreeViewModel = DataContext as BinaryTreeViewModel;
            Grid clickedGrid = sender as Grid;
            string tagString = clickedGrid.Tag.ToString();
            int.TryParse(tagString, out int tag);
            binaryTreeViewModel.NullNodeClick(tag);
        }

        private void NewNodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var binaryTreeViewModel = DataContext as BinaryTreeViewModel;
                TextBox tb = sender as TextBox;
                if (binaryTreeViewModel.AddOrUpdateNodeCommand.CanExecute(binaryTreeViewModel.NewNodeValue))
                {
                    binaryTreeViewModel.AddOrUpdateNodeCommand.Execute(binaryTreeViewModel.NewNodeValue);
                    tb.Text = "";
                    tb.Focus();
                }
            }
        }
        
        private void ZoomLevelChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var binaryTreeViewModel = DataContext as BinaryTreeViewModel;
            if (binaryTreeViewModel == null) return;
            binaryTreeViewModel.ZoomChanged();
        }

        private void ChangeViewClick(object sender, RoutedEventArgs e)
        {
            Window win = Window.GetWindow(this);
            // Changes to view for Huffman Algorithm
            (win.DataContext as MainViewModel).SelectedViewModel = new HuffmanViewModel();
        }
        
        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var binaryTreeVM = DataContext as BinaryTreeViewModel;
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var contextMenu = menuItem.Parent as ContextMenu;
                var parent = contextMenu.PlacementTarget as Grid;
                //ContextMenu menu = menuItem.ContextMenu;
                //binaryTreeVM.SelectedNullNodeId = (int)(menu.Parent as Grid).Tag;
                binaryTreeVM.SelectedNodeId = (int)parent.Tag;
                if (binaryTreeVM.DeleteButtonClickCommand.CanExecute(null))
                {
                    binaryTreeVM.DeleteButtonClickCommand.Execute(null);
                }
            }
        }

        private void ChangeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var binaryTreeVM = DataContext as BinaryTreeViewModel;
            MenuItem menuItem = sender as MenuItem;
            var contextMenu = menuItem.Parent as ContextMenu;
            var parent = contextMenu.PlacementTarget as Grid;
            if (menuItem != null)
            {
                binaryTreeVM.SelectedChangeNodeId = (int)parent.Tag;
                binaryTreeVM.SelectedNullNodeId = null;
                binaryTreeVM.PopupVisible = false;
                binaryTreeVM.PopupVisible = true;
                if (binaryTreeVM.AddOrUpdateNodeCommand.CanExecute(null))
                {
                    binaryTreeVM.AddOrUpdateNodeCommand.Execute(null);
                }
            }
        }
    }
}

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
        BinaryTreeViewModel bTVM;
        public BinaryTree()
        {
            InitializeComponent();
            DataContext = this.Resources["vm"];
            bTVM = DataContext as BinaryTreeViewModel;
        }

        private void NodeClick(object sender, MouseButtonEventArgs e)
        {
            Grid clickedGrid = sender as Grid;
            string tagString = clickedGrid.Tag.ToString();
            int.TryParse(tagString, out int tag);
            bTVM.NodeClick(tag);

        }

        private void NullNodeClick(object sender, MouseButtonEventArgs e)
        {
            Grid clickedGrid = sender as Grid;
            string tagString = clickedGrid.Tag.ToString();
            int.TryParse(tagString, out int tag);
            bTVM.NullNodeClick(tag);
        }

        private void NewNodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox tb = sender as TextBox;
                if (bTVM.AddOrUpdateNodeCommand.CanExecute(bTVM.NewNodeValue))
                {
                    bTVM.AddOrUpdateNodeCommand.Execute(bTVM.NewNodeValue);
                    tb.Text = "";
                    tb.Focus();
                }
            }
        }
        
        private void ZoomLevelChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (bTVM == null) return;
            bTVM.ZoomChanged();
        }

        private void ChangeViewClick(object sender, RoutedEventArgs e)
        {
            Window win = Window.GetWindow(this);
            var vm = win.DataContext as MainViewModel;
            // Changes to view for Huffman Algorithm
            BinaryTreeViewModel.SavedBTVM = bTVM;
            vm.SelectedViewModel = new HuffmanViewModel();
        }
        
        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var contextMenu = menuItem.Parent as ContextMenu;
                var parent = contextMenu.PlacementTarget as Grid;
                //ContextMenu menu = menuItem.ContextMenu;
                //binaryTreeVM.SelectedNullNodeId = (int)(menu.Parent as Grid).Tag;
                bTVM.SelectedNodeId = (int)parent.Tag;
                if (bTVM.DeleteButtonClickCommand.CanExecute(null))
                {
                    bTVM.DeleteButtonClickCommand.Execute(null);
                }
            }
        }

        private void ChangeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            var contextMenu = menuItem.Parent as ContextMenu;
            var parent = contextMenu.PlacementTarget as Grid;
            if (menuItem != null)
            {
                bTVM.SelectedChangeNodeId = (int)parent.Tag;
                bTVM.SelectedNullNodeId = null;
                NewNodeTB.Text = bTVM.Nodes.First(x => x.ID == bTVM.SelectedChangeNodeId).Value.ToString();
                bTVM.PopupVisible = false;
                bTVM.PopupVisible = true;
                if (bTVM.AddOrUpdateNodeCommand.CanExecute(null))
                {
                    bTVM.AddOrUpdateNodeCommand.Execute(null);
                }
            }
        }
    }
}

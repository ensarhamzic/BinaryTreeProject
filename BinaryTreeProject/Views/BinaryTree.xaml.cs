using BinaryTreeProject.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        // Handling click on node
        private void NodeClick(object sender, MouseButtonEventArgs e)
        {
            Grid clickedGrid = sender as Grid;
            string tagString = clickedGrid.Tag.ToString();
            int.TryParse(tagString, out int tag);
            bTVM.NodeClick(tag);
        }

        // Handling click on node where new node can be added
        private void NullNodeClick(object sender, MouseButtonEventArgs e)
        {
            Grid clickedGrid = sender as Grid;
            string tagString = clickedGrid.Tag.ToString();
            int.TryParse(tagString, out int tag);
            bTVM.NullNodeClick(tag);
            Task.Delay(100).ContinueWith(_ =>
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    NewNodeTB.Focus();
                }));
            });
        }

        private void NewNodeTB_KeyDown(object sender, KeyEventArgs e)
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

        // Changes to view for Huffman algorithm
        private void ChangeViewClick(object sender, RoutedEventArgs e)
        {
            Window win = Window.GetWindow(this);
            var vm = win.DataContext as MainViewModel;
            // saving current state so we load it again when user goes back to binary tree
            BinaryTreeViewModel.SavedBTVM = bTVM;
            vm.SelectedViewModel = new HuffmanViewModel();
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            var contextMenu = menuItem.Parent as ContextMenu;
            var parent = contextMenu.PlacementTarget as Grid;
            bTVM.SelectedNodeId = (int)parent.Tag;
            if (bTVM.DeleteButtonClickCommand.CanExecute(null))
                bTVM.DeleteButtonClickCommand.Execute(null);
        }

        private void ChangeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            var contextMenu = menuItem.Parent as ContextMenu;
            var parent = contextMenu.PlacementTarget as Grid;
            bTVM.SelectedChangeNodeId = (int)parent.Tag;
            bTVM.SelectedNullNodeId = null;
            NewNodeTB.Text = bTVM.Nodes.First(x => x.ID == bTVM.SelectedChangeNodeId).Value.ToString();
            if(bTVM.PopupVisible)
                bTVM.PopupVisible = false;
            bTVM.PopupVisible = true;
            Task.Delay(100).ContinueWith(_ =>
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    NewNodeTB.SelectAll();
                    NewNodeTB.Focus();
                }));
            });
        }

        private void MoreInfoClick(object sender, RoutedEventArgs e)
        {
            string path = $"{Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName)}\\Resources\\BinaryTree_Info.pdf";
            Process.Start(path);
        }
    }
}

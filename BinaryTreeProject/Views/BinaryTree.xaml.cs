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
using System.Windows.Shapes;

namespace BinaryTreeProject.Views
{
    /// <summary>
    /// Interaction logic for BinaryTree.xaml
    /// </summary>
    public partial class BinaryTree : Window
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
                if (binaryTreeViewModel.AddNewNodeCommand.CanExecute(binaryTreeViewModel.NewNodeValue))
                {
                    binaryTreeViewModel.AddNewNodeCommand.Execute(binaryTreeViewModel.NewNodeValue);
                    tb.Text = "";
                    tb.Focus();
                }
            }
        }

    }
}

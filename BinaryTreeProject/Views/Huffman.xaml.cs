using BinaryTreeProject.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BinaryTreeProject.Views
{
    /// <summary>
    /// Interaction logic for Huffman.xaml
    /// </summary>
    public partial class Huffman : UserControl
    {
        HuffmanViewModel hVM;
        public Huffman()
        {
            InitializeComponent();
            DataContext = this.Resources["vm"];
            hVM = DataContext as HuffmanViewModel;
        }

        // Changes to view for Binary Tree
        private void ChangeViewClick(object sender, RoutedEventArgs e)
        {
            if (hVM.IsRunning) return;
            Window win = Window.GetWindow(this);
            var vm = win.DataContext as MainViewModel;
            // Saving current algorithm state to go back to it later if needed
            HuffmanViewModel.SavedHVM = hVM;
            vm.SelectedViewModel = new BinaryTreeViewModel();
        }

        private void MoreInfoClick(object sender, RoutedEventArgs e)
        {
            string path = @"..\..\Resources\Huffman_Info.pdf";
            Process.Start(path);
        }

        private void ViewLoaded(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            win.Title = "Huffman coding";
            win.Icon = new BitmapImage(new Uri(@"../../Icons/h.png", UriKind.RelativeOrAbsolute));
        }
    }
}

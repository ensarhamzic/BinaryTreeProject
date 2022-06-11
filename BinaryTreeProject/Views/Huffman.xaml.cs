using BinaryTreeProject.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            Window win = Window.GetWindow(this);
            var vm = win.DataContext as MainViewModel;
            // Saving current algorithm state to go back to it later if needed
            HuffmanViewModel.SavedHVM = hVM;
            vm.SelectedViewModel = new BinaryTreeViewModel();
        }

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
                if (hVM.NextStepCommand.CanExecute(null))
                    hVM.NextStepCommand.Execute(null);
            else if(e.Key == Key.Left)
                if (hVM.PreviousStepCommand.CanExecute(null))
                    hVM.PreviousStepCommand.Execute(null);
        }
    }
}

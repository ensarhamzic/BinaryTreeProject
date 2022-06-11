using System;
using System.Windows.Input;

namespace BinaryTreeProject.ViewModels.Commands
{
    internal class CancelAddCommand : ICommand
    {
        private BinaryTreeViewModel binaryTreeViewModel;
        public CancelAddCommand(BinaryTreeViewModel binaryTreeViewModel)
        {
            this.binaryTreeViewModel = binaryTreeViewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            binaryTreeViewModel.SelectedNullNodeId = null;
            binaryTreeViewModel.SelectedChangeNodeId = null;
            binaryTreeViewModel.InputVisible = false;
            binaryTreeViewModel.PopupVisible = false;
        }
    }
}

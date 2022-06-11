using System;
using System.Windows.Input;

namespace BinaryTreeProject.ViewModels.Commands
{
    internal class TreeFromFileCommand : ICommand
    {
        private BinaryTreeViewModel binaryTreeViewModel;
        public TreeFromFileCommand(BinaryTreeViewModel binaryTreeViewModel)
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
            binaryTreeViewModel.LoadTreeFromFile();
        }
    }
}

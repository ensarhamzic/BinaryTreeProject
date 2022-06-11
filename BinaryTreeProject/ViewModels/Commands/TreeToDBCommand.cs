using System;
using System.Windows.Input;

namespace BinaryTreeProject.ViewModels.Commands
{
    internal class TreeToDBCommand : ICommand
    {
        private BinaryTreeViewModel binaryTreeViewModel;
        public TreeToDBCommand(BinaryTreeViewModel binaryTreeViewModel)
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
            if(binaryTreeViewModel.BinaryTree.Root == null)
                return false;
            return true;
        }

        public void Execute(object parameter)
        {
            binaryTreeViewModel.SaveTreeToDB();
        }
    }
}

using System;
using System.Windows.Input;

namespace BinaryTreeProject.ViewModels.Commands
{
    internal class ClosePopupCommand : ICommand
    {
        private BinaryTreeViewModel binaryTreeVM;
        public ClosePopupCommand(BinaryTreeViewModel binaryTreeVM)
        {
            this.binaryTreeVM = binaryTreeVM;
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
            binaryTreeVM.ClosePopup();
        }
    }
}

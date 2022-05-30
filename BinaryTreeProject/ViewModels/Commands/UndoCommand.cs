using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BinaryTreeProject.ViewModels.Commands
{
    internal class UndoCommand : ICommand
    {
        private BinaryTreeViewModel binaryTreeViewModel;
        public UndoCommand(BinaryTreeViewModel binaryTreeViewModel)
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
            if (binaryTreeViewModel.UndoStack.Count > 0)
                return true;
            return false;
        }

        public void Execute(object parameter)
        {
            binaryTreeViewModel.Undo();
        }
    }
}

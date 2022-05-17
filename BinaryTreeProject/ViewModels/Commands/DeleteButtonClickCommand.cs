using BinaryTreeProject.Models;
using BinaryTreeProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BinaryTreeProject.ViewModels.Commands
{
    internal class DeleteButtonClickCommand : ICommand
    {
        private BinaryTreeViewModel binaryTreeViewModel;
        public DeleteButtonClickCommand(BinaryTreeViewModel binaryTreeViewModel)
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
            if (binaryTreeViewModel.SelectedNodeId == null)
            {
                return false;
            }
            return true;
        }

        public void Execute(object parameter)
        {
            int nodeId = (int)binaryTreeViewModel.SelectedNodeId;
            Node nodeToDelete = binaryTreeViewModel.Nodes.First(n => n.ID == nodeId);
            binaryTreeViewModel.DeleteNode(nodeToDelete);
        }
    }
}

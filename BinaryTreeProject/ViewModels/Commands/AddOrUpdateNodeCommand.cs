using BinaryTreeProject.Models;
using System;
using System.Windows.Input;

namespace BinaryTreeProject.ViewModels.Commands
{
    internal class AddOrUpdateNodeCommand : ICommand
    {
        private BinaryTreeViewModel binaryTreeVM;
        public AddOrUpdateNodeCommand(BinaryTreeViewModel binaryTreeVM)
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
            string value = parameter as string;
            int number;
            // TODO: number check temporary here (till I fix the issue with numbers overflow from node in the view)
            bool isValid = int.TryParse(value, out number) && (number > -1000 && number < 1000); 
            return (isValid && (binaryTreeVM.SelectedNullNodeId != null)) || (isValid && binaryTreeVM.NullNodes.Count == 0)
                || (isValid && binaryTreeVM.SelectedChangeNodeId != null);
        }

        public void Execute(object parameter)
        {
            int value = int.Parse(parameter.ToString());
            if(binaryTreeVM.SelectedChangeNodeId != null)
            {
                binaryTreeVM.AddOrUpdateNode(null, value, '/', true);
                return;
            }
            char side;
            Node parentNode;
            Node selectedNullNode = binaryTreeVM.NullNodes[(int)binaryTreeVM.SelectedNullNodeId - 1];
            if (selectedNullNode.LeftNode == null
                && selectedNullNode.RightNode == null)
            {
                parentNode = null;
                side = '/'; // doesn't matter
            }
            else if (selectedNullNode.LeftNode != null)
            {
                parentNode = selectedNullNode.LeftNode;
                side = 'L';
            }
            else
            {
                parentNode = selectedNullNode.RightNode;
                side = 'R';
            }
            binaryTreeVM.AddOrUpdateNode(parentNode, value, side);
        }
    }
}

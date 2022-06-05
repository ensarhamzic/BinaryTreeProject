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
    internal class AddNodeCommand : ICommand
    {
        private BinaryTreeViewModel binaryTreeViewModel;
        public AddNodeCommand(BinaryTreeViewModel binaryTreeViewModel)
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
            string value = parameter as string;
            int number;
            // TODO: number check temporary here (till I fix the issue with numbers overflow from node in the view)
            bool isValid = int.TryParse(value, out number) && (number > -1000 && number < 10000); 
            return (isValid && (binaryTreeViewModel.SelectedNullNodeId != null)) || (isValid && binaryTreeViewModel.NullNodes.Count == 0);
        }

        public void Execute(object parameter)
        {
            string value = parameter as string;
            char side;
            Node parentNode;
            if(binaryTreeViewModel.SelectedNullNodeId == null)
            {
                parentNode = null;
                side = '/'; // doesn't matter
            }
            else if (binaryTreeViewModel.NullNodes[(int)binaryTreeViewModel.SelectedNullNodeId-1].LeftNode != null)
            {
                parentNode = binaryTreeViewModel.NullNodes[(int)binaryTreeViewModel.SelectedNullNodeId - 1].LeftNode;
                side = 'L';
            }
            else
            {
                parentNode = binaryTreeViewModel.NullNodes[(int)binaryTreeViewModel.SelectedNullNodeId - 1].RightNode;
                side = 'R';
            }
            binaryTreeViewModel.AddNode(parentNode, int.Parse(value), side);
        }
    }
}

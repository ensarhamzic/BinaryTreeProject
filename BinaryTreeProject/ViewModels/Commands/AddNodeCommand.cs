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
        private BinaryTreeViewModel binaryTreeVM;
        public AddNodeCommand(BinaryTreeViewModel binaryTreeVM)
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
            return (isValid && (binaryTreeVM.SelectedNullNodeId != null)) || (isValid && binaryTreeVM.NullNodes.Count == 0);
        }

        public void Execute(object parameter)
        {
            string value = parameter as string;
            char side;
            Node parentNode;
            if(binaryTreeVM.SelectedNullNodeId == null
                || (binaryTreeVM.NullNodes[(int)binaryTreeVM.SelectedNullNodeId - 1].LeftNode == null
                && binaryTreeVM.NullNodes[(int)binaryTreeVM.SelectedNullNodeId - 1].RightNode == null))
            {
                parentNode = null;
                side = '/'; // doesn't matter
            }
            else if (binaryTreeVM.NullNodes[(int)binaryTreeVM.SelectedNullNodeId-1].LeftNode != null)
            {
                parentNode = binaryTreeVM.NullNodes[(int)binaryTreeVM.SelectedNullNodeId - 1].LeftNode;
                side = 'L';
            }
            else
            {
                parentNode = binaryTreeVM.NullNodes[(int)binaryTreeVM.SelectedNullNodeId - 1].RightNode;
                side = 'R';
            }
            binaryTreeVM.AddNode(parentNode, int.Parse(value), side);
        }
    }
}

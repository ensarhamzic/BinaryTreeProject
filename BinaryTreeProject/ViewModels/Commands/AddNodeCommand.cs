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
            return int.TryParse(value, out _);
        }

        public void Execute(object parameter)
        {
            string value = parameter as string;
            binaryTreeViewModel.BinaryTree.AddNode(binaryTreeViewModel.BinaryTree.Root, int.Parse(value));
        }
    }
}

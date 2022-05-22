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
    internal class SaveTreeCommand : ICommand
    {
        private BinaryTreeViewModel binaryTreeViewModel;
        public SaveTreeCommand(BinaryTreeViewModel binaryTreeViewModel)
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
            /* if(binaryTreeViewModel.BinaryTree.Root == null)
                return false; */
            return true;
        }

        public void Execute(object parameter)
        {
            binaryTreeViewModel.SaveTreeToFile();
        }
    }
}

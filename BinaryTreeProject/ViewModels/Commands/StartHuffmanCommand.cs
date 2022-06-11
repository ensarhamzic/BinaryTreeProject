using System;
using System.Windows.Input;

namespace BinaryTreeProject.ViewModels.Commands
{
    public class StartHuffmanCommand : ICommand
    {
        private HuffmanViewModel huffmanVM;
        public StartHuffmanCommand(HuffmanViewModel huffmanVM)
        {
            this.huffmanVM = huffmanVM;
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
            if (string.IsNullOrEmpty(huffmanVM.EnteredText))
                return false;
            return true;
        }
        
        public void Execute(object parameter)
        {
            huffmanVM.StartHuffman();
        }
    }
}

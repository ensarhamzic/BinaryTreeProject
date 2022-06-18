using System;
using System.Windows.Input;

namespace BinaryTreeProject.ViewModels.Commands
{
    public class NextStepHuffmanCommand : ICommand
    {
        private HuffmanViewModel huffmanVM;
        public NextStepHuffmanCommand(HuffmanViewModel huffmanVM)
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
            if (huffmanVM.IsStarted && huffmanVM.Huffman.Trees.Count > 1)
                return true;
            return false;
        }

        public void Execute(object parameter)
        {
            huffmanVM.NextStep(false);
        }
    }
}

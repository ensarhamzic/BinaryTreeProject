using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BinaryTreeProject.ViewModels.Commands
{
    public class SkipToEndHuffmanCommand : ICommand
    {
        private HuffmanViewModel huffmanVM;
        public SkipToEndHuffmanCommand(HuffmanViewModel huffmanVM)
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
            huffmanVM.SkipToEnd();
        }
    }
}

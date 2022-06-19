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
            if (string.IsNullOrEmpty(huffmanVM.EnteredText)
                || IsSameCharacterRepeated(huffmanVM.EnteredText) )
                return false;
            return true;
        }
        
        public void Execute(object parameter)
        {
            huffmanVM.StartHuffman();
        }

        private bool IsSameCharacterRepeated(string s)
        {
            char first = s[0];
            for (int i = 1; i < s.Length; i++)
                if (s[i] != first)
                    return false;
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BinaryTreeProject.Commands;
using BinaryTreeProject.Models;

namespace BinaryTreeProject.ViewModels
{
    internal class BinaryTreeViewModel : INotifyPropertyChanged
    {
        public BinaryTree BinaryTree { get; set; }


        private string newNodeValue;
        public string NewNodeValue
        {
            get { return newNodeValue; }
            set
            {
                newNodeValue = value;
                OnPropertyChanged("NewNodeValue");
            }
        }

        public ICommand AddNewNodeCommand { get; private set; }

        public BinaryTreeViewModel()
        {
            BinaryTree = new BinaryTree();
            AddNewNodeCommand = new AddNodeCommand(this);
        }





        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}

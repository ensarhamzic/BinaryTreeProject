using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BinaryTreeProject.ViewModels.Commands;
using BinaryTreeProject.Models;
using System.Collections.ObjectModel;

namespace BinaryTreeProject.ViewModels
{
    internal class BinaryTreeViewModel : INotifyPropertyChanged
    {
        public BinaryTree BinaryTree { get; set; }
        private bool inputVisible;

        public ObservableCollection<Position> NullNodesPositions { get; set; }
        public bool InputVisible
        {
            get { return inputVisible; }
            set
            {
                inputVisible = value;
                OnPropertyChanged("InputVisible");
            }
        }

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
        public ICommand AddButtonClickCommand { get; private set; }

        public ICommand CancelAddCommand { get; private set; }

        public BinaryTreeViewModel()
        {
            BinaryTree = new BinaryTree();
            AddNewNodeCommand = new AddNodeCommand(this);
            AddButtonClickCommand = new AddButtonClickCommand(this);
            CancelAddCommand = new CancelAddCommand(this);
            NullNodesPositions = new ObservableCollection<Position>();
        }

        public void AddButtonClick()
        {
            CalculateNullNodePositions();
            InputVisible = true;
        }

        private void CalculateNullNodePositions()
        {
            NullNodesPositions.Clear();
            foreach (var node in BinaryTree.Nodes)
            {
                if (node.LeftNode == null)
                {
                    NullNodesPositions.Add(new Position(node.Position.X, node.Position.Y + BinaryTree.CircleDiameter));
                }
                if (node.RightNode == null)
                {
                    NullNodesPositions.Add(new Position(node.Position.X + BinaryTree.CircleDiameter - BinaryTree.AddCircleDiameter, node.Position.Y + BinaryTree.CircleDiameter));
                }
            }
        }

        public void NodeClick(int nodeId)
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}

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
        private int? selectedNodeId;
        private int? selectedNullNodeId;

        public int? SelectedNodeId
        {
            get { return selectedNodeId; }
            set
            {
                selectedNodeId = value;
                OnPropertyChanged("SelectedNodeId");
            }
        }

        public int? SelectedNullNodeId
        {
            get { return selectedNullNodeId; }
            set
            {
                selectedNullNodeId = value;
                OnPropertyChanged("SelectedNullNodeId");
            }
        }

        public ObservableCollection<Node> NullNodes { get; set; }
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
            NullNodes = new ObservableCollection<Node>();
            SelectedNodeId = null;
            SelectedNullNodeId = null;
        }

        public void AddButtonClick()
        {
            CalculateNullNodePositions();
            InputVisible = true;
        }

        private void CalculateNullNodePositions()
        {
            NullNodes.Clear();
            int nodeId = 0;
            foreach (var node in BinaryTree.Nodes)
            {
                if (node.LeftNode == null)
                {
                    Node nullNode = new Node();
                    nullNode.ID = ++nodeId; // used for click on node
                    nullNode.LeftNode = node; // used to know if node is parents left or right node
                    nullNode.Position = new Position(node.Position.X, node.Position.Y + BinaryTree.CircleDiameter);
                    NullNodes.Add(nullNode);
                }
                
                if (node.RightNode == null)
                {
                    Node nullNode = new Node();
                    nullNode.ID = ++nodeId;
                    nullNode.ParentNode = node;
                    nullNode.RightNode = node; // used to know if node is parents left or right node
                    nullNode.Position = new Position(node.Position.X + BinaryTree.CircleDiameter - BinaryTree.AddCircleDiameter,
                        node.Position.Y + BinaryTree.CircleDiameter);
                    NullNodes.Add(nullNode);
                }
            }
        }

        public void NodeClick(int nodeId)
        {
            SelectedNodeId = nodeId;
        }

        internal void NullNodeClick(int nullNodeId)
        {
            SelectedNullNodeId = nullNodeId;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}

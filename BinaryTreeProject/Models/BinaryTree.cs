using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace BinaryTreeProject.Models
{
    internal class BinaryTree : INotifyPropertyChanged
    {
        private static int nodeId;
        private Node _root; // tree root

        public Node Root
        {
            get { return _root; }
            set
            {
                _root = value;
                OnPropertyChanged("Root");
            }
        }

        public BinaryTree()
        {
            nodeId = 0;
            Root = null;

        }

        // add node to the tree
        public void AddNode(Node parentNode, int value, char side)
        {
            Node newNode = new Node(value, ++nodeId);
            if (parentNode == null)
            {
                Root = newNode;
            }
            else if (side == 'L')
            {
                parentNode.LeftNode = newNode;
                newNode.ParentNode = parentNode;
            }
            else
            {
                parentNode.RightNode = newNode;
                newNode.ParentNode = parentNode;
            }
        }

        // TODO:  Stupid but working (must change)
        public void DeleteNode(Node nodeToDelete)
        {
            if(nodeToDelete == Root)
            {
                Root = null;
                nodeId = 0;
            }
            else if(nodeToDelete.ParentNode.LeftNode == nodeToDelete)
            {
                nodeToDelete.ParentNode.LeftNode = null;
            } else
            {
                nodeToDelete.ParentNode.RightNode = null;
            }
        }


        // Calculating max depth of the tree
        public int CalculateMaxDepth(Node node)
        {
            if (node == null)
                return -1;
            else
            {
                int lDepth = CalculateMaxDepth(node.LeftNode);
                int rDepth = CalculateMaxDepth(node.RightNode);

                if (lDepth > rDepth)
                    return (lDepth + 1);
                else
                    return (rDepth + 1);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

       
    }
}

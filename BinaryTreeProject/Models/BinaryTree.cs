using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            if (nodeToDelete == Root)
            {
                Root = null;
                nodeId = 0;
            }
            else if (nodeToDelete.ParentNode.LeftNode == nodeToDelete)
            {
                nodeToDelete.ParentNode.LeftNode = null;
            }
            else
            {
                nodeToDelete.ParentNode.RightNode = null;
            }
        }


        // Calculating max depth of the tree
        public int CalculateMaxDepth(Node node)
        {
            if (node == null)
                return 0;
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
        
        public virtual void LevelOrder(List<int?> values)
        {
            int h = CalculateMaxDepth(Root);
            int i;
            for (i = 1; i <= h; i++)
            {
                CurrentLevel(Root, values, i);
            }
        }
        
        public virtual void CurrentLevel(Node node, List<int?> values,int level)
        {
            if (node == null)
            {
                values.Add(null);
                return;
            }
            if (level == 1)
            {
                values.Add(node.Value);
            }
            else if (level > 1)
            {
                CurrentLevel(node.LeftNode, values, level - 1);
                CurrentLevel(node.RightNode, values, level - 1);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


    }
}

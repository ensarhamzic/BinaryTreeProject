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
    public class BinaryTree : INotifyPropertyChanged
    {
        public static int nodeId;
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


        public void Preorder(Node node, List<Node> values)
        {
            if (node == null)
            {
                return;
            }
            values.Add(node);
            Preorder(node.LeftNode, values);
            Preorder(node.RightNode, values);
        }

        public void Preorder(Node node, List<int?> values)
        {
            if(node == null)
            {
                values.Add(null);
                return;
            }
            values.Add(node.Value);
            Preorder(node.LeftNode, values);
            Preorder(node.RightNode, values);
        }

        // Build tree from preorder list of tree
        public int preId = 0;
        public Node BuildTreeFromPreorder(List<int?> preorder, bool first)
        {
            if(first) {
                preId = 0;
                nodeId = 0;
            }
            
            if (preId >= preorder.Count) return null;

            int? nextValue = preorder[preId++];

            if (nextValue != null)
            {
                Node node = new Node(nextValue, ++nodeId);
                node.LeftNode = BuildTreeFromPreorder(preorder, false);

                if (node.LeftNode != null)
                    node.LeftNode.ParentNode = node;

                node.RightNode = BuildTreeFromPreorder(preorder, false);
                if (node.RightNode != null)
                    node.RightNode.ParentNode = node;
                return node;
            }
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


    }
}

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

        /*public void PreOrder(Node node, List<int> values)
        {
            if (node == null) return;
            values.Add((int)node.Value);
            PreOrder(node.LeftNode, values);
            PreOrder(node.RightNode, values);
        }

        public void InOrder(Node node, List<int> values)
        {
            if (node == null) return;
            InOrder(node.LeftNode, values);
            values.Add((int)node.Value);
            InOrder(node.RightNode, values);
        } */

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
        public Node BuildTreeFromFile(List<int?> preorder, bool first)
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
                node.LeftNode = BuildTreeFromFile(preorder, false);

                if (node.LeftNode != null)
                    node.LeftNode.ParentNode = node;

                node.RightNode = BuildTreeFromFile(preorder, false);
                if (node.RightNode != null)
                    node.RightNode.ParentNode = node;
                return node;
            }
            return null;
        }






        /*
        // Build tree from given preorder and inorder traversals
        public virtual Node BuildTree(List<int> inorder, List<int> preoder, int inStrt, int inEnd)
        {
            if (inStrt > inEnd)
            {
                return null;
            }

            // Pick current node from Preorder traversal using preIndex and increment preIndex 
            Node tNode = new Node(preoder[preIndex++], ++nodeId);

            // If this node has no children then return 
            if (inStrt == inEnd)
            {
                return tNode;
            }

            // Else find the index of this node in Inorder traversal 
            int inIndex = SearchNode(inorder, inStrt, inEnd, (int)tNode.Value);

            // Using index in Inorder traversal, construct left and right subtress 
            tNode.LeftNode = BuildTree(inorder, preoder, inStrt, inIndex - 1);
            if (tNode.LeftNode != null)
                tNode.LeftNode.ParentNode = tNode;
            tNode.RightNode = BuildTree(inorder, preoder, inIndex + 1, inEnd);
            if (tNode.RightNode != null)
                tNode.RightNode.ParentNode = tNode;

            return tNode;
        }


        // Function to find index of value in arr[start...end].
        // The function assumes that value is present in in[] 
        public virtual int SearchNode(List<int> inorder, int strt, int end, int value)
        {
            int i;
            for (i = strt; i <= end; i++)
            {
                if (inorder[i] == value)
                {
                    return i;
                }
            }
            return i;
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

        public virtual void CurrentLevel(Node node, List<int?> values, int level)
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
        */

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


    }
}

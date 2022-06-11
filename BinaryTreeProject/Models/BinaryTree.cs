using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace BinaryTreeProject.Models
{
    public class BinaryTree : INotifyPropertyChanged
    {
        public int nodeId;
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

        // TODO:  Improve deleting (setting node to NULL does not work)
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
        
        // Preorder that returs list od Nodes
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

        // Preorder that returns list of nullable int values
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
        public int preId = 0; // cuurent id of node
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

        // Build tree from data taken from database
        public void BuildTreeFromDatabase(List<Node> nodes, DataTable dt)
        {
            int lastId = 0;
            for (int i = 0; i < nodes.Count; i++)
            {
                int? parentId;
                // Finds parent id
                if (dt.Rows[i]["parent_id"] == DBNull.Value)
                    parentId = null;
                else
                    parentId = Convert.ToInt32(dt.Rows[i]["parent_id"]);

                if ((int)nodes[i].ID > lastId)
                {
                    // calculated biggest id so new nodes don't have same id
                    lastId = (int)nodes[i].ID;
                }

                if (parentId == null)
                {
                    // If parent id is null, then it's root
                    Root = nodes[i];
                }
                else
                {
                    // if not root, finds parent node and adds it as child
                    nodes[i].ParentNode = nodes.Find(n => n.ID == parentId);
                    if (dt.Rows[i]["side"] as string == "L")
                        nodes[i].ParentNode.LeftNode = nodes[i];
                    else
                        nodes[i].ParentNode.RightNode = nodes[i];
                }
            }
            nodeId = lastId + 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


    }
}

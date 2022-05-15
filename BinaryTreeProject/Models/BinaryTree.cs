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
        private Node _root;
        private int maxDepth;
        private int circleDiameter;
        private int canvasWidth;

        public int CanvasWidth
        {
            get { return circleDiameter; }
            set
            {
                canvasWidth = value;
                OnPropertyChanged("CanvasWidth");
            }
        }


        public int CircleDiameter
        {
            get { return circleDiameter; }
            set
            {
                circleDiameter = value;
                OnPropertyChanged("CircleDiameter");
            }
        }


        // Mozda mi uopste ne treba
        public int MaxDepth
        {
            get { return maxDepth; }
            set
            {
                maxDepth = value;
                OnPropertyChanged("MaxDepth");
            }
        }
        public ObservableCollection<Node> Nodes { get; set; }
        public ObservableCollection<LinePosition> LinePositions { get; set; }

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
            Root = null;
            Nodes = new ObservableCollection<Node>();
            LinePositions = new ObservableCollection<LinePosition>();
            maxDepth = 0;
            CircleDiameter = 50;
        }

        public void AddNode(Node node, int value)
        {
            if (node == null)
            {
                Root = new Node(value);
            }
            else if (value < node.Value)
            {
                if (node.LeftNode == null)
                {
                    node.LeftNode = new Node(value);
                    node.LeftNode.ParentNode = node;
                }
                else
                {
                    AddNode(node.LeftNode, value);
                }
            }
            else
            {
                if (node.RightNode == null)
                {
                    node.RightNode = new Node(value);
                    node.RightNode.ParentNode = node;
                }
                else
                {
                    AddNode(node.RightNode, value);
                }
            }
            MaxDepth = CalculateMaxDepth(Root);
            UpdateNodesCollection(Root);
            CalculateNodePositions();
            UpdateLinePositions(Root);
        }

        private void CalculateNodePositions()
        {
            int lastX = 0;
            foreach (var node in Nodes)
            {
                node.Position.X = lastX + CircleDiameter * 0.7;
                lastX += (int)(CircleDiameter * 0.7);
            }
            CalculateVerticalPositions(Root);
        }

        private void UpdateLinePositions(Node node)
        {
            if (node == null)
                return;
            if (node == Root)
            {
                LinePositions.Clear();
            }

            if (node.ParentNode != null)
            {
                var line = new LinePosition();
                line.StartPosition = new Position(node.ParentNode.Position.X + CircleDiameter / 2, node.ParentNode.Position.Y + CircleDiameter);
                line.EndPosition = new Position(node.Position.X + CircleDiameter / 2, node.Position.Y);
                LinePositions.Add(line);
            }
            UpdateLinePositions(node.LeftNode);
            UpdateLinePositions(node.RightNode);
        }

        private void UpdateNodesCollection(Node node)
        {
            if (node == null)
            {
                return;
            }

            if (node == Root)
            {
                Nodes.Clear();
            }
            UpdateNodesCollection(node.LeftNode);
            Nodes.Add(node);
            UpdateNodesCollection(node.RightNode);
        }

        public void CalculateVerticalPositions(Node node)
        {

            if (node == null) return;

            if (node == Root)
            {
                // node.Position.X = 600;
                node.Position.Y = 50;
            }

            if (node.LeftNode != null)
            {
                //int depth = CalculateNodeDepth(node.LeftNode);
                // node.LeftNode.Position.X = node.Position.X - (MaxDepth + 1 - depth) * (CircleDiameter*2);
                node.LeftNode.Position.Y = node.Position.Y + (CircleDiameter + 20);
            }

            if (node.RightNode != null)
            {
                //int depth = CalculateNodeDepth(node.RightNode);
                //node.RightNode.Position.X = node.Position.X + (MaxDepth + 1 - depth) * (CircleDiameter * 2);
                node.RightNode.Position.Y = node.Position.Y + (CircleDiameter + 20);
            }

            CalculateVerticalPositions(node.LeftNode);
            CalculateVerticalPositions(node.RightNode);

        }

        private int CalculateMaxDepth(Node node)
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

        private int CalculateNodeDepth(Node node)
        {
            int depth = 0;
            while (node.ParentNode != null)
            {
                node = node.ParentNode;
                depth++;
            }
            return depth;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

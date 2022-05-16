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
        private int maxDepth; // max depth of the tree
        private int circleDiameter; // diameter of the circle representing a node
        private double canvasWidth; // width of the canvas
        private double canvasHeight; // height of the canvas
        private double verticalNodeOffset; // vertical offset between two nodes
        private double horizontalNodeOffset; // horizontal offset between two nodes
        private double addCircleDiameter; // diameter of the circle representing a node when adding a new node

        public double CanvasWidth
        {
            get { return canvasWidth; }
            set
            {
                canvasWidth = value;
                OnPropertyChanged("CanvasWidth");
            }
        }

        public double CanvasHeight
        {
            get { return canvasHeight; }
            set
            {
                canvasHeight = value;
                OnPropertyChanged("CanvasHeight");
            }
        }

        public double VerticalNodeOffset
        {
            get { return verticalNodeOffset; }
            set
            {
                verticalNodeOffset = value;
                OnPropertyChanged("verticalNodeOffset");
            }
        }

        public double HorizontalNodeOffset
        {
            get { return horizontalNodeOffset; }
            set
            {
                horizontalNodeOffset = value;
                OnPropertyChanged("horizontalNodeOffset");
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

        public double AddCircleDiameter
        {
            get { return addCircleDiameter; }
            set
            {
                addCircleDiameter = value;
                OnPropertyChanged("AddCircleDiameter");
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

        public Node Root
        {
            get { return _root; }
            set
            {
                _root = value;
                OnPropertyChanged("Root");
            }
        }

        // collection of nodes (used for binding)
        public ObservableCollection<Node> Nodes { get; set; }

        // collection of line positions (used for binding)
        public ObservableCollection<LinePosition> LinePositions { get; set; }

      

        public BinaryTree()
        {
            nodeId = 0;
            Root = null;
            Nodes = new ObservableCollection<Node>();
            LinePositions = new ObservableCollection<LinePosition>();
            MaxDepth = 0;
            CircleDiameter = 50;
            CanvasWidth = 0;
            CanvasHeight = 0;
            VerticalNodeOffset = CircleDiameter * 0.5;
            HorizontalNodeOffset = CircleDiameter * 0.7;
            AddCircleDiameter = CircleDiameter * 0.3;          

        }

        // add node to the tree
        public void AddNode(Node parentNode, int value, char side)
        {
            Node newNode = new Node(value, ++nodeId);
            if(parentNode == null)
            {
                Root = newNode;
            }
            else if(side == 'L')
            {
                parentNode.LeftNode = newNode;
                newNode.ParentNode = parentNode;
            }
            else
            {
                parentNode.RightNode = newNode;
                newNode.ParentNode = parentNode;
            }
            
            UpdateNodesCollection(Root);
            CalculateNodePositions();
            UpdateLinePositions(Root);
        }

        // Calculate positions of the nodes on the canvas
        private void CalculateNodePositions()
        {
            double startX = CalculateCanvasWidth();
            foreach(var node in Nodes)
            {
                node.Position.X = startX + HorizontalNodeOffset;
                startX += HorizontalNodeOffset;
            }
            CalculateVerticalPositions(Root);
            CalculateCanvasHeight();
        }

        // Calculate canvas width
        private double CalculateCanvasWidth()
        {
            int leftSubtreeNodes = 0;
            int rightSubtreeNodes = 0;
            bool isLeftSubtree = true;
            double startX;
            foreach (var node in Nodes)
            {
                if (isLeftSubtree && node != Root)
                {
                    leftSubtreeNodes++;
                }
                else if (node == Root)
                {
                    isLeftSubtree = false;
                }
                else
                {
                    rightSubtreeNodes++;
                }

            }
            if (leftSubtreeNodes > rightSubtreeNodes)
            {
                CanvasWidth = ((leftSubtreeNodes + 2) * HorizontalNodeOffset) * 2;
                startX = 0;
            }
            else
            {
                startX = (rightSubtreeNodes - leftSubtreeNodes) * HorizontalNodeOffset;
                if (rightSubtreeNodes == 0)
                {
                    CanvasWidth = HorizontalNodeOffset * 4;
                }
                else
                {
                    CanvasWidth = ((rightSubtreeNodes + 2) * HorizontalNodeOffset) * 2;
                }
            }

            return startX;
        }


        // Calculate canvas height
        private void CalculateCanvasHeight()
        {
            int depth = CalculateMaxDepth(Root);
            CanvasHeight = (depth + 2) * CircleDiameter + (depth + 1) * VerticalNodeOffset;
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

        // Updates nodes collection upon modifying the tree
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

        // Calculating Y positions of the nodes
        private void CalculateVerticalPositions(Node node)
        {

            if (node == null) return;

            if (node == Root)
            {
                node.Position.Y = VerticalNodeOffset;
            }

            if (node.LeftNode != null)
            {
                node.LeftNode.Position.Y = node.Position.Y + (CircleDiameter + VerticalNodeOffset);
            }

            if (node.RightNode != null)
            {
                node.RightNode.Position.Y = node.Position.Y + (CircleDiameter + VerticalNodeOffset);
            }

            CalculateVerticalPositions(node.LeftNode);
            CalculateVerticalPositions(node.RightNode);
        }

        // Calculating max depth of the tree
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

        // Calculating depth of specific node
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

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BinaryTreeProject.ViewModels.Commands;
using BinaryTreeProject.Models;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace BinaryTreeProject.ViewModels
{
    internal class BinaryTreeViewModel : INotifyPropertyChanged
    {
        private BinaryTree binaryTree { get; set; } // instance of the binary tree
        private bool inputVisible; // is add input visible
        private int? selectedNodeId; // id of existing node selected
        private int? selectedNullNodeId; // id of null (non-existing) node selected
        private string newNodeValue; // value of user input
        private int circleDiameter; // diameter of the circle representing a node
        private double canvasWidth; // width of the canvas
        private double canvasHeight; // height of the canvas
        private double verticalNodeOffset; // vertical offset between two nodes
        private double horizontalNodeOffset; // horizontal offset between two nodes
        private List<int?> nodesSaveList; // list of nodes to save



        private double addCircleDiameter; // diameter of the circle representing a node when adding a new node

        // collection of nodes (used for binding)
        public ObservableCollection<Node> Nodes { get; set; }

        // collection of line positions (used for binding)
        public ObservableCollection<LinePosition> LinePositions { get; set; }

        // collection of non-existing nodes (used for binding)
        public ObservableCollection<Node> NullNodes { get; set; }

        public BinaryTree BinaryTree
        {
            get { return binaryTree; }
            set
            {
                binaryTree = value;
                OnPropertyChanged("BinaryTree");
            }
        }

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

        public List<int?> NodesSaveList
        {
            get { return nodesSaveList; }
            set
            {
                nodesSaveList = value;
                OnPropertyChanged("NodesSaveList");
            }
        }

        public bool InputVisible
        {
            get { return inputVisible; }
            set
            {
                inputVisible = value;
                OnPropertyChanged("InputVisible");
            }
        }

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
        public ICommand DeleteButtonClickCommand { get; private set; }
        public ICommand SaveTreeCommand { get; private set; }

        public BinaryTreeViewModel()
        {
            BinaryTree = new BinaryTree();
            // Commands
            AddNewNodeCommand = new AddNodeCommand(this);
            AddButtonClickCommand = new AddButtonClickCommand(this);
            CancelAddCommand = new CancelAddCommand(this);
            DeleteButtonClickCommand = new DeleteButtonClickCommand(this);
            SaveTreeCommand = new SaveTreeCommand(this);
            // Other stuff
            NullNodes = new ObservableCollection<Node>();
            SelectedNodeId = null;
            SelectedNullNodeId = null;
            CircleDiameter = 50;
            CanvasWidth = 0;
            CanvasHeight = 0;
            VerticalNodeOffset = CircleDiameter * 0.5;
            HorizontalNodeOffset = CircleDiameter * 0.7;
            AddCircleDiameter = CircleDiameter * 0.3;
            Nodes = new ObservableCollection<Node>();
            LinePositions = new ObservableCollection<LinePosition>();
            NodesSaveList = new List<int?>();
        }

        public void AddNode(Node parentNode, int v, char side)
        {
            BinaryTree.AddNode(parentNode, v, side);
            UpdateNodesCollection(BinaryTree.Root);
            CalculateNodePositions();
            UpdateLinePositions(BinaryTree.Root);
            CalculateNullNodePositions();
            SelectedNullNodeId = null;
        }

        public void DeleteNode(Node nodeToDelete)
        {
            BinaryTree.DeleteNode(nodeToDelete);
            selectedNodeId = null;
            InputVisible = false;
            if (BinaryTree.Root == null)
            {
                Nodes.Clear();
                LinePositions.Clear();
                NullNodes.Clear();
            } else
            {
                UpdateNodesCollection(BinaryTree.Root);
                CalculateNodePositions();
                UpdateLinePositions(BinaryTree.Root);
            }
        }

        // Saves Binary Tree to file in level order format
        public void SaveTreeToFile()
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                
                sfd.Filter = "Text files (*.txt)|*.txt";
                sfd.ShowDialog();
                if (sfd.FileName != "")
                {
                    using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            BinaryTree.LevelOrder(NodesSaveList);
                            string text = "";
                            foreach (int? nodeId in NodesSaveList)
                            {
                                if (nodeId != null)
                                {
                                    text += nodeId.ToString() + " ";
                                } else
                                {
                                    text += "null ";
                                }
                            }
                            MessageBox.Show(text);
                            sw.Write(text);
                        }
                            
                    }
                   
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Calculate positions of the nodes on the canvas
        private void CalculateNodePositions()
        {
            double startX = CalculateCanvasWidth();
            foreach (var node in Nodes)
            {
                node.Position.X = startX + HorizontalNodeOffset;
                startX += HorizontalNodeOffset;
            }
            CalculateVerticalPositions(BinaryTree.Root);
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
                if (isLeftSubtree && node != BinaryTree.Root)
                {
                    leftSubtreeNodes++;
                }
                else if (node == BinaryTree.Root)
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
            int depth = BinaryTree.CalculateMaxDepth(BinaryTree.Root);
            CanvasHeight = (depth + 1) * CircleDiameter + (depth + 1) * VerticalNodeOffset;
        }

        private void UpdateLinePositions(Node node)
        {
            if (node == null)
                return;
            if (node == BinaryTree.Root)
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

            if (node == BinaryTree.Root)
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

            if (node == BinaryTree.Root)
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

        public void AddButtonClick()
        {
            CalculateNullNodePositions();
            InputVisible = true;
        }

        private void CalculateNullNodePositions()
        {
            NullNodes.Clear();
            int nodeId = 0;
            foreach (var node in Nodes)
            {
                if (node.LeftNode == null)
                {
                    Node nullNode = new Node();
                    nullNode.ID = ++nodeId; // used for click on node
                    nullNode.LeftNode = node; // used to know if node is parents left or right node
                    nullNode.Position = new Position(node.Position.X, node.Position.Y + CircleDiameter);
                    NullNodes.Add(nullNode);
                }
                
                if (node.RightNode == null)
                {
                    Node nullNode = new Node();
                    nullNode.ID = ++nodeId;
                    nullNode.ParentNode = node;
                    nullNode.RightNode = node; // used to know if node is parents left or right node
                    nullNode.Position = new Position(node.Position.X + CircleDiameter - AddCircleDiameter,
                        node.Position.Y + CircleDiameter);
                    NullNodes.Add(nullNode);
                }
            }
        }

        public void NodeClick(int nodeId)
        {
            if(selectedNodeId == nodeId)
            {
                selectedNodeId = null;    
            } else
            {
                SelectedNodeId = nodeId;
            }
            UpdateNodesCollection(BinaryTree.Root);
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

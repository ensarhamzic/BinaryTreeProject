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
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace BinaryTreeProject.ViewModels
{
    internal class BinaryTreeViewModel : INotifyPropertyChanged
    {
        private BinaryTree binaryTree; // instance of the binary tree
        private bool inputVisible; // is add input visible
        private int? selectedNodeId; // id of existing node selected
        private int? selectedNullNodeId; // id of null (non-existing) node selected
        private string newNodeValue; // value of user input
        private int circleDiameter; // diameter of the circle representing a node
        private double canvasWidth; // width of the canvas
        private double canvasHeight; // height of the canvas
        private double verticalNodeOffset; // vertical offset between two nodes
        private double horizontalNodeOffset; // horizontal offset between two nodes
        private double addCircleDiameter; // diameter of the circle representing a node when adding a new node
        private Stack<List<int?>> undoStack;
        private Stack<List<int?>> redoStack;

        // collection of nodes (used for binding, to draw nodes on the canvas)
        public ObservableCollection<Node> Nodes { get; set; }
        // collection of line positions (used for binding, to draw lines connecting nodes)
        public ObservableCollection<LinePosition> LinePositions { get; set; }
        // collection of non-existing nodes (used for binding, to draw places where new node can be added)
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
        public Stack<List<int?>> UndoStack
        {
            get { return undoStack; }
            set
            {
                undoStack = value;
            }
        } 

        public Stack<List<int?>> RedoStack
        {
            get { return redoStack; }
            set
            {
                redoStack = value;
            }
        }

        // commands
        public ICommand AddNewNodeCommand { get; private set; }
        public ICommand AddButtonClickCommand { get; private set; }
        public ICommand CancelAddCommand { get; private set; }
        public ICommand DeleteButtonClickCommand { get; private set; }
        public ICommand SaveTreeToFileCommand { get; private set; }
        public ICommand LoadTreeFromFileCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        public BinaryTreeViewModel()
        {
            BinaryTree = new BinaryTree();
            // Commands
            AddNewNodeCommand = new AddNodeCommand(this);
            AddButtonClickCommand = new AddButtonClickCommand(this);
            CancelAddCommand = new CancelAddCommand(this);
            DeleteButtonClickCommand = new DeleteButtonClickCommand(this);
            SaveTreeToFileCommand = new TreeToFileCommand(this);
            LoadTreeFromFileCommand = new TreeFromFileCommand(this);
            UndoCommand = new UndoCommand(this);
            RedoCommand = new RedoCommand(this);
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
            UndoStack = new Stack<List<int?>>();
            RedoStack = new Stack<List<int?>>();
        }

        public void AddNode(Node parentNode, int v, char side)
        {
            UpdateUndoStack();
            RedoStack = new Stack<List<int?>>();
            BinaryTree.AddNode(parentNode, v, side);
            UpdateUI();
            SelectedNullNodeId = null;
        }

        public void DeleteNode(Node nodeToDelete)
        {
            UpdateUndoStack();
            RedoStack = new Stack<List<int?>>();
            BinaryTree.DeleteNode(nodeToDelete);
            selectedNodeId = null;
            InputVisible = false;
            if (BinaryTree.Root == null)
            {
                Nodes.Clear();
                LinePositions.Clear();
                NullNodes.Clear();
            }
            else
            {
                UpdateUI();
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
                            /*List<int> inorder = new List<int>();
                            List<int> preorder = new List<int>();
                            BinaryTree.InOrder(BinaryTree.Root, inorder);
                            BinaryTree.PreOrder(BinaryTree.Root, preorder);

                            string text = "";
                            foreach (int node in inorder)
                            {
                                text += node.ToString() + " ";
                            }
                            sw.WriteLine(text);

                            text = "";
                            foreach (int node in preorder)
                            {
                                text += node.ToString() + " ";
                            }
                            sw.WriteLine(text); */
                            List<int?> preorder = new List<int?>();
                            BinaryTree.Preorder(BinaryTree.Root, preorder);
                            string text = "";
                            foreach (int? node in preorder)
                            {
                                if (node == null)
                                {
                                    text += "null ";
                                }
                                else
                                {
                                    text += node.ToString() + " ";
                                }
                            }
                            sw.WriteLine(text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LoadTreeFromFile()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.Filter = "Text files (*.txt)|*.txt";
                ofd.ShowDialog();
                if (ofd.FileName != "")
                {
                    using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            /*string text = sr.ReadLine();
                            string[] inorderString = text.Split(' ');
                            text = sr.ReadLine();
                            string[] preorderString = text.Split(' ');

                            List<int> inorder = new List<int>();
                            List<int> preorder = new List<int>();
                            foreach (string s in inorderString)
                            {
                                if (s != "")
                                    inorder.Add(int.Parse(s));
                            }
                            foreach (string s in preorderString)
                            {
                                if (s != "")
                                    preorder.Add(int.Parse(s));
                            }
                            int len = inorder.Count;

                            BinaryTree = new BinaryTree();
                            BinaryTree.nodeId = 0; // Reset Node Id
                            BinaryTree.preIndex = 0; // Reset preIndex
                            BinaryTree.Root = BinaryTree.BuildTree(inorder, preorder, 0, len - 1);
                            UpdateUI();
                            selectedNodeId = null;
                            selectedNullNodeId = null;
                            InputVisible = false;
                            */

                            string text = sr.ReadLine();
                            string[] preorderString = text.Split(' ');
                            List<int?> preorder = new List<int?>();
                            foreach (var s in preorderString)
                            {
                                if (s == "null" || s == "")
                                {
                                    preorder.Add(null);
                                }
                                else
                                {
                                    preorder.Add(int.Parse(s));
                                }
                            }
                            BinaryTree = new BinaryTree();
                            BinaryTree.Root = BinaryTree.BuildTreeFromPreorder(preorder, true);
                            UpdateUI();
                            UndoStack = new Stack<List<int?>>();
                            RedoStack = new Stack<List<int?>>();
                            selectedNodeId = null;
                            selectedNullNodeId = null;
                            InputVisible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Undo()
        {
            UpdateRedoStack();
            List<int?> previousTree = UndoStack.Pop();
            BinaryTree = new BinaryTree();
            BinaryTree.Root = BinaryTree.BuildTreeFromPreorder(previousTree, true);
            selectedNodeId = null;
            selectedNullNodeId = null;
            UpdateUI();
        }

        public void Redo()
        {
            UpdateUndoStack();
            List<int?> previousTree = RedoStack.Pop();
            BinaryTree = new BinaryTree();
            BinaryTree.Root = BinaryTree.BuildTreeFromPreorder(previousTree, true);
            selectedNodeId = null;
            selectedNullNodeId = null;
            UpdateUI();
        }

        /*private int textId;
        private int? NextNode()
        {
            if (preorderString[textId] == "")
            {
                return 999;
            }
            else if (preorderString[textId] == "null")
            {
                textId++;
                return null;
            }
            else
            {
                int node = int.Parse(preorderString[textId]);
                textId++;
                return node;
            }
        } */



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
            if (node == BinaryTree.Root)
            {
                Nodes.Clear();
            }
            if (node == null)
            {
                return;
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
            if (selectedNodeId == nodeId)
            {
                selectedNodeId = null;
            }
            else
            {
                SelectedNodeId = nodeId;
            }
            UpdateUI();
        }

        public void NullNodeClick(int nullNodeId)
        {
            SelectedNullNodeId = nullNodeId;
        }

        private void UpdateUI()
        {
            UpdateNodesCollection(BinaryTree.Root);
            CalculateNodePositions();
            UpdateLinePositions(BinaryTree.Root);
            CalculateNullNodePositions();
        }

        private void UpdateUndoStack()
        {
            List<int?> savedTree = new List<int?>();
            BinaryTree.Preorder(BinaryTree.Root, savedTree);
            UndoStack.Push(savedTree);
        }

        private void UpdateRedoStack()
        {
            List<int?> savedTree = new List<int?>();
            BinaryTree.Preorder(BinaryTree.Root, savedTree);
            RedoStack.Push(savedTree);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}

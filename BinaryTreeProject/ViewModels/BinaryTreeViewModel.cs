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
using BinaryTreeProject.Windows;
using System.Data;

namespace BinaryTreeProject.ViewModels
{
    internal class BinaryTreeViewModel : BaseViewModel
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
        private double nodeValueSize; // font size of the node value
        private double addCircleDiameter; // diameter of the circle representing a node when adding a new node
        private Stack<List<int?>> undoStack; // stack of undo operations
        private Stack<List<int?>> redoStack; // stack of redo operations
        private Database database; // database of the binary tree
        private int numberOfNodes;
        private int treeDepth;
        private string maxNode;
        private string minNode;
        private int zoomLevel;
        
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

        public double NodeValueSize
        {
            get { return nodeValueSize; }
            set
            {
                nodeValueSize = value;
                OnPropertyChanged("NodeValueSize");
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
        public Database Database
        {
            get
            {
                return database;
            }
            set
            {
                database = value;
            }
        }

        public int NumberOfNodes
        {
            get
            {
                return numberOfNodes;
            }
            set
            {
                numberOfNodes = value;
                OnPropertyChanged("NumberOfNodes");
            }
        }

        public int TreeDepth
        {
            get
            {
                return treeDepth;
            }
            set
            {
                treeDepth = value;
                OnPropertyChanged("TreeDepth");
            }
        }

        public string MaxNode
        {
            get
            {
                return maxNode;
            }
            set
            {
                maxNode = value;
                OnPropertyChanged("MaxNode");
            }
        }

        public string MinNode
        {
            get
            {
                return minNode;
            }
            set
            {
                minNode = value;
                OnPropertyChanged("MinNode");
            }
        }

        public int ZoomLevel
        {
            get
            {
                return zoomLevel;
            }
            set
            {
                zoomLevel = value;
                OnPropertyChanged("ZoomLevel");
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
        public ICommand SaveTreeToDBCommand { get; private set; }
        public ICommand LoadTreeFromDBCommand { get; private set; }

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
            SaveTreeToDBCommand = new TreeToDBCommand(this);
            LoadTreeFromDBCommand = new TreeFromDBCommand(this);
            UndoCommand = new UndoCommand(this);
            RedoCommand = new RedoCommand(this);
            // Other stuff
            NullNodes = new ObservableCollection<Node>();
            SelectedNodeId = null;
            SelectedNullNodeId = null;
            CanvasWidth = 0;
            CanvasHeight = 0;
            SetNodeSizes(50);
            Nodes = new ObservableCollection<Node>();
            LinePositions = new ObservableCollection<LinePosition>();
            UndoStack = new Stack<List<int?>>();
            RedoStack = new Stack<List<int?>>();
            Database = new Database();
            NumberOfNodes = 0;
            TreeDepth = 0;
            MaxNode = "/";
            MinNode = "/";
            ZoomLevel = 1;
    }

        // Adds new node and updates everything that need to be updated
        public void AddNode(Node parentNode, int v, char side)
        {
            UpdateStack(UndoStack);
            RedoStack = new Stack<List<int?>>();
            BinaryTree.AddNode(parentNode, v, side);
            UpdateUI();
            SelectedNullNodeId = null;
        }

        // Deletes node and updates everything that need to be updated
        public void DeleteNode(Node nodeToDelete)
        {
            UpdateStack(UndoStack);
            RedoStack = new Stack<List<int?>>();
            BinaryTree.DeleteNode(nodeToDelete);
            selectedNodeId = null;
            InputVisible = false;
            if (BinaryTree.Root == null)
            {
                Nodes.Clear();
                LinePositions.Clear();
                NullNodes.Clear();
                NumberOfNodes = 0;
                TreeDepth = 0;
                MaxNode = "/";
                MinNode = "/";
            }
            else
            {
                UpdateUI();
            }
        }

        // Saves Binary Tree to file in preorder format
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

        // Saves Binary Tree to database
        public void SaveTreeToDB()
        {
            List<Node> preorder = new List<Node>();
            BinaryTree.Preorder(BinaryTree.Root, preorder);
            var dbdialog = new DBDialog();
            if (dbdialog.ShowDialog() == true)
            {
                Database.SaveTree(dbdialog.TreeName, preorder);
            }
        }

        // Loads Binary Tree from file in preorder format
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

        // Loads Binary Tree from database
        public void LoadTreeFromDB()
        {
            var loadDbDialog = new LoadDBDialog();
            List<Node> foundNodes = new List<Node>();
            DataTable dt;
            if (loadDbDialog.ShowDialog() == true)
            {
                dt = loadDbDialog.NodesTable;
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    Node temp = new Node
                    {
                        ID = Convert.ToInt32(dt.Rows[i]["id"]),
                        Value = Convert.ToInt32(dt.Rows[i]["value"]),
                    };

                    foundNodes.Add(temp);
                }

                BinaryTree.BuildTreeFromDatabase(foundNodes, dt);
                UpdateUI();
                UndoStack = new Stack<List<int?>>();
                RedoStack = new Stack<List<int?>>();
                selectedNodeId = null;
                selectedNullNodeId = null;
                InputVisible = false;
            }
        }

        // Returns to previous state of binary tree
        public void Undo()
        {
            UpdateStack(RedoStack);
            List<int?> previousTree = UndoStack.Pop();
            BinaryTree = new BinaryTree();
            BinaryTree.Root = BinaryTree.BuildTreeFromPreorder(previousTree, true);
            selectedNodeId = null;
            selectedNullNodeId = null;
            UpdateUI();
        }

        // Returns to next state of binary tree
        public void Redo()
        {
            UpdateStack(UndoStack);
            List<int?> previousTree = RedoStack.Pop();
            BinaryTree = new BinaryTree();
            BinaryTree.Root = BinaryTree.BuildTreeFromPreorder(previousTree, true);
            selectedNodeId = null;
            selectedNullNodeId = null;
            UpdateUI();
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

        // Calculates positions of nodes where new nodes can be added
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
        
        public void ZoomChanged()
        {
            switch(ZoomLevel)
            {
                case 1:
                    SetNodeSizes(50);
                    break;
                case 2:
                    SetNodeSizes(65);
                    break;
                case 3:
                    SetNodeSizes(75);
                    break;
                case 4:
                    SetNodeSizes(85);
                    break;
                case 5:
                    SetNodeSizes(100);
                    break;
                default:
                    SetNodeSizes(50);
                    break;
            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            UpdateNodesCollection(BinaryTree.Root);
            CalculateNodePositions();
            UpdateLinePositions(BinaryTree.Root);
            CalculateNullNodePositions();
            NumberOfNodes = Nodes.Count;
            TreeDepth = BinaryTree.CalculateMaxDepth(BinaryTree.Root);
            MaxNode = Nodes.Max(node => node.Value).ToString();
            MinNode = Nodes.Min(node => node.Value).ToString();
        }

        private void SetNodeSizes(int circleDiameter)
        {
            CircleDiameter = circleDiameter;
            VerticalNodeOffset = CircleDiameter * 0.5;
            HorizontalNodeOffset = CircleDiameter * 0.7;
            AddCircleDiameter = CircleDiameter * 0.3;
            NodeValueSize = CircleDiameter * 0.4;
        }

        // Pushes current state of binary tree to passed stack
        private void UpdateStack(Stack<List<int?>> s)
        {
            List<int?> savedTree = new List<int?>();
            BinaryTree.Preorder(BinaryTree.Root, savedTree);
            s.Push(savedTree);
        }
    }
}

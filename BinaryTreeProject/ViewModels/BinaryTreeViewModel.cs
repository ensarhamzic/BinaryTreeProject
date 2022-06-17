using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using BinaryTreeProject.ViewModels.Commands;
using BinaryTreeProject.Models;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;
using BinaryTreeProject.Windows;
using System.Data;
using System.Timers;

namespace BinaryTreeProject.ViewModels
{
    public class BinaryTreeViewModel : BaseViewModel
    {
        private BinaryTree binaryTree; // instance of the binary tree
        private bool inputVisible; // is add input visible
        private bool popupVisible; // is new node popup visible
        private bool messagePopupVisible; // is message popup visible
        private string popupMessage; // message to display in popup
        private bool isError; // is message popup an error or success
        private int? selectedNodeId; // id of existing node selected
        private int? selectedNullNodeId; // id of null (non-existing) node selected
        private int? selectedChangeNodeId; // id of node to change its value
        private string newNodeValue; // value of user input
        private double nodeValueSize; // font size of the node value
        private double addCircleDiameter; // diameter of the circle representing a node when adding a new node
        private Stack<List<int?>> undoStack; // stack of undo operations
        private Stack<List<int?>> redoStack; // stack of redo operations
        private Database database; // database of the binary tree
        private int numberOfNodes; // number of nodes in the binary tree
        private int treeDepth; // depth of the binary tree
        private string maxNode; // maximum node value in the binary tree
        private string minNode; // minimum node value in the binary tree
        private int zoomLevel; // zoom level of the canvas
        private Timer timer; // timer for the popup
        private string loadedTreeName; // name of last loaded tree
        public static BinaryTreeViewModel SavedBTVM; // saved binary tree view model

        // collection of nodes (used for binding, to draw nodes on the canvas)
        public ObservableCollection<Node> Nodes { get; set; }
        // collection of line positions (used for binding, to draw lines connecting nodes)
        public ObservableCollection<LinePosition> LinePositions { get; set; }
        // collection of non-existing nodes (used for binding, to draw places where new node can be added)
        public ObservableCollection<Node> NullNodes { get; set; }
        public BinaryTree BinaryTree
        {
            get { return binaryTree; }
            set { binaryTree = value; OnPropertyChanged("BinaryTree"); }
        }
        public double NodeValueSize
        {
            get { return nodeValueSize; }
            set { nodeValueSize = value; OnPropertyChanged("NodeValueSize"); }
        }
        public double AddCircleDiameter
        {
            get { return addCircleDiameter; }
            set { addCircleDiameter = value; OnPropertyChanged("AddCircleDiameter"); }
        }
        public int? SelectedNodeId
        {
            get { return selectedNodeId; }
            set { selectedNodeId = value; OnPropertyChanged("SelectedNodeId"); }
        }
        public int? SelectedNullNodeId
        {
            get { return selectedNullNodeId; }
            set { selectedNullNodeId = value; OnPropertyChanged("SelectedNullNodeId"); }
        }
        public int? SelectedChangeNodeId
        {
            get { return selectedChangeNodeId; }
            set { selectedChangeNodeId = value; }
        }
        public bool InputVisible
        {
            get { return inputVisible; }
            set { inputVisible = value; OnPropertyChanged("InputVisible"); }
        }
        public bool PopupVisible
        {
            get { return popupVisible; }
            set { popupVisible = value; OnPropertyChanged("PopupVisible"); }
        }
        public bool MessagePopupVisible
        {
            get { return messagePopupVisible; }
            set { messagePopupVisible = value; OnPropertyChanged("MessagePopupVisible"); }
        }
        public string PopupMessage
        {
            get { return popupMessage; }
            set { popupMessage = value; OnPropertyChanged("PopupMessage"); }
        }
        public bool IsError
        {
            get { return isError; }
            set { isError = value; OnPropertyChanged("IsError"); }
        }
        public string NewNodeValue
        {
            get { return newNodeValue; }
            set { newNodeValue = value; OnPropertyChanged("NewNodeValue"); }
        }
        public Stack<List<int?>> UndoStack
        {
            get { return undoStack; }
            set { undoStack = value; }
        }
        public Stack<List<int?>> RedoStack
        {
            get { return redoStack; }
            set { redoStack = value; }
        }
        public Database Database
        {
            get { return database; }
            set { database = value; }
        }
        public int NumberOfNodes
        {
            get { return numberOfNodes; }
            set { numberOfNodes = value; OnPropertyChanged("NumberOfNodes"); }
        }
        public int TreeDepth
        {
            get { return treeDepth; }
            set { treeDepth = value; OnPropertyChanged("TreeDepth"); }
        }
        public string MaxNode
        {
            get { return maxNode; }
            set { maxNode = value; OnPropertyChanged("MaxNode"); }
        }
        public string MinNode
        {
            get { return minNode; }
            set { minNode = value; OnPropertyChanged("MinNode"); }
        }
        public int ZoomLevel
        {
            get { return zoomLevel; }
            set { zoomLevel = value; }
        }

        public string LoadedTreeName
        {
            get { return loadedTreeName; }
            set { loadedTreeName = value; OnPropertyChanged("LoadedTreeName"); }
        }
        // commands
        public ICommand AddOrUpdateNodeCommand { get; private set; }
        public ICommand AddButtonClickCommand { get; private set; }
        public ICommand CancelAddCommand { get; private set; }
        public ICommand DeleteButtonClickCommand { get; private set; }
        public ICommand SaveTreeToFileCommand { get; private set; }
        public ICommand LoadTreeFromFileCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }
        public ICommand SaveTreeToDBCommand { get; private set; }
        public ICommand LoadTreeFromDBCommand { get; private set; }
        public ICommand ClosePopupCommand { get; private set; }

        public BinaryTreeViewModel()
        {
            BinaryTree = new BinaryTree();
            // Commands
            AddOrUpdateNodeCommand = new AddOrUpdateNodeCommand(this);
            AddButtonClickCommand = new AddButtonClickCommand(this);
            CancelAddCommand = new CancelAddCommand(this);
            DeleteButtonClickCommand = new DeleteButtonClickCommand(this);
            SaveTreeToFileCommand = new TreeToFileCommand(this);
            LoadTreeFromFileCommand = new TreeFromFileCommand(this);
            SaveTreeToDBCommand = new TreeToDBCommand(this);
            LoadTreeFromDBCommand = new TreeFromDBCommand(this);
            UndoCommand = new UndoCommand(this);
            RedoCommand = new RedoCommand(this);
            ClosePopupCommand = new ClosePopupCommand(this);
            // Other stuff
            NullNodes = new ObservableCollection<Node>();
            SelectedNodeId = null;
            SelectedNullNodeId = null;
            SelectedChangeNodeId = null;
            InputVisible = false;
            PopupVisible = false;
            MessagePopupVisible = false;
            PopupMessage = "";
            LoadedTreeName = "";
            IsError = false;
            CanvasWidth = 300;
            CanvasHeight = 300;
            SetNodeSizes(60);
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
            timer = new Timer();
            LoadSavedData();
        }

        // Adds or updates new node and updates UI
        public void AddOrUpdateNode(Node parentNode, int v, char side, bool isUpdate = false)
        {
            UpdateStack(UndoStack);
            RedoStack = new Stack<List<int?>>();
            if (isUpdate)
                Nodes.First(x => x.ID == SelectedChangeNodeId).Value = v;
            else
                BinaryTree.AddNode(parentNode, v, side);
            UpdateUI();
            PopupVisible = false;
            SelectedNullNodeId = null;
            SelectedChangeNodeId = null;
            NewNodeValue = "";
        }

        // Deletes node and updates UI
        public void DeleteNode(Node nodeToDelete)
        {
            UpdateStack(UndoStack);
            RedoStack = new Stack<List<int?>>();
            BinaryTree.DeleteNode(nodeToDelete);
            selectedNodeId = null;
            selectedNullNodeId = null;
            InputVisible = false;
            PopupVisible = false;
            UpdateUI();
        }

        // Saves Binary Tree to file in preorder format
        public void SaveTreeToFile()
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Text files (*.txt)|*.txt";
                if(!string.IsNullOrEmpty(LoadedTreeName))
                    sfd.FileName = $"{LoadedTreeName}.txt";
                if (sfd.ShowDialog() == true)
                {
                    if (sfd.FileName != "")
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
                                        text += "null ";
                                    else
                                        text += node.ToString() + " ";
                                }
                                sw.WriteLine(text);
                            }
                        }
                    ShowPopup(false, "Successfully saved the tree to file");
                }
            }
            catch
            {
                ShowPopup(true, "Error saving the tree to file");
            }
        }

        // Saves Binary Tree to database
        public void SaveTreeToDB()
        {
            while (true)
            {
                var dbDialog = new DBDialog(LoadedTreeName);
                bool? dialogResult = dbDialog.ShowDialog();
                if (dialogResult != true) return;
                if (dbDialog.TreeName.Length <= 20 && dbDialog.TreeName.Length > 0)
                {
                    List<Node> preorder = new List<Node>();
                    BinaryTree.Preorder(BinaryTree.Root, preorder);
                    bool isSaved = Database.SaveTree(dbDialog.TreeName, preorder);
                    if (isSaved)
                        ShowPopup(false, "Successfully saved the tree to database");
                    else
                        ShowPopup(true, "Error saving the tree to database");
                    return;
                }
                else
                    ShowPopup(true, "Tree name must be between 1 and 20 characters long");
            }
        }

        // Loads Binary Tree from file in preorder format
        public void LoadTreeFromFile()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Text files (*.txt)|*.txt";
                if (ofd.ShowDialog() == true)
                {
                    if (ofd.FileName != "")
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
                                        preorder.Add(null);
                                    else
                                        preorder.Add(int.Parse(s));
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
                    LoadedTreeName = ofd.SafeFileName.Substring(0, ofd.SafeFileName.Length - 4);
                    ShowPopup(false, "Successfully loaded tree from file");
                }

            }
            catch
            {
                ShowPopup(true, "Error loading tree from file");
            }
        }

        // Loads Binary Tree from database
        public void LoadTreeFromDB()
        {
            while (true)
            {
                var loadDbDialog = new LoadDBDialog();
                bool? dialogResult = loadDbDialog.ShowDialog();
                if (dialogResult != true) return;
                if (loadDbDialog.TreeName.Length <= 20 && loadDbDialog.TreeName.Length > 0)
                {
                    List<Node> foundNodes = new List<Node>();
                    DataTable dt;
                    dt = Database.LoadTree(loadDbDialog.TreeName);
                    if (dt == null)
                        ShowPopup(true, "Tree does not exist");
                    else
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
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
                        LoadedTreeName = loadDbDialog.TreeName;
                        ShowPopup(false, "Successfully loaded tree from database");
                        return;
                    }
                }
                else
                    ShowPopup(true, "Tree name length must be between 1 and 20 characters");
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
                    leftSubtreeNodes++;
                else if (node == BinaryTree.Root)
                    isLeftSubtree = false;
                else
                    rightSubtreeNodes++;
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
                    CanvasWidth = HorizontalNodeOffset * 4;
                else
                    CanvasWidth = ((rightSubtreeNodes + 2) * HorizontalNodeOffset) * 2;
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
                LinePositions.Clear();
            if (node.ParentNode != null)
            {
                var line = new LinePosition();
                double startX, startY;
                //double startX = node.ParentNode.Position.X + CircleDiameter / 2;
                //double startY = node.ParentNode.Position.Y + CircleDiameter;
                /*if(node.Position.X < node.ParentNode.Position.X)
                {
                    startX = (CircleDiameter / 2) * Math.Sin(225) + node.ParentNode.Position.X + CircleDiameter * 0.75;
                    startY = (CircleDiameter / 2) * Math.Cos(225) + node.ParentNode.Position.Y + CircleDiameter * 0.75;
                } else
                {
                    startX = (CircleDiameter / 2) * Math.Sin(315) + node.ParentNode.Position.X + CircleDiameter * 0.25;
                    startY = (CircleDiameter / 2) * Math.Cos(315) + node.ParentNode.Position.Y + CircleDiameter * 0.625;
                }    */

                if (node.Position.X < node.ParentNode.Position.X)
                {
                    startX = (CircleDiameter / 2) * Math.Abs(Math.Sin(135)) + node.ParentNode.Position.X + CircleDiameter * 0.25;
                    startY = (CircleDiameter / 2) * Math.Abs(Math.Cos(135)) + node.ParentNode.Position.Y + CircleDiameter * 0.45;
                }
                else
                {
                    startX = (CircleDiameter / 2) * Math.Abs(Math.Sin(45)) + node.ParentNode.Position.X + CircleDiameter * 0.25;
                    startY = (CircleDiameter / 2) * Math.Abs(Math.Cos(45)) + node.ParentNode.Position.Y + CircleDiameter * 0.7;
                }

                line.StartPosition = new Position(startX, startY);
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
                Nodes.Clear();
            if (node == null)
                return;
            UpdateNodesCollection(node.LeftNode);
            Nodes.Add(node);
            UpdateNodesCollection(node.RightNode);
        }

        // Calculating Y positions of the nodes
        private void CalculateVerticalPositions(Node node)
        {
            if (node == null) return;
            if (node == BinaryTree.Root)
                node.Position.Y = VerticalNodeOffset;
            if (node.LeftNode != null)
                node.LeftNode.Position.Y = node.Position.Y + (CircleDiameter + VerticalNodeOffset);
            if (node.RightNode != null)
                node.RightNode.Position.Y = node.Position.Y + (CircleDiameter + VerticalNodeOffset);
            CalculateVerticalPositions(node.LeftNode);
            CalculateVerticalPositions(node.RightNode);
        }

        public void AddButtonClick()
        {
            if (InputVisible)
            {
                InputVisible = false;
                PopupVisible = false;
                selectedNullNodeId = null;
                SelectedChangeNodeId = null;
                return;
            }
            CalculateNullNodePositions();
            InputVisible = true;
        }

        // Calculates positions of nodes where new nodes can be added
        private void CalculateNullNodePositions()
        {
            NullNodes.Clear();
            int nodeId = 0;
            if (Nodes.Count > 0)
            {
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
                        nullNode.RightNode = node; // used to know if node is parents left or right node
                        nullNode.Position = new Position(node.Position.X + CircleDiameter - AddCircleDiameter,
                            node.Position.Y + CircleDiameter);
                        NullNodes.Add(nullNode);
                    }
                }
            }
            else
            {
                Node nullNode = new Node();
                nullNode.ID = ++nodeId;
                nullNode.Position = new Position(CanvasWidth / 2 - CircleDiameter / 2, VerticalNodeOffset + CircleDiameter / 2);
                NullNodes.Add(nullNode);
            }

        }

        public void NodeClick(int nodeId)
        {
            if (selectedNodeId == nodeId)
                selectedNodeId = null;
            else
                SelectedNodeId = nodeId;
            UpdateUI();
        }

        public void NullNodeClick(int nullNodeId)
        {
            if (SelectedNullNodeId == nullNodeId) return;
            if (PopupVisible)
                PopupVisible = false;
            PopupVisible = true;
            SelectedNullNodeId = nullNodeId;
            SelectedChangeNodeId = null;
        }

        public void ZoomChanged()
        {
            switch (ZoomLevel)
            {
                case 1:
                    SetNodeSizes(60);
                    break;
                case 2:
                    SetNodeSizes(75);
                    break;
                case 3:
                    SetNodeSizes(85);
                    break;
                case 4:
                    SetNodeSizes(100);
                    break;
                case 5:
                    SetNodeSizes(120);
                    break;
                default:
                    SetNodeSizes(60);
                    break;
            }
            UpdateUI();
        }

        // Updates UI so it representes the current state of the tree
        public void UpdateUI()
        {
            SelectedChangeNodeId = null;
            UpdateNodesCollection(BinaryTree.Root);
            CalculateNodePositions();
            UpdateLinePositions(BinaryTree.Root);
            if (Nodes.Count == 0)
                LinePositions.Clear();
            CalculateNullNodePositions();
            NumberOfNodes = Nodes.Count;
            TreeDepth = BinaryTree.CalculateMaxDepth(BinaryTree.Root);
            MaxNode = Nodes.Max(node => node.Value).ToString();
            MinNode = Nodes.Min(node => node.Value).ToString();
            if (string.IsNullOrEmpty(MaxNode))
                MaxNode = "/";
            if (string.IsNullOrEmpty(MinNode))
                MinNode = "/";
        }

        // Sets sizes of nodes depending on the node diameter
        private void SetNodeSizes(int circleDiameter)
        {
            CircleDiameter = circleDiameter;
            VerticalNodeOffset = CircleDiameter * 0.5;
            HorizontalNodeOffset = CircleDiameter * 0.7;
            AddCircleDiameter = CircleDiameter * 0.3;
            NodeValueSize = CircleDiameter * 0.4;
        }

        // When timer elapses, hide popup
        private void TimerElapsed(Object source, ElapsedEventArgs e)
        {
            ClosePopup();
        }

        // Show popup on the screen and start its duration timer
        private void ShowPopup(bool isError, string msg)
        {
            IsError = isError;
            PopupMessage = msg;
            MessagePopupVisible = true;
            timer.Interval = 5000;
            timer.Elapsed += new ElapsedEventHandler(TimerElapsed);
            timer.AutoReset = false;
            timer.Start();
        }
        public void ClosePopup()
        {
            MessagePopupVisible = false;
            PopupMessage = "";
            timer.Stop();
        }

        // Pushes current state of binary tree to passed stack
        private void UpdateStack(Stack<List<int?>> s)
        {
            List<int?> savedTree = new List<int?>();
            BinaryTree.Preorder(BinaryTree.Root, savedTree);
            s.Push(savedTree);
        }

        // Loads previous state of binary tree when loading view
        private void LoadSavedData()
        {
            if (SavedBTVM == null) return;
            BinaryTree = SavedBTVM.BinaryTree;
            ZoomLevel = SavedBTVM.ZoomLevel;
            ZoomChanged();
            SelectedNodeId = SavedBTVM.SelectedNodeId;
            UndoStack = SavedBTVM.UndoStack;
            RedoStack = SavedBTVM.RedoStack;
            LoadedTreeName = SavedBTVM.LoadedTreeName;
            UpdateUI();
        }
    }
}
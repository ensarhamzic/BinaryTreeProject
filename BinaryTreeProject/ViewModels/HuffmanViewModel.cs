using BinaryTreeProject.Models;
using BinaryTreeProject.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BinaryTreeProject.ViewModels
{
    public class HuffmanViewModel : BaseViewModel
    {
        private Huffman huffman; // instance of huffman algorithm
        private string enteredText; // text entered by user
        private bool isStarted; // is huffman algorithm started
        private bool isRunning; // is huffman algorithm step animation running
        private List<char> characters; // list of unique characters in algorithm
        private Stack<List<HuffmanTree>> previousStates; // previous states of the algorithm
        private List<int> currentNodeIds; // Ids of nodes that are currently being changed in algorithm (used to color them differently)
        public static HuffmanViewModel SavedHVM; // saved huffman view model
        
        private bool tableVisible; // is algorithm end table visible
        private Position tablePosition; // position of the algorithm end table


        public Huffman Huffman
        {
            get { return huffman; }
            set { huffman = value; OnPropertyChanged("Huffman"); }
        }
        public bool IsStarted
        {
            get { return isStarted; }
            set { isStarted = value; OnPropertyChanged("IsStarted"); }
        }

        public bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; OnPropertyChanged("IsRunning"); }
        }
        public Stack<List<HuffmanTree>> PreviousStates
        {
            get { return previousStates; }
            set { previousStates = value; OnPropertyChanged("PreviousStates"); }
        }
        public List<char> Characters
        {
            get { return characters; }
            set { characters = value; OnPropertyChanged("Characters"); }
        }
        public string EnteredText
        {
            get { return enteredText; }
            set { enteredText = value; OnPropertyChanged("EnteredText"); }
        }
        public List<int> CurrentNodeIds
        {
            get { return currentNodeIds; }
            set { currentNodeIds = value; OnPropertyChanged("CurrentNodeIds"); }
        }

        public bool TableVisible
        {
            get { return tableVisible; }
            set { tableVisible = value; OnPropertyChanged("TableVisible"); }
        }
        public Position TablePosition
        {
            get { return tablePosition; }
            set { tablePosition = value; OnPropertyChanged("TablePosition"); }
        }
        public ObservableCollection<HuffmanNode> Nodes { get; set; }
        public ObservableCollection<LinePosition> LinePositions { get; set; }
        public ObservableCollection<LineCode> LineCodes { get; set; }
        public ObservableCollection<CharacterCode> CharacterCodes { get; set; }

        public ICommand StartCommand { get; private set; }
        public ICommand NextStepCommand { get; private set; }
        public ICommand PreviousStepCommand { get; private set; }
        public ICommand SkipToEndCommand { get; private set; }
        public ICommand BackToStartCommand { get; private set; }
        public HuffmanViewModel()
        {
            Huffman = new Huffman();
            Nodes = new ObservableCollection<HuffmanNode>();
            LinePositions = new ObservableCollection<LinePosition>();
            LineCodes = new ObservableCollection<LineCode>();
            CharacterCodes = new ObservableCollection<CharacterCode>();
            CircleDiameter = 75;
            CanvasWidth = 0;
            CanvasHeight = 0;
            VerticalNodeOffset = CircleDiameter * 0.5;
            HorizontalNodeOffset = CircleDiameter * 1.3;
            IsStarted = false;
            IsRunning = false;
            TableVisible = false;
            TablePosition = new Position();
            Characters = new List<char>();
            CurrentNodeIds = new List<int>();
            PreviousStates = new Stack<List<HuffmanTree>>();
            // commands
            StartCommand = new StartHuffmanCommand(this);
            NextStepCommand = new NextStepHuffmanCommand(this);
            PreviousStepCommand = new PreviousStepHuffmanCommand(this);
            SkipToEndCommand = new SkipToEndHuffmanCommand(this);
            BackToStartCommand = new BackToStartHuffmanCommand(this);
            LoadSavedData();
        }

        // Starts Huffman algorithm
        public void StartHuffman()
        {
            if (IsRunning) return;
            Characters.Clear();
            IsStarted = true;
            PreviousStates.Clear();
            Huffman.Trees.Clear();
            UpdateUI();
            // Creates trees for every node, while calculating its frequency
            foreach (char c in enteredText)
                if (!Characters.Contains(c))
                {
                    int frequency = 0;
                    foreach (char ch in enteredText)
                        if (ch == c)
                            frequency++;
                    Huffman.Trees.Add(new HuffmanTree(new HuffmanNode(c, frequency)));
                    Characters.Add(c);
                }
            UpdateUI();
        }

        // Calculates trees states of next step of algorithm and updates UI
        public async void NextStep(bool isSkipped = true)
        {
            if (IsRunning) return;
            IsRunning = true;
            SaveCurrentState();
            CurrentNodeIds.Clear();
            HuffmanTree first = Huffman.Trees[0];
            HuffmanTree second = Huffman.Trees[1];
            
            if(!isSkipped)
            {
                CurrentNodeIds.Add((int)first.Root.ID);
                CurrentNodeIds.Add((int)second.Root.ID);
                UpdateUI();
                await Task.Delay(1000).ContinueWith(_ =>
                {
                    var mergedTree = CalculateNextState(first, second);
                    CurrentNodeIds.Clear();
                    CurrentNodeIds.Add((int)mergedTree.Root.ID);
                });
                UpdateUI();
                await Task.Delay(1000).ContinueWith(_ =>
                {
                    CurrentNodeIds.Clear();
                });
            } else
                CalculateNextState(first, second);
            
            IsRunning = false;
            UpdateUI();
        }

        private HuffmanTree CalculateNextState(HuffmanTree first, HuffmanTree second)
        {
            HuffmanTree mergedTree = new HuffmanTree(new HuffmanNode(null,
                (int)first.Root.Value + (int)second.Root.Value));
            // makes new merged tree root as parent of first and second root (merging the trees)
            mergedTree.Root.LeftNode = first.Root;
            mergedTree.Root.RightNode = second.Root;
            first.Root.ParentNode = mergedTree.Root;
            second.Root.ParentNode = mergedTree.Root;
            Huffman.Trees.RemoveRange(0, 2);
            Huffman.Trees.Add(mergedTree);
            return mergedTree;
        }
        

        // Saves current state of the algorithm to be able to go back to it
        private void SaveCurrentState()
        {
            List<HuffmanTree> currentState = new List<HuffmanTree>();
            foreach (HuffmanTree tree in Huffman.Trees)
            {
                HuffmanTree newTree = new HuffmanTree();
                newTree.Root = SaveTree(tree.Root);
                currentState.Add(newTree);
            }
            PreviousStates.Push(currentState);
        }

        // Helper function to SaveCurrentState
        private HuffmanNode SaveTree(HuffmanNode node)
        {
            if (node == null)
                return null;
            HuffmanNode newNode = new HuffmanNode(node.Character, (int)node.Value, node.ID);
            newNode.LeftNode = SaveTree(node.LeftNode);
            if (newNode.LeftNode != null)
                newNode.LeftNode.ParentNode = newNode;
            newNode.RightNode = SaveTree(node.RightNode);
            if (newNode.RightNode != null)
                newNode.RightNode.ParentNode = newNode;
            return newNode;
        }

        // Goes back one step in the algorithm
        public void PreviousStep()
        {
            if (IsRunning) return;
            Huffman.Trees = PreviousStates.Pop();
            UpdateUI();
        }

        // Ends the algorithm
        public void SkipToEnd()
        {
            if (IsRunning) return;
            while (Huffman.Trees.Count > 1)
                NextStep();
        }

        // Goes back to the start of the algorithm
        public void BackToStart()
        {
            if (IsRunning) return;
            while (PreviousStates.Count > 0)
                PreviousStep();
        }

        // Calculates node positons on the canvas
        private void CalculateNodePositions()
        {
            double startX = 0; // must change
            foreach (var node in Nodes)
            {
                node.Position.X = startX + HorizontalNodeOffset;
                startX += HorizontalNodeOffset;
            }
            CalculateVerticalPositions();
        }

        // Helper function for CalculateNodePositions
        private void CalculateVerticalPositions()
        {
            foreach (var tree in Huffman.Trees)
                VerticalPositionsTree(tree.Root);
        }

        // Helper function for CalculateNodePositions
        private void VerticalPositionsTree(HuffmanNode node)
        {
            if (node == null) return;
            if (node.ParentNode == null)
                node.Position.Y = VerticalNodeOffset;
            if (node.LeftNode != null)
                node.LeftNode.Position.Y = node.Position.Y + (CircleDiameter + VerticalNodeOffset);
            if (node.RightNode != null)
                node.RightNode.Position.Y = node.Position.Y + (CircleDiameter + VerticalNodeOffset);
            VerticalPositionsTree(node.LeftNode);
            VerticalPositionsTree(node.RightNode);
        }

        // Calculates line positions between nodes
        private void UpdateLinePositions()
        {
            LinePositions.Clear();
            foreach (var tree in Huffman.Trees)
                UpdateTreeLines(tree.Root);
        }

        // Helper function for UpdateLinePositions
        private void UpdateTreeLines(HuffmanNode node)
        {
            if (node == null)
                return;
            if (node.ParentNode != null)
            {
                var line = new LinePosition();
                double startX, startY;
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
                //line.StartPosition = new Position(node.ParentNode.Position.X + CircleDiameter / 2, node.ParentNode.Position.Y + CircleDiameter);
                line.StartPosition = new Position(startX, startY);
                line.EndPosition = new Position(node.Position.X + CircleDiameter / 2, node.Position.Y);
                LinePositions.Add(line);
            }
            UpdateTreeLines(node.LeftNode);
            UpdateTreeLines(node.RightNode);
        }

        // Updates Nodes in the algorithm
        private void UpdateNodesCollection()
        {
            Nodes.Clear();
            Huffman.Trees = Huffman.Trees.OrderBy(t => (int)t.Root.Value).ToList();
            foreach (var tree in Huffman.Trees)
                UpdateNodesFromTree(tree.Root);
        }

        // Helper function for UpdateNodesCollection
        private void UpdateNodesFromTree(HuffmanNode node)
        {
            if (node == null) return;
            UpdateNodesFromTree(node.LeftNode);
            Nodes.Add(node);
            UpdateNodesFromTree(node.RightNode);
        }

        // Calculates canvas sizes
        private void CalculateCanvasSize()
        {
            double maxWidth = 0;
            double maxHeight = 0;
            foreach (var node in Nodes)
            {
                if (node.Position.X + CircleDiameter > maxWidth)
                    maxWidth = node.Position.X + CircleDiameter;
                if (node.Position.Y + CircleDiameter > maxHeight)
                    maxHeight = node.Position.Y + CircleDiameter;
            }
            CanvasWidth = maxWidth + HorizontalNodeOffset;
            CanvasHeight = maxHeight + VerticalNodeOffset;
        }

        private void UpdateLineCodes()
        {
            LineCodes.Clear();
            if (Huffman.Trees.Count != 1) return;
            foreach (var line in LinePositions)
            {
                int code;
                if (line.StartPosition.X < line.EndPosition.X)
                    code = 1;
                else
                    code = 0;
                Position pos = new Position((line.StartPosition.X + line.EndPosition.X) / 2,
                    (line.StartPosition.Y + line.EndPosition.Y) / 2 );
                LineCode newLC = new LineCode(code, pos);
                LineCodes.Add(newLC);
            }
        }

        private void UpdateCharacterCodes()
        {
            CharacterCodes.Clear();
            if (Huffman.Trees.Count != 1)
            {
                TableVisible = false;
                return;
            }
            foreach(var ch in Characters)
            {
                string code = "";
                int freq;
                HuffmanNode node = Nodes.FirstOrDefault(n => n.Character == ch);
                freq = (int)node.Value;
                while(node.ParentNode != null)
                {
                    if (node.ParentNode.LeftNode == node)
                        code += "0";
                    else
                        code += "1";
                    node = node.ParentNode;
                }
                // Reversing array
                char[] charArray = code.ToCharArray();
                Array.Reverse(charArray);
                code = new string(charArray);
                // Adding code to list
                CharacterCode newCC = new CharacterCode(ch, (int)freq, code);
                CharacterCodes.Add(newCC);
            }
            
            // Calculating max Y position
            double yPos = 0;
            foreach (var node in Nodes)
                if (node.Position.Y > yPos)
                    yPos = node.Position.Y;
            yPos += CircleDiameter + 20; // moves datagrid below nodes

            // Calculating X position
            double xPos = CanvasWidth / 2 - 150;
            TablePosition = new Position(xPos, yPos);
            TableVisible = true;

            CanvasHeight += (Characters.Count + 3) * 30;
        }


        // Updates everything to draw latest algorithm state on the canvas
        public void UpdateUI()
        {
            UpdateNodesCollection();
            CalculateNodePositions();
            UpdateLinePositions();
            CalculateCanvasSize();
            UpdateLineCodes();
            UpdateCharacterCodes();
        }
        // Loads previous state of algorithm when loading view
        private void LoadSavedData()
        {
            if (SavedHVM == null) return;
            Huffman = SavedHVM.Huffman;
            EnteredText = SavedHVM.EnteredText;
            IsStarted = SavedHVM.IsStarted;
            PreviousStates = SavedHVM.PreviousStates;
            Characters = SavedHVM.Characters;
            UpdateUI();
        }
    }
}

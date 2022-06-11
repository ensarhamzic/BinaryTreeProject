using BinaryTreeProject.Models;
using BinaryTreeProject.ViewModels.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BinaryTreeProject.ViewModels
{
    public class HuffmanViewModel : BaseViewModel
    {
        private Huffman huffman; // instance of huffman algorithm
        private string enteredText; // text entered by user
        private bool isStarted; // is huffman algorithm started
        private Stack<List<HuffmanTree>> previousStates; // previous states of the algorithm
        public static HuffmanViewModel SavedHVM;

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
        public Stack<List<HuffmanTree>> PreviousStates
        {
            get { return previousStates; }
            set { previousStates = value; OnPropertyChanged("PreviousStates"); }
        }
        public string EnteredText
        {
            get { return enteredText; }
            set { enteredText = value; OnPropertyChanged("EnteredText"); }
        }
        public ObservableCollection<HuffmanNode> Nodes { get; set; }
        public ObservableCollection<LinePosition> LinePositions { get; set; }

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
            CircleDiameter = 50;
            CanvasWidth = 0;
            CanvasHeight = 0;
            VerticalNodeOffset = CircleDiameter * 0.5;
            HorizontalNodeOffset = CircleDiameter * 1.3;
            isStarted = false;
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
            IsStarted = true;
            PreviousStates.Clear();
            Huffman.Trees.Clear();
            UpdateUI();
            List<char> previousChars = new List<char>();
            // Creates trees for every node, while calculating its frequency
            foreach (char c in enteredText)
                if (!previousChars.Contains(c))
                {
                    int frequency = 0;
                    foreach (char ch in enteredText)
                    {
                        if (ch == c)
                            frequency++;
                    }
                    Huffman.Trees.Add(new HuffmanTree(new HuffmanNode(c, frequency)));
                    previousChars.Add(c);
                }
            UpdateUI();
        }

        // Calculates trees states of next step of algorithm and updates UI
        public void NextStep()
        {
            SaveCurrentState();
            HuffmanTree first = Huffman.Trees[0];
            HuffmanTree second = Huffman.Trees[1];
            HuffmanTree mergedTree = new HuffmanTree(new HuffmanNode(null,
                (int)first.Root.Value + (int)second.Root.Value));
            // makes new merged tree root as parent of first and second root (merging the trees)
            mergedTree.Root.LeftNode = first.Root;
            mergedTree.Root.RightNode = second.Root;
            first.Root.ParentNode = mergedTree.Root;
            second.Root.ParentNode = mergedTree.Root;
            Huffman.Trees.RemoveRange(0, 2);
            Huffman.Trees.Add(mergedTree);
            UpdateUI();
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
            Huffman.Trees = PreviousStates.Pop();
            UpdateUI();
        }

        // Ends the algorithm
        public void SkipToEnd()
        {
            while (Huffman.Trees.Count > 1)
                NextStep();
        }

        // Goes back to the start of the algorithm
        public void BackToStart()
        {
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
                line.StartPosition = new Position(node.ParentNode.Position.X + CircleDiameter / 2, node.ParentNode.Position.Y + CircleDiameter);
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

        // Updates everything to draw latest algorithm state on the canvas
        public void UpdateUI()
        {
            UpdateNodesCollection();
            CalculateNodePositions();
            UpdateLinePositions();
            CalculateCanvasSize();
        }
        // Loads previous state of algorithm when loading view
        private void LoadSavedData()
        {
            if (SavedHVM == null) return;
            Huffman = SavedHVM.Huffman;
            EnteredText = SavedHVM.EnteredText;
            IsStarted = SavedHVM.IsStarted;
            PreviousStates = SavedHVM.PreviousStates;
            UpdateUI();
        }
    }
}

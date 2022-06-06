using BinaryTreeProject.Models;
using BinaryTreeProject.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BinaryTreeProject.ViewModels
{
    public class HuffmanViewModel : BaseViewModel
    {
        private Huffman huffman; // instance of huffman algorithm
        private string enteredText; // text entered by user
        private int circleDiameter; // diameter of the circle representing a node
        private double canvasWidth; // width of the canvas
        private double canvasHeight; // height of the canvas
        private double verticalNodeOffset; // vertical offset between two nodes
        private double horizontalNodeOffset; // horizontal offset between two nodes
        private bool isStarted; // is huffman algorithm started
        private Stack<List<HuffmanTree>> previousStates; // previous states of the algorithm

        public Huffman Huffman
        {
            get { return huffman; }
            set
            {
                huffman = value;
                OnPropertyChanged("Huffman");
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
        public bool IsStarted
        {
            get
            {
                return isStarted;
            }
            set
            {
                isStarted = value;
                OnPropertyChanged("IsStarted");
            }
        }
        public Stack<List<HuffmanTree>> PreviousStates
        {
            get { return previousStates; }
            set
            {
                previousStates = value;
                OnPropertyChanged("PreviousStates");
            }
        }

        public ObservableCollection<HuffmanNode> Nodes { get; set; }
        public ObservableCollection<LinePosition> LinePositions { get; set; }
        public string EnteredText
        {
            get { return enteredText; }
            set
            {
                enteredText = value;
                OnPropertyChanged("EnteredText");
            }
        }

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
        }

        public void StartHuffman()
        {
            IsStarted = true;
            PreviousStates.Clear();
            Huffman.Trees.Clear();
            UpdateUI();
            List<char> previousChars = new List<char>();

            foreach (char c in enteredText)
            {
                if (!previousChars.Contains(c))
                {
                    int frequency = 0;
                    foreach (char ch in enteredText)
                    {
                        if (ch == c)
                        {
                            frequency++;
                        }
                    }
                    Huffman.Trees.Add(new HuffmanTree(new HuffmanNode(c, frequency)));
                    previousChars.Add(c);
                }
            }
            UpdateUI();
        }

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

        public void PreviousStep()
        {
            Huffman.Trees = PreviousStates.Pop();
            UpdateUI();
        }

        public void SkipToEnd()
        {
            while (Huffman.Trees.Count > 1)
            {
                NextStep();
            }
        }

        public void BackToStart()
        {
            while (PreviousStates.Count > 0)
            {
                PreviousStep();
            }
        }

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

        private void CalculateVerticalPositions()
        {
            foreach (var tree in Huffman.Trees)
            {
                VerticalPositionsTree(tree.Root);
            }
        }

        private void VerticalPositionsTree(HuffmanNode node)
        {
            if (node == null) return;

            if (node.ParentNode == null)
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

            VerticalPositionsTree(node.LeftNode);
            VerticalPositionsTree(node.RightNode);
        }

        private void UpdateLinePositions()
        {
            LinePositions.Clear();
            foreach (var tree in Huffman.Trees)
            {
                UpdateTreeLines(tree.Root);
            }
        }

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

        private void UpdateNodesCollection()
        {
            Nodes.Clear();
            Huffman.Trees = Huffman.Trees.OrderBy(t => (int)t.Root.Value).ToList();
            foreach (var tree in Huffman.Trees)
            {
                UpdateNodesFromTree(tree.Root);
            }
        }

        private void UpdateNodesFromTree(HuffmanNode node)
        {
            if (node == null)
                return;
            UpdateNodesFromTree(node.LeftNode);
            Nodes.Add(node);
            UpdateNodesFromTree(node.RightNode);
        }

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

        private void UpdateUI()
        {
            UpdateNodesCollection();
            CalculateNodePositions();
            UpdateLinePositions();
            CalculateCanvasSize();
        }
    }
}

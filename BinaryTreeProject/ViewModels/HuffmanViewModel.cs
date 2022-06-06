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
        private bool isStarted;

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
        public HuffmanViewModel()
        {
            Huffman = new Huffman();
            Nodes = new ObservableCollection<HuffmanNode>();
            LinePositions = new ObservableCollection<LinePosition>();
            CircleDiameter = 50;
            CanvasWidth = 0;
            CanvasHeight = 0;
            VerticalNodeOffset = CircleDiameter * 0.5;
            HorizontalNodeOffset = CircleDiameter * 0.7;
            isStarted = false;
            // commands
            StartCommand = new StartHuffmanCommand(this);
            NextStepCommand = new NextStepHuffmanCommand(this);
        }

        public void StartHuffman()
        {
            IsStarted = true;
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

        private void CalculateNodePositions()
        {
            double startX = 0; // must change
            foreach (var node in Nodes)
            {
                //if (node.ParentNode == null)
                //{
                    node.Position.X = startX + (horizontalNodeOffset * 2);
                    startX += HorizontalNodeOffset * 2;
                //}
                //else
                //{
                //    node.Position.X = startX + HorizontalNodeOffset;
                //    startX += HorizontalNodeOffset;
                //}
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

        private void UpdateUI()
        {
            UpdateNodesCollection();
            CalculateNodePositions();
            UpdateLinePositions();
        }
    }
}

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
        private Huffman huffman;
        private string enteredText;
        private int circleDiameter; // diameter of the circle representing a node
        private double canvasWidth; // width of the canvas
        private double canvasHeight; // height of the canvas
        private double verticalNodeOffset; // vertical offset between two nodes
        private double horizontalNodeOffset; // horizontal offset between two nodes

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

        public ObservableCollection<HuffmanNode> Nodes { get; set; }
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
        public HuffmanViewModel()
        {
            Huffman = new Huffman();
            Nodes = new ObservableCollection<HuffmanNode>();
            CircleDiameter = 50;
            CanvasWidth = 0;
            CanvasHeight = 0;
            VerticalNodeOffset = CircleDiameter * 0.5;
            HorizontalNodeOffset = CircleDiameter * 0.7;
            // commands
            StartCommand = new StartHuffmanCommand(this);
        }

        public void StartHuffman()
        {
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

        private void CalculateNodePositions()
        {
            double startX = 0; // must change
            foreach (var node in Nodes)
            {
                if (node.ParentNode == null)
                {
                    node.Position.X = startX + (horizontalNodeOffset * 2.5);
                    startX += HorizontalNodeOffset * 2.5;
                }
                else
                {
                    node.Position.X = startX + HorizontalNodeOffset;
                    startX += HorizontalNodeOffset;
                }
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
            //
        }

        private void UpdateNodesCollection()
        {
            Nodes.Clear();
            foreach (var tree in Huffman.Trees)
            {
                UpdateNodesFromTree(tree.Root);
            }
        }

        private void UpdateNodesFromTree(HuffmanNode node)
        {
            if (node == null)
            {
                return;
            }
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


namespace BinaryTreeProject.Models
{
    public class HuffmanNode : Node
    {
        private char? character;
        private HuffmanNode _leftNode;
        private HuffmanNode _rightNode;
        private HuffmanNode _parentNode;

        public new HuffmanNode LeftNode
        {
            get { return _leftNode; }
            set { _leftNode = value; OnPropertyChanged("LeftNode"); }
        }

        public new HuffmanNode RightNode
        {
            get { return _rightNode; }
            set { _rightNode = value; OnPropertyChanged("RightNode");}
        }

        public new HuffmanNode ParentNode
        {
            get { return _parentNode; }
            set { _parentNode = value; OnPropertyChanged("ParentNode"); }
        }


        public char? Character
        {
            get { return character; }
            set { character = value; OnPropertyChanged("Character"); }
        }
        public HuffmanNode(char? character, int frequency, int? id = null)
        {
            ID = id;
            Character = character;
            Value = frequency;
            Position = new Position();
        }
    }
}

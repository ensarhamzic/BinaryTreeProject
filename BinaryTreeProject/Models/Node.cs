using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTreeProject.Models
{
    internal class Node : INotifyPropertyChanged
    {
        private int? _value;
        private int? id;
        private Node _leftNode;
        private Node _rightNode;
        private Node _parentNode;
        private Position _position;

        public Position Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged("Position");
            }
        }

        public int? ID
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("ID");
            }
        }

        public int? Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        public Node LeftNode
        {
            get { return _leftNode; }
            set
            {
                _leftNode = value;
                OnPropertyChanged("LeftNode");
            }
        }

        public Node RightNode
        {
            get { return _rightNode; }
            set
            {
                _rightNode = value;
                OnPropertyChanged("RightNode");
            }
        }

        public Node ParentNode
        {
            get { return _parentNode; }
            set
            {
                _parentNode = value;
                OnPropertyChanged("ParentNode");
            }
        }

        public Node()
        {
            Value = null;
            LeftNode = null;
            RightNode = null;
            ParentNode = null;
            Position = null;
            ID = 0;
        }

        public Node(int? value, int? id)
        {
            Value = value;
            LeftNode = null;
            RightNode = null;
            ParentNode = null;
            Position = new Position();
            ID = id;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

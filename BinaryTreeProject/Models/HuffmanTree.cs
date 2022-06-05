using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTreeProject.Models
{
    public class HuffmanTree : INotifyPropertyChanged
    {
        public static int nodeId = 0;
        private HuffmanNode _root;

        public HuffmanNode Root
        {
            get { return _root; }
            set
            {
                _root = value;
                OnPropertyChanged("Root");
            }
        }

        public HuffmanTree()
        {
            Root = null;
        }

        public HuffmanTree(HuffmanNode root)
        {
            root.ID = ++nodeId;
            Root = root;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

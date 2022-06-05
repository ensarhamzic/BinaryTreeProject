using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTreeProject.Models
{
    public class Huffman : INotifyPropertyChanged
    {
        private List<HuffmanTree> trees;

        public List<HuffmanTree> Trees
        {
            get { return trees; }
            set
            {
                trees = value;
                OnPropertyChanged("Trees");
            }
        }

        public Huffman()
        {
            Trees = new List<HuffmanTree>();
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

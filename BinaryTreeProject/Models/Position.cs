using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BinaryTreeProject.Models
{
    internal class Position : INotifyPropertyChanged
    {
        private double _x;
        private double _y;

        public double X
        {
            get { return _x; }
            set
            {
                _x = value;
                OnPropertyChanged("X");
            }
        }

        public double Y
        {
            get { return _y; }
            set
            {
                _y = value;
                OnPropertyChanged("Y");
            }
        }

        public Position()
        {
            X = 0;
            Y = 0;
        }

        public Position(double x, double y)
        {
            X = x;
            Y = y;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

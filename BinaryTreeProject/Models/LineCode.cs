using System.ComponentModel;

namespace BinaryTreeProject.Models
{
    public class LineCode : INotifyPropertyChanged
    {
        private int _code;
        private Position _pos;

        public int Code
        {
            get { return _code; }
            set { _code = value; OnPropertyChanged("Code"); }
        }

        public Position Position
        {
            get { return _pos; }
            set { _pos = value; OnPropertyChanged("Pos"); }
        }

        public LineCode()
        {
            Code = 0;
            Position = new Position();
        }

        public LineCode(int code, Position pos)
        {
            Code = code;
            Position = pos;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

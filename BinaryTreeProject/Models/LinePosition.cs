using System.ComponentModel;

namespace BinaryTreeProject.Models
{
    public class LinePosition : INotifyPropertyChanged
    {
        private Position startPosition;
        private Position endPosition;

        public Position StartPosition
        {
            get { return startPosition; }
            set
            {
                startPosition = value;
                OnPropertyChanged("StartPosition");
            }
        }

        public Position EndPosition
        {
            get { return endPosition; }
            set
            {
                endPosition = value;
                OnPropertyChanged("EndPosition");
            }
        }

        public LinePosition()
        {
            startPosition = new Position();
            endPosition = new Position();
        }

        public LinePosition(double X1, double Y1, double X2, double Y2)
        {
            startPosition = new Position(X1, Y1);
            endPosition = new Position(X2, Y2);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

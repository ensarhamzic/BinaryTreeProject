using System.ComponentModel;

namespace BinaryTreeProject.Models
{
    public class CharacterCode : INotifyPropertyChanged
    {
        private char _character;
        private int _frequency;
        private string _code;

        public char Character
        {
            get { return _character; }
            set { _character = value; OnPropertyChanged("Character"); }
        }
        public int Frequency
        {
            get { return _frequency; }
            set { _frequency = value; OnPropertyChanged("Frequency"); }
        }

        public string Code
        {
            get { return _code; }
            set { _code = value; OnPropertyChanged("Code"); }
        }

        public CharacterCode()
        {
            Character = '/';
            Frequency = 0;
            Code = "0";
        }

        public CharacterCode(char ch, int freq, string code)
        {
            Character = ch;
            Frequency = freq;
            Code = code;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

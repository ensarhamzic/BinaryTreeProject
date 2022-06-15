using System.ComponentModel;

namespace BinaryTreeProject.Models
{
    public class CharacterCode : INotifyPropertyChanged
    {
        private char _character;
        private string _code;

        public char Character
        {
            get { return _character; }
            set { _character = value; OnPropertyChanged("Character"); }
        }

        public string Code
        {
            get { return _code; }
            set { _code = value; OnPropertyChanged("Code"); }
        }

        public CharacterCode()
        {
            Character = '/';
            Code = "0";
        }

        public CharacterCode(char ch, string code)
        {
            Character = ch;
            Code = code;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

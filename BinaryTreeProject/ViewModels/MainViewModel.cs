using System.ComponentModel;

namespace BinaryTreeProject.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private BaseViewModel selectedViewModel = new BinaryTreeViewModel();
        public BaseViewModel SelectedViewModel
        {
            get { return selectedViewModel; }
            set
            {
                selectedViewModel = value;
                OnPropertyChanged("SelectedViewModel");
            }
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

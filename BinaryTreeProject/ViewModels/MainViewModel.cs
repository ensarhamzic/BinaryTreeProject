using System.ComponentModel;

namespace BinaryTreeProject.ViewModels
{
    // Manages currently selected view model, which is used to display the corresponding view
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTreeProject.ViewModels
{
    public class MainViewModel : BaseViewModel
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



    }
}

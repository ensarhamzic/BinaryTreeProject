using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace BinaryTreeProject.Windows
{
    /// <summary>
    /// Interaction logic for LoadDBDialog.xaml
    /// </summary>
    public partial class LoadDBDialog : Window
    {
        public string TreeName
        {
            get { return TreeNameTB.Text; }
            set { TreeNameTB.Text = value; }
        }
        
        public LoadDBDialog()
        {
            InitializeComponent();
            (FindName("TreeNameTB") as TextBox).Focus();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

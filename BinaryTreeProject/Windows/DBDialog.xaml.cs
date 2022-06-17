using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BinaryTreeProject.Windows
{
    /// <summary>
    /// Interaction logic for DBDialog.xaml
    /// </summary>
    public partial class DBDialog : Window
    {
        public string TreeName
        {
            get { return TreeNameTB.Text; }
            set { TreeNameTB.Text = value; }
        }
        public DBDialog(string treeName = null)
        {
            InitializeComponent();
            (FindName("TreeNameTB") as TextBox).Focus();
            TreeName = treeName;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}

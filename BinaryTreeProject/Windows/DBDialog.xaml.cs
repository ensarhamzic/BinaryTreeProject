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
            var tb = FindName("TreeNameTB") as TextBox;
            tb.Focus();
            TreeName = treeName;
            tb.SelectionStart = tb.Text.Length;
            tb.SelectionLength = 0;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}

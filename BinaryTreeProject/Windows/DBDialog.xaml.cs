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
        public DBDialog()
        {
            InitializeComponent();
            (FindName("TreeNameTB") as TextBox).Focus();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (TreeName.Length <= 20 && TreeName.Length > 0)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MaxTB.Foreground = Brushes.Red;
                MessageBox.Show("Tree name length must be between 1 and 20 characters", "Error");
            }
        }
    }
}

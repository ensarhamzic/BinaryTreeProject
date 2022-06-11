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

        public DataTable NodesTable { get; set; }

        private Database Database;
        public LoadDBDialog()
        {
            InitializeComponent();
            (FindName("TreeNameTB") as TextBox).Focus();
            Database = new Database();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            if (TreeName.Length <= 20 && TreeName.Length > 0)
            {
                DataTable dt = Database.LoadTree(TreeName);
                if (dt == null)
                {
                    MessageBox.Show("Tree does not exist", "Error");
                }
                else
                {
                    NodesTable = dt;
                    this.DialogResult = true;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Tree name length must be between 1 and 20 characters", "Error");
            }
        }

    }
}

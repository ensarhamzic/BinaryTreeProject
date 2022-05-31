using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

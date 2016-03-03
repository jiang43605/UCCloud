using System.Windows;

namespace PrefixPlugin
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public string Prefix
        {
            get { return this.txtprefix.Text; }
            set { this.txtprefix.Text = value; }
        }

        public bool? CanAddPrefix
        {
            get { return this.checkBox.IsChecked; }
            set { this.checkBox.IsChecked = value; }
        }
    }
}

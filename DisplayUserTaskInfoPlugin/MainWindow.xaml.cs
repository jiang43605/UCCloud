using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DisplayUserTaskInfoPlugin
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

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //List<TaskModel> taskModellist = new List<TaskModel>();
            //TaskModel taskModel = new TaskModel()
            //{
            //    Status = "测试用例",
            //    Name = "测试",
            //    Size_fmt = "1.8TB",
            //    TButton = () => { MessageBox.Show("你点我了"); }
            //};
            //taskModellist.Add(taskModel);
            //this.dataGrid.ItemsSource = taskModellist;
        }

        public void ButtonBase_OnClick(object sender, RoutedEventArgs args)
        {
            Button button = (Button) sender;
            var rout = button.DataContext as Action;
            rout.Invoke();
        }
    }
}

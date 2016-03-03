using System;
using System.Linq;
using System.Windows.Controls;
using IUCWindow;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DisplayUserTaskInfoPlugin
{
    public class InfoPlugin : IUCWindowPlugin
    {
        private MainWindow _mainWindow = new MainWindow();
        private IUCDataHandle _iDataHandle;

        public string Name
        {
            get { return "任务表查看"; }
        }

        public void GetDataHandle(IUCDataHandle iDataHandle)
        {
            this._iDataHandle = iDataHandle;
        }

        public void SetLink(TextBox textBox)
        {
            // TODO
        }

        public void ShowWindow()
        {
            if (this._mainWindow.IsLoaded)
            {
                this._mainWindow.Activate();
                return;
            }

            this._mainWindow.Closed += (s, r) =>
            {
                this._mainWindow = new MainWindow();
            };

            this._mainWindow.Loaded += _mainWindow_Loaded;
            this._mainWindow.lbUpdata.PreviewMouseLeftButtonUp += LbUpdata_PreviewMouseLeftButtonUp;
            this._mainWindow.Show();
        }

        private void LbUpdata_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this._mainWindow_Loaded(this._mainWindow, null);
        }

        private void _mainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindow mainWindow = sender as MainWindow;
            Task.Factory.StartNew(() =>
            {
                mainWindow.Dispatcher.BeginInvoke(new Action(() =>
                {

                    var taskmodels = JsonConvert.DeserializeObject<List<TaskModel>>(this._iDataHandle?.GetUserTaskListData());
                    taskmodels.ForEach(o =>
                    {
                        o.Name = o.Name.Length > 20 ? o.Name.Substring(0, 20) + "..." : o.Name;
                        o.Status = o.Status.Contains("过期时间") ? "待过期" : o.Status;
                        o.TButton = () =>
                        {
                            var result = MessageBox.Show("你确定要删除它吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information);
                            if (result == MessageBoxResult.No) return;
                            MessageBox.Show(this._iDataHandle.DelTask(o.Task_id));
                            this._mainWindow_Loaded(this._mainWindow, null);
                        };
                    });
                    mainWindow.dataGrid.ItemsSource = taskmodels;
                }));
            });
        }

        private Button CreatButton()
        {
            Button button = new Button();
            button.Content = "删除";
            button.BorderBrush = null;
            button.Background = new SolidColorBrush(Colors.CadetBlue);
            button.Margin = new Thickness(2);
            return button;
        }
    }
}
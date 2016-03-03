using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using Chengf;
using DisplayUserTaskInfoPlugin;
using IUCWindow;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace PlayOrDownloadPlugin
{
    public class PlayOrDownload : IUCWindowPlugin
    {
        private IUCDataHandle _iDataHandle;
        private MainWindow _mainWindow = new MainWindow();
        private IEnumerable<DownloadModel> _downloadModels;
        private WebClient webClient = new WebClient();
        private string _exestring;
        public string Name
        {
            get { return "下载或播放"; }
        }

        public void GetDataHandle(IUCDataHandle iDataHandle)
        {
            this._iDataHandle = iDataHandle;
        }

        public void SetLink(TextBox textBox)
        {
            //
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
                this._mainWindow._timer.Close();
                this.webClient.Dispose();
                this._mainWindow = new MainWindow();
            };

            this._mainWindow.Loaded += _mainWindow_Loaded;
            this._mainWindow.btnPlay.Click += BtnPlay_Click;
            this._mainWindow.btndown.Click += Btndown_Click;
            this._mainWindow.labelHead.MouseLeftButtonUp += LabelHead_MouseLeftButtonUp;
            this._mainWindow.labelHead.ContextMenu = this.GetLableContextMenu();
            this._mainWindow.Show();
        }

        private void LabelHead_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!Filter) return;
            string name = this._mainWindow.cbox.SelectionBoxItem as string;
            DownloadModel downloadModel = this._downloadModels?.Where(o => o != null && o.Name == name).FirstOrDefault();

            if (this._exestring == null)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = false;
                openFileDialog.Filter = "可执行文件|*.exe";

                bool? r = openFileDialog.ShowDialog();
                if (r != true) return;
                this._exestring = openFileDialog.FileName;
            }

            try
            {
                if (!string.IsNullOrEmpty(downloadModel.Link_hi))
                    Process.Start(this._exestring, downloadModel.Link_hi);
                else if (!string.IsNullOrEmpty(downloadModel.Link_low))
                    Process.Start(this._exestring, downloadModel.Link_low);
                else MessageBox.Show("获取播放链接失败");
            }
            catch (Exception exception)
            {
                this._exestring = null;
                MessageBox.Show(exception.Message);
            }
        }

        private void Btndown_Click(object sender, RoutedEventArgs e)
        {
            if (!Filter) return;

            string name = this._mainWindow.cbox.SelectionBoxItem as string;
            DownloadModel downloadModel = this._downloadModels?.Where(o => o != null && o.Name == name).FirstOrDefault();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = downloadModel.Name;
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.AddExtension = true;
            saveFileDialog.Title = "保存视频";

            bool? r = saveFileDialog.ShowDialog();
            if (r != true) return;

            webClient.DownloadFileCompleted += WebClientOnDownloadFileCompleted;
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;

            if (this._mainWindow.checkboxIshigh.IsChecked == true && downloadModel.Link_hi != null)
                webClient.DownloadFileAsync(new Uri(downloadModel.Link_hi), saveFileDialog.FileName);
            else
                webClient.DownloadFileAsync(new Uri(downloadModel.Link_low), saveFileDialog.FileName);

        }

        private void WebClientOnDownloadFileCompleted(object sender, AsyncCompletedEventArgs asyncCompletedEventArgs)
        {
            MessageBox.Show("下载完成", "下载提示", MessageBoxButton.OK, MessageBoxImage.Information);
            this._mainWindow.Pb.Value = 0;
            this._mainWindow.lbpb.Content = "0%[0B]";
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (!Filter) return;

            string name = this._mainWindow.cbox.SelectionBoxItem as string;
            DownloadModel downloadModel = this._downloadModels?.Where(o => o != null && o.Name == name).FirstOrDefault();

            this._mainWindow.mediaElement.LoadedBehavior = MediaState.Manual;
            if (this._mainWindow.checkboxIshigh.IsChecked == true && downloadModel.Link_hi != null)
                this._mainWindow.mediaElement.Source = new Uri(downloadModel.Link_hi);
            else if (downloadModel.Link_low != null)
                this._mainWindow.mediaElement.Source = new Uri(downloadModel.Link_low);
            else { MessageBox.Show("由于UC的原因该视频无法播放，正在准备当中"); return; }
            this._mainWindow.lbuffer.Visibility = Visibility.Visible;
            this._mainWindow.mediaElement.Play();
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            int i = e.ProgressPercentage;
            this._mainWindow.Pb.Dispatcher.BeginInvoke(new Action(() =>
            {
                this._mainWindow.Pb.Value = i;
                this._mainWindow.lbpb.Content = $"{i}%[{Cf_IO.FormatSize(e.TotalBytesToReceive)}]";
            }));
        }

        private void _mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this._mainWindow.btnPlay.IsEnabled = false;
            this._mainWindow.btndown.IsEnabled = false;
            this._mainWindow.Pb.Maximum = 100;

            Task.Factory.StartNew(() =>
            {
                var taskmodels = JsonConvert.DeserializeObject<TaskModel[]>(this._iDataHandle.GetUserTaskListData());
                IEnumerable<DownloadModel[]> downloadModelses = IHelp.GetDownloadList(taskmodels);
                this._downloadModels = downloadModelses.Where(o => o.Length > 0).Select(o => o.FirstOrDefault(p => IHelp.IsMovie(p.Name)));

                string[] itemStrings = this._downloadModels.Where(o => o != null).Select(o => o.Name).ToArray();
                this._mainWindow.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this._mainWindow.cbox.ItemsSource = itemStrings;
                    this._mainWindow.btnPlay.IsEnabled = true;
                    this._mainWindow.btndown.IsEnabled = true;
                }));
            });

        }

        private bool Filter
        {
            get
            {
                if (this._downloadModels == null || this._mainWindow.cbox.SelectedItem == null)
                {
                    MessageBox.Show("未获取到电影资源！");
                    return false;
                }
                return true;
            }
        }

        private ContextMenu GetLableContextMenu()
        {
            ContextMenu contextMenu = new ContextMenu();

            MenuItem menuItem_about = new MenuItem() { Header = "关于" };
            MenuItem menuItem_reset = new MenuItem() { Header = "重置第三方播放器" };

            const string about = "单击此标题可以调用你事先设置好的（即第一次设置的值）第三方播放器来播放当前选中的影片（推荐PotPaly）," +
                                 "选择重置第三方播放器可以重新指定一个播放器";
            menuItem_about.Click += (a, b) => { MessageBox.Show(about); };
            menuItem_reset.Click += (a, b) => { this._exestring = null; };

            contextMenu.Items.Add(menuItem_about);
            contextMenu.Items.Add(menuItem_reset);

            return contextMenu;
        }

    }
}
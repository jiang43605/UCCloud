using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using Timer = System.Timers.Timer;

namespace PlayOrDownloadPlugin
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _canpause;
        private bool _changeposition;
        public Timer _timer = new Timer(1000);
        public MainWindow()
        {
            InitializeComponent();
            //this.mediaElement.Source = new Uri("‪sss.mp4", UriKind.RelativeOrAbsolute);
            //this.mediaElement.LoadedBehavior = MediaState.Manual;
            //this.mediaElement.Play();
        }

        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            this.lbuffer.Visibility = Visibility.Hidden;
            this.sliderPosition.Value = 0;
            this.sliderPosition.Maximum = this.mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
            this._timer.Elapsed += _timer_Elapsed;
            this._timer.Start();
        }
        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this._timer.Stop();
            this.Dispatcher.Invoke(new Action(() =>
            {
                if (this._changeposition) return;
                var position = this.mediaElement.Position.TotalSeconds;
                this.sliderPosition.Value = position;
                this.Pbonsliderdisplaybuffer.Value = this.mediaElement.DownloadProgress;
                this.lbplay.Content = $"{this.mediaElement.Position.Hours.ToString("00")}:{this.mediaElement.Position.Minutes.ToString("00")}:{this.mediaElement.Position.Seconds.ToString("00")}" +
                                      $"/{this.mediaElement.NaturalDuration.TimeSpan.Hours.ToString("00")}:{this.mediaElement.NaturalDuration.TimeSpan.Minutes.ToString("00")}:{this.mediaElement.NaturalDuration.TimeSpan.Seconds.ToString("00")}";
            }));
            this._timer.Start();
        }

        private void mediaElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MediaElement midElement = sender as MediaElement;
            if (!_canpause)
            {
                midElement.Pause();
                this._canpause = true;
                return;
            }
            midElement.Play();
            this._canpause = false;
        }
        private void mediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            var r = MessageBox.Show($"播放出现错误，错误信息：{e.ErrorException.Message}，是否尝试重播？", "错误提示", MessageBoxButton.YesNo, MessageBoxImage.Error);
            if (r == MessageBoxResult.Yes)
            {
                this.mediaElement.LoadedBehavior = MediaState.Manual;
                this.mediaElement.Position = TimeSpan.Zero;
                this.mediaElement.Play();
                return;
            }

            this.sliderPosition.Value = 0;
            while (this._timer.Enabled)
            {
                this._timer.Stop();
            }
        }

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            while (this._timer.Enabled)
            {
                this._timer.Stop();
            }
        }

        private void sliderPosition_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.mediaElement.Position = TimeSpan.FromSeconds(this.sliderPosition.Value);
            this._changeposition = false;
        }

        private void sliderPosition_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this._changeposition = true;
        }

        /// <summary>
        /// 暂时不启用，频繁访问太恶心了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!this._changeposition) return;
        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            WindowState windowState = this.WindowState;
            if (windowState == WindowState.Maximized)
            {
                Grid.SetColumn(this.mediaElement, 1);
                Grid.SetRow(this.mediaElement, 1);
                Grid.SetRowSpan(this.mediaElement, 3);

                Grid.SetColumn(this.GridPlay, 1);
                Grid.SetColumnSpan(this.GridPlay, 2);

                this.GridPb.Visibility = Visibility.Hidden;
                this.RectanglePlay.Visibility = Visibility.Hidden;
                this.labelHead.Foreground = new SolidColorBrush(Colors.White);
                this.lbplay.FontSize = 20;
                this.lbplay.Foreground = new SolidColorBrush(Colors.White);
                this.GridControl.Visibility = Visibility.Hidden;
                this.maingrid.Background = new SolidColorBrush(Colors.Black);
            }
            else
            {
                this.GridPb.Visibility = Visibility.Visible;
                this.RectanglePlay.Visibility = Visibility.Visible;
                this.labelHead.Foreground = new SolidColorBrush(Colors.Gray);
                this.lbplay.FontSize = 8;
                this.lbplay.Foreground = new SolidColorBrush(Colors.Teal);
                this.GridControl.Visibility = Visibility.Visible;
                this.maingrid.Background = null;

                Grid.SetColumn(this.mediaElement, 1);
                Grid.SetColumnSpan(this.mediaElement, 1);
                Grid.SetRow(this.mediaElement, 3);
                Grid.SetRowSpan(this.mediaElement, 1);

                Grid.SetColumnSpan(this.GridPlay, 1);
                Grid.SetColumn(this.GridPlay, 1);
            }
        }

        private void sliderPosition_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (!_canpause)
                {
                    this.mediaElement.Pause();
                    this._canpause = true;
                    return;
                }
                this.mediaElement.Play();
                this._canpause = false;
            }

            //TODO 暂时不起作用
            //if (e.Key == Key.Right)
            //{
            //    this._changeposition = true;
            //    this.mediaElement.Position = this.mediaElement.Position.Add(TimeSpan.FromSeconds(10));
            //    this._changeposition = false;
            //}
            //if (e.Key == Key.Left)
            //{
            //    this._changeposition = true;
            //    this.mediaElement.Position = this.mediaElement.Position.Subtract(TimeSpan.FromSeconds(10));
            //    this._changeposition = false;
            //}
        }
    }
}

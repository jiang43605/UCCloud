﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using IUCWindow;
using UCCloudDisc;

namespace UC.Windows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private UCLogin _ucLogin;
        private UCDownload _ucDownload;
        private const string _pluginPath = "../Plug-in";
        private IEnumerable<IUCWindowPlugin> _iUcWindowPlugins;
        private readonly Queue<TaskModel> _updataTaskModels = new Queue<TaskModel>();             // 记录需要更新TaskModel的实例
        private readonly Dictionary<string, int> _itemDictionary = new Dictionary<string, int>(); // ListBox资源字典
        private const string SerializePath = "CookieContainer.dat";                     // Cookie保存路径
        public MainWindow()
        {
            InitializeComponent();
           // AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.IsTerminating)
                MessageBox.Show("发生致命错误，请重启程序！");
            Exception ex = e.ExceptionObject as Exception;
            MessageBox.Show(ex.Message);
        }
        /// <summary>
        /// 窗体Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.logincontrol.IsEnabled = false;

            Task.Factory.StartNew(this.UpdataTaskModelMonitor);

            this.InitializeCode();
        }
        /// <summary>
        /// 初始化代码
        /// </summary>
        public void InitializeCode()
        {
            // 一些初始化任务
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                // 读取插件
                this._iUcWindowPlugins = UCWindowHelp.ReadPlugin(_pluginPath);
                this.plugincomboBox.ItemsSource = UCWindowHelp.CreatButtons(this._iUcWindowPlugins);

                this._ucLogin = new UCLogin();
                UCHelp.BinaryDeserializeCookieContainer(SerializePath, ref this._ucLogin);
                if (this._ucLogin.IsLogin)
                {
                    // cookiecontaniner赋值后已经登录
                    this.lblogininfo.Content = this._ucLogin.LoginResultMsg.Name + "用户已经成功登录！";
                    this.logincontrol.Visibility = Visibility.Hidden;
                    this.maincontrol.Visibility = Visibility.Visible;
                    this._ucDownload = new UCDownload(this._ucLogin.HttpWeb);
                    UCWindowHelp.InitializeIUCWindowPlugin(this._iUcWindowPlugins, this._ucLogin.HttpWeb);
                    return;
                }
                this.logincontrol.IsEnabled = true;
                byte[] bytes = this._ucLogin.GetCaptchaBytes();
                this.SetBytesToImageAsyn(bytes);
            }));
        }
        /// <summary>
        /// 登录按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnlogin_Click(object sender, RoutedEventArgs e)
        {
            this.btnlogin.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (this.imagecaptcha.Source == null) return;

                if (string.IsNullOrEmpty(this.txtusername.Text) || string.IsNullOrEmpty(this.txtPassword.Password) ||
                    string.IsNullOrEmpty(this.txtcaptcha.Text))
                {
                    MessageBox.Show("帐号或者密码或者验证码不能为空！");
                    return;
                }

                this._ucLogin.Username = this.txtusername.Text;
                this._ucLogin.Password = this.txtPassword.Password;
                this.btnlogin.IsEnabled = false;

                // 登录
                var loginstatus = this._ucLogin.Login(this.txtcaptcha.Text);

                switch (loginstatus)
                {
                    case LoginStatus.InvalidLoginnameOrPassword:
                        {
                            MessageBox.Show("帐号或者密码错误");
                            this.SetBytesToImageAsyn(this._ucLogin.ErrorCaptcha());
                            this.btnlogin.IsEnabled = true;
                            return;
                        }
                    case LoginStatus.InvalidCaptcha:
                        {
                            MessageBox.Show("验证码不正确");
                            this.SetBytesToImageAsyn(this._ucLogin.ErrorCaptcha());
                            this.btnlogin.IsEnabled = true;
                            this.txtcaptcha.Clear();
                            return;
                        }
                    case LoginStatus.Success:
                        {
                            this.lblogininfo.Content = this._ucLogin.LoginResultMsg.Name + "用户已经成功登录！";
                            this.logincontrol.Visibility = Visibility.Hidden;
                            this.maincontrol.Visibility = Visibility.Visible;
                            this._ucDownload = new UCDownload(this._ucLogin.HttpWeb);
                            this.btnlogin.IsEnabled = true;
                            UCWindowHelp.InitializeIUCWindowPlugin(this._iUcWindowPlugins, this._ucLogin.HttpWeb);
                            return;
                        }
                    default:
                        {
                            MessageBox.Show("发生未知错误！");
                            this.btnlogin.IsEnabled = true;
                            return;
                        }
                }
            }));
        }
        /// <summary>
        /// 单击验证码图片事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imagecaptcha_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            byte[] bytes = this._ucLogin?.RefreshCaptcha();
            this.SetBytesToImageAsyn(bytes);
        }
        /// <summary>
        /// 使Bytes在图像控件中显示出来，异步的
        /// </summary>
        /// <param name="bytes"></param>
        private void SetBytesToImageAsyn(byte[] bytes)
        {
            this.imagecaptcha.Dispatcher.BeginInvoke(new Action(() =>
            {
                using (MemoryStream memoryStream = new MemoryStream(bytes, 0, bytes.Length))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = memoryStream;
                    bitmapImage.EndInit();
                    this.imagecaptcha.Source = bitmapImage;
                }
            }));
        }
        /// <summary>
        /// 添加新任务的按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddTask_Click(object sender, RoutedEventArgs e)
        {
            this.btnAddTask.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (string.IsNullOrEmpty(this.txtlink.Text))
                {
                    MessageBox.Show("链接不能为空");
                    return;
                }

                // 执行插件方法
                try { this._iUcWindowPlugins.ToList().ForEach(o => o.SetLink(this.txtlink)); } catch { }

                if (this._ucDownload == null) throw new ArgumentNullException("_ucDownload");
                bool tempbool = this._ucLogin.IsLogin;
                var msg = this._ucDownload.SetNewTask(this._ucLogin.LoginResultMsg.Token, this.txtlink.Text);
                this.listBox.Items.Add(msg.Msg);

                var tasks = this._ucDownload.GeTaskModel();
                var lists = tasks.Where(o => !o.Success && !o.Failed).ToList();

                lock (this._updataTaskModels)
                {
                    lists.ForEach(o =>
                    {
                        // 检查是否队列中已经存在，并且完成第一次数据展示
                        if (this._updataTaskModels.FirstOrDefault(p => p.Task_id == o.Task_id) == null)
                        {
                            this._updataTaskModels.Enqueue(o);
                            this.DisplayDataInListBox(o);
                        }
                    });
                }
                //this.txtlink.Clear();
            }));
        }
        /// <summary>
        /// 监控_updataTaskModels内元素
        /// </summary>
        private void UpdataTaskModelMonitor()
        {
            while (true)
            {
                if (this._updataTaskModels.Count > 0)
                {
                    var taskModel = this._updataTaskModels.Dequeue();

                    // 更新数据
                    var tasks = this._ucDownload.GeTaskModel();
                    taskModel = tasks.FirstOrDefault(o => o.Task_id == taskModel.Task_id);
                    if (!taskModel.Status.Contains("过期时间") && !taskModel.Status.Contains("已存至网盘") && !taskModel.Failed)
                        this._updataTaskModels.Enqueue(taskModel);
                    else taskModel.Status = taskModel.Status.Contains("过期时间") == true ? "离线完成" : taskModel.Status;

                    // 将数据展示到ListBox上面
                    this.DisplayDataInListBox(taskModel);

                    Thread.Sleep(3000);
                }
                else
                    Thread.Sleep(10000);
            }
        }
        /// <summary>
        /// 综合this._itemDictionary和传入的TaskModel来更新ListBox上面的数据
        /// </summary>
        /// <param name="taskModel"></param>
        private void DisplayDataInListBox(TaskModel taskModel)
        {
            string msginlistbox = $"[{taskModel.Status}][{taskModel.Size_fmt}][{taskModel.Fullname}]";
            bool tempbool = this._itemDictionary.ContainsKey(taskModel.Task_id);
            this.listBox.Dispatcher.Invoke(() =>
            {
                if (tempbool)
                {
                    int index = this._itemDictionary[taskModel.Task_id];
                    this.listBox.Items[index] = msginlistbox;
                }
                else
                {
                    this._itemDictionary.Add(taskModel.Task_id, this.listBox.Items.Count);
                    this.listBox.Items.Add(msginlistbox);
                }
            });
        }
        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mianwindow_Closed(object sender, EventArgs e)
        {
            if (this._ucLogin?.IsLogin == true)
                UCHelp.BinarySerializeCookieContainer(SerializePath, this._ucLogin.HttpWeb.HttpCookieContainer);
            Environment.Exit(0);
        }
        /// <summary>
        /// 退出登录按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnlogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("你确定要退出吗？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (mbr == MessageBoxResult.No) return;

            File.Delete(SerializePath);
            this.logincontrol.Visibility = Visibility.Visible;
            this.maincontrol.Visibility = Visibility.Hidden;
            this.logincontrol.IsEnabled = true;
            this._ucLogin.LogOut();
            byte[] bytes = this._ucLogin.GetCaptchaBytes();
            SetBytesToImageAsyn(bytes);
        }

        /// <summary>
        /// 验证码框获得焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtcaptcha_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.txtcaptcha.Text != "请输入验证码") return;
            this.txtcaptcha.Text = "";
            this.txtcaptcha.Foreground = new SolidColorBrush(Colors.Black);
        }
        /// <summary>
        /// 验证码框失去焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtcaptcha_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtcaptcha.Text)) return;
            this.txtcaptcha.Text = "请输入验证码";
            this.txtcaptcha.Foreground = new SolidColorBrush(Colors.Gray);
        }
    }
}
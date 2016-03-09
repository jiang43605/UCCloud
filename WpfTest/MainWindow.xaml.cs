using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using CefSharp;
using CefSharp.Wpf;
using IUCWindow;

namespace WpfTest
{
    class CookieMonster : ICookieVisitor
    {
        readonly List<Tuple<string, string>> cookies = new List<Tuple<string, string>>();
        readonly Action<IEnumerable<Tuple<string, string>>> useAllCookies;

        public CookieMonster(Action<IEnumerable<Tuple<string, string>>> useAllCookies)
        {
            this.useAllCookies = useAllCookies;
        }

        public bool Visit(Cookie cookie, int count, int total, ref bool deleteCookie)
        {
            cookies.Add(new Tuple<string, string>(cookie.Name, cookie.Value));

            if (count == total - 1)
                useAllCookies(cookies);

            return true;
        }
    }
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private IUCWindowPlugin _icPlugin;
        private ChromiumWebBrowser chromium;
        public MainWindow()
        {
            InitializeComponent();
            Cef.Initialize();
            chromium = new ChromiumWebBrowser();
            chromium.Address = "http://disk.yun.uc.cn/";
            chromium.FrameLoadEnd += Chromium_FrameLoadEnd;
            this.Gridbrower.Children.Add(chromium);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //this._icPlugin.ShowWindow();         

        }

        private void Chromium_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Chromium_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Browser.IsLoading) return;

            var visitor = new CookieMonster(all_cookies =>
            {
                var sb = new StringBuilder();
                foreach (var nameValue in all_cookies)
                    sb.AppendLine(nameValue.Item1 + " = " + nameValue.Item2);
                this.Dispatcher.BeginInvoke(new MethodInvoker(() =>
                {
                   // System.Windows.MessageBox.Show(sb.ToString());
                }));
            });

            Cef.GetGlobalCookieManager().VisitAllCookies(visitor);

            var scriptext = System.IO.File.ReadAllText("UC.js");
            chromium.ExecuteScriptAsync(scriptext);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Assembly assembly = Assembly.LoadFile(Path.GetFullPath("PluginWindow.exe"));
            //var types = assembly.GetTypes();
            //foreach (var item in types)
            //{
            //    if (!typeof(IUCWindowPlugin).IsAssignableFrom(item)) continue;
            //    this._icPlugin = Activator.CreateInstance(item) as IUCWindowPlugin;
            //}
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            chromium.Dispose();

            Cef.Shutdown();

            Environment.Exit(0);
        }
    }
}

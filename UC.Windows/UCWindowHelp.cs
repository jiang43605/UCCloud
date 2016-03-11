using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using CefSharp;
using CefSharp.Wpf;
using Chengf;
using IUCWindow;


namespace UC.Windows
{
    /// <summary>
    /// UC.Windows内部帮助类
    /// </summary>
    internal static class UCWindowHelp
    {
        /// <summary>
        /// 创建一个ChromiumWebBrowser浏览器
        /// </summary>
        /// <param name="endEventHandler"></param>
        /// <returns></returns>
        internal static ChromiumWebBrowser CreatChromiumWebBrowser(EventHandler<FrameLoadEndEventArgs> endEventHandler )
        {
            ChromiumWebBrowser webBrowser = new ChromiumWebBrowser();
            webBrowser.Address = "http://disk.yun.uc.cn/";
            webBrowser.FrameLoadEnd += endEventHandler;
            return webBrowser;
        }
        /// <summary>
        /// 创建一个TextBox用于ListBox
        /// </summary>
        /// <returns></returns>
        internal static TextBox CreatLabel(string text)
        {
            TextBox textBox = new TextBox();
            textBox.TextWrapping = TextWrapping.Wrap;
            textBox.HorizontalAlignment = HorizontalAlignment.Left;
            textBox.HorizontalContentAlignment = HorizontalAlignment.Left;
            textBox.BorderBrush = null;
            textBox.AcceptsReturn = true;
            textBox.Text = text;
            return textBox;
        }

        /// <summary>
        /// 根据IEnumerable<IUCWindowPlugin/>创建对应的Button
        /// </summary>
        /// <returns></returns>
        internal static IEnumerable<Control> CreatButtons(IEnumerable<IUCWindowPlugin> icws)
        {
            if (icws == null) return new List<Control>();

            List<Control> controls = new List<Control>();

            foreach (IUCWindowPlugin icw in icws)
            {
                Label control = new Label();
                control.Margin = new Thickness(0);
                control.BorderBrush = null;
                control.Content = icw.Name;
                control.PreviewMouseLeftButtonUp += (s, r) => icw.ShowWindow();

                controls.Add(control);
            }
            return controls;
        }
        /// <summary>
        /// 初始化IUCWindowPlugin
        /// </summary>
        /// <param name="icw"></param>
        internal static void InitializeIUCWindowPlugin(IEnumerable<IUCWindowPlugin> icws, Cf_HttpWeb cfHttpWeb)
        {
            foreach (IUCWindowPlugin icw in icws)
            {
                icw.GetDataHandle(new UCConnectData(cfHttpWeb));
            }
        }
        /// <summary>
        /// 在指定的pat目录中返回所有插件的实例
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static IEnumerable<IUCWindowPlugin> ReadPlugin(string path)
        {
            List<IUCWindowPlugin> icws = new List<IUCWindowPlugin>();

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return null;
            }

            string[] files = Directory.GetFiles(path);
            files = files.Select(o => Path.GetFullPath(o)).ToArray();
            foreach (string file in files)
            {
                if (Path.GetExtension(file) != ".exe" && Path.GetExtension(file) != ".dll") continue;

                Assembly assembly = Assembly.LoadFile(file);
                var types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (!typeof(IUCWindowPlugin).IsAssignableFrom(type)) continue;
                    IUCWindowPlugin icw = Activator.CreateInstance(type) as IUCWindowPlugin;
                    icws.Add(icw);
                }
            }
            return icws;
        }
    }
}
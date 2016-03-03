using System;
using System.Windows.Controls;
using IUCWindow;
using PrefixPlugin;

namespace PluginWindow
{
    public class PluginWindow : IUCWindowPlugin
    {
        private MainWindow _mainWindow = new MainWindow();
        private string _prefix;
        private bool? _canAddPrefix;
        public string Name
        {
            get { return "前缀增强"; }
        }

        public void SetLink(TextBox textBox)
        {
            if (this._mainWindow.CanAddPrefix == true)
            {
                textBox.Text = this._mainWindow.Prefix + textBox.Text;
            }
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
                this._prefix = this._mainWindow.Prefix;
                this._canAddPrefix = this._mainWindow.CanAddPrefix;

                this._mainWindow = new MainWindow();

                this._mainWindow.Prefix = this._prefix;
                this._mainWindow.CanAddPrefix = this._canAddPrefix;
            };
            this._mainWindow.Show();
        }

        public void GetDataHandle(IUCDataHandle iDataHandle)
        {
            // TODO
        }
    }
}

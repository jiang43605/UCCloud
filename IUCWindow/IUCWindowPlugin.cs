using System.Windows.Controls;

namespace IUCWindow
{
    /// <summary>
    /// UC.Windows对外开放的插件接口
    /// </summary>
    public interface IUCWindowPlugin
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 获取主程序的内部数据
        /// </summary>
        /// <returns></returns>
        void GetDataHandle(IUCDataHandle iDataHandle);
        /// <summary>
        /// 在提交链接之前执行
        /// </summary>
        /// <param name="textBox"></param>
        void SetLink(TextBox textBox);
        /// <summary>
        /// 显示窗口
        /// </summary>
        void ShowWindow();
        
    }
}
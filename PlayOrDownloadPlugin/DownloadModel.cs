namespace PlayOrDownloadPlugin
{
    /// <summary>
    /// 下载模型
    /// </summary>
    public class DownloadModel
    {
        /// <summary>
        /// 影片名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 低质量
        /// </summary>
        public string Link_low { set; get; }
        /// <summary>
        /// 高质量
        /// </summary>
        public string Link_hi { set; get; }
    }
}
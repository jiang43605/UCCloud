namespace IUCWindow
{
    /// <summary>
    /// 数据接口
    /// </summary>
    public interface IUCDataHandle
    {
        /// <summary>
        /// 获取用户的任务列表信息，返回的为json格式数据
        /// </summary>
        /// <returns></returns>
        string GetUserTaskListData();
        /// <summary>
        /// 删除任务
        /// </summary>
        string DelTask(string taskid);
    }
}
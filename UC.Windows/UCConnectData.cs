using System;
using System.Net;
using Chengf;
using IUCWindow;
using Newtonsoft.Json;
using UCCloudDisc;

namespace UC.Windows
{
    /// <summary>
    /// 对外展示内部数据
    /// </summary>
    public class UCConnectData : IUCDataHandle
    {
        private Cf_HttpWeb _httpWeb;
        public UCConnectData(Cf_HttpWeb cfHttpWeb)
        {
            this._httpWeb = cfHttpWeb;
        }

        public string DelTask(string taskid)
        {
            UCDownload ucDownload = new UCDownload(this._httpWeb);
            return ucDownload.DelTask(new TaskModel() {Task_id = taskid}).Msg;
        }

        public string GetUserTaskListData()
        {
            UCDownload ucDownload = new UCDownload(this._httpWeb);
            return JsonConvert.SerializeObject(ucDownload.GeTaskModel());
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Chengf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UCCloudDisc.Model;

namespace UCCloudDisc
{
    /// <summary>
    /// UC下载
    /// </summary>
    public class UCDownload
    {
        private Cf_HttpWeb _httpWeb;

        public Cf_HttpWeb HttpWeb
        {
            get
            {
                return this._httpWeb;
            }

            set
            {
                this._httpWeb = value;
            }
        }

        public UCDownload(Cf_HttpWeb httpweb)
        {
            this._httpWeb = httpweb;
        }

        /// <summary>
        /// 新建一个任务
        /// </summary>
        /// <param name="token"></param>
        /// <param name="tasklink"></param>
        /// <returns></returns>
        public MsgModel SetNewTask(string token, string tasklink)
        {
            const string tasklinkurl = "http://disk.yun.uc.cn/ajax/newtask";

            this._httpWeb.Accpet = "application/json, text/javascript, *; q=0.01";
            this._httpWeb.HeaderAdd.Add("Accept-Language", "zh-CN,zh;q=0.8");
            this._httpWeb.HeaderAdd.Add("X-Requested-With", "XMLHttpRequest");
            this._httpWeb.HeaderAdd.Add("DNT", "1");
            this._httpWeb.HeaderAdd.Add("RA-Ver", "3.0.7");
            this._httpWeb.HeaderAdd.Add("Origin", "http://disk.yun.uc.cn");
            this._httpWeb.EncodingSet = "utf-8";
            this._httpWeb.ContentType = "application/x-www-form-urlencoded";
            this._httpWeb.Referer = "http://disk.yun.uc.cn/";
            string tempstring = $"newlx_url={HttpUtility.UrlEncode(tasklink, Encoding.UTF8)}&token={token}";
            byte[] bytes = Encoding.UTF8.GetBytes(tempstring);
            using (MemoryStream memoryStream = new MemoryStream(bytes, 0, bytes.Length))
            {
                var msg = this.HttpWeb.PostOrGet(tasklinkurl, HttpMethod.POST, memoryStream).HtmlValue;
                return JsonConvert.DeserializeObject<MsgModel>(msg);
            }
        }

        /// <summary>
        /// 删除某项任务
        /// </summary>
        /// <returns></returns>
        public MsgModel DelTask(TaskModel taskModel)
        {
            const string dellinkuri = "http://disk.yun.uc.cn/ajax/dellxTasks";
            this._httpWeb.Referer = "http://disk.yun.uc.cn/";
            this._httpWeb.HeaderAdd.Add("Origin", "http://disk.yun.uc.cn");
            this._httpWeb.Accpet = "application/json, text/javascript, *; q=0.01";
            this._httpWeb.ContentType = "application/x-www-form-urlencoded";
            string tempstring = "taskids%5B%5D=" + taskModel.Task_id;
            byte[] bytes = Encoding.UTF8.GetBytes(tempstring);
            using (MemoryStream memoryStream = new MemoryStream(bytes, 0, bytes.Length))
            {
                string msg = this.HttpWeb.PostOrGet(dellinkuri, HttpMethod.POST,memoryStream).HtmlValue;
                return JsonConvert.DeserializeObject<MsgModel>(msg);
            }
        }

        /// <summary>
        /// 获取任务列表，出错时返回null
        /// </summary>
        /// <returns></returns>
        public TaskModel[] GeTaskModel()
        {
            string linkurl = "http://disk.yun.uc.cn/uclxmgr/ajaxGetList?offset=0&_=" + Cf_Web.currenttime();
            this._httpWeb.Accpet = "application/json, text/javascript, */*; q=0.01";
            this._httpWeb.Referer = "http://disk.yun.uc.cn/";

            var jsondata = this._httpWeb.CPostOrGet(linkurl, HttpMethod.GET).HtmlValue;
            jsondata = Regex.Unescape(jsondata);

            try
            {
                JObject jObject = JObject.Parse(jsondata);
                var data = jObject["uclxList"];
                return JsonConvert.DeserializeObject<TaskModel[]>(data.ToString());
            }
            catch
            {
                return null;
            }
        }
    }
}

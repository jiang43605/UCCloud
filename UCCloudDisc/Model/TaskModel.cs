using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UCCloudDisc
{
    /// <summary>
    /// UC任务列表数据模型
    /// </summary>
    public class TaskModel
    {
        private string _name;
        private long _size;
        private string _size_fmt;
        private string _type;
        private string _start_time;
        private string _task_id;
        private string _status;
        private string _store_type;
        private string _lx_open_url;
        private int _status_code;
        private double _percent;
        private string _downloadurl;
        private string _renewtaskurl;
        private string _fullname;
        private string _refer;
        private bool _isIrreg;
        private string _errorcode;
        private string _fileid;
        private bool _expired;
        private string _djangoID;
        private bool _show_save_to_nd;
        private bool _showOpenVideo;
        private string _task_status_class;
        private string _valid_info;
        private bool _failed;
        private string _openFileDetailUrl;
        private string _typename;
        private bool _success;
        private bool _rename;


        /// <summary>
        /// 资源的name属性
        /// </summary>
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }
        /// <summary>
        /// 资源的size属性
        /// </summary>
        public long Size
        {
            get { return this._size; }
            set { this._size = value; }
        }
        /// <summary>
        /// 资源的size_fmt属性
        /// </summary>
        public string Size_fmt
        {
            get { return this._size_fmt; }
            set { this._size_fmt = value; }
        }
        /// <summary>
        /// 资源的type属性
        /// </summary>
        public string Type
        {
            get { return this._type; }
            set { this._type = value; }
        }
        /// <summary>
        /// 资源的start_time属性
        /// </summary>
        public string Start_time
        {
            get { return this._start_time; }
            set { this._start_time = value; }
        }
        /// <summary>
        /// 资源的remain_time属性
        /// </summary>
        public string Remain_time { get; set; }

        /// <summary>
        /// 资源的task_id属性
        /// </summary>
        public string Task_id
        {
            get { return this._task_id; }
            set { this._task_id = value; }
        }
        /// <summary>
        /// 资源的status属性
        /// </summary>
        public string Status
        {
            get { return this._status; }
            set { this._status = value; }
        }
        /// <summary>
        /// 资源的store_type属性
        /// </summary>
        public string Store_type
        {
            get { return this._store_type; }
            set { this._store_type = value; }
        }
        /// <summary>
        /// 资源的lx_open_url属性
        /// </summary>
        public string Lx_open_url
        {
            get { return this._lx_open_url; }
            set { this._lx_open_url = value; }
        }
        /// <summary>
        /// 资源的status_code属性
        /// </summary>
        public int Status_code
        {
            get { return this._status_code; }
            set { this._status_code = value; }
        }
        /// <summary>
        /// 资源的percent属性
        /// </summary>
        public double Percent
        {
            get { return this._percent; }
            set { this._percent = value; }
        }
        /// <summary>
        /// 资源的downloadurl属性
        /// </summary>
        public string Downloadurl
        {
            get { return this._downloadurl; }
            set { this._downloadurl = value; }
        }
        /// <summary>
        /// 资源的renewtaskurl属性
        /// </summary>
        public string Renewtaskurl
        {
            get { return this._renewtaskurl; }
            set { this._renewtaskurl = value; }
        }
        /// <summary>
        /// 资源的fullname属性
        /// </summary>
        public string Fullname
        {
            get { return this._fullname; }
            set { this._fullname = value; }
        }
        /// <summary>
        /// 资源的refer属性
        /// </summary>
        public string Refer
        {
            get { return this._refer; }
            set { this._refer = value; }
        }
        /// <summary>
        /// 资源的isIrreg属性
        /// <summary>
        public bool IsIrreg
        {
            get { return this._isIrreg; }
            set { this._isIrreg = value; }
        }
        /// <summary>
        /// 资源的errorcode属性
        /// </summary>
        public string Errorcode
        {
            get { return this._errorcode; }
            set { this._errorcode = value; }
        }
        /// <summary>
        /// 资源的fileid属性
        /// <summary>
        public string Fileid
        {
            get { return this._fileid; }
            set { this._fileid = value; }
        }
        /// <summary>
        /// 资源的expired属性
        /// </summary>
        public bool Expired
        {
            get { return this._expired; }
            set { this._expired = value; }
        }
        /// <summary>
        /// 资源的djangoID属性
        /// </summary>
        public string DjangoID
        {
            get { return this._djangoID; }
            set { this._djangoID = value; }
        }
        /// <summary>
        /// 资源的show_save_to_nd属性
        /// </summary>
        public bool Show_save_to_nd
        {
            get { return this._show_save_to_nd; }
            set { this._show_save_to_nd = value; }
        }
        /// <summary>
        /// 资源的showOpenVideo属性
        /// </summary>
        public bool ShowOpenVideo
        {
            get { return this._showOpenVideo; }
            set { this._showOpenVideo = value; }
        }
        /// <summary>
        /// 资源的task_status_class属性
        /// <summary>
        public string Task_status_class
        {
            get { return this._task_status_class; }
            set { this._task_status_class = value; }
        }
        /// <summary>
        /// 资源的valid_info属性
        /// </summary>
        public string Valid_info
        {
            get { return this._valid_info; }
            set { this._valid_info = value; }
        }
        /// <summary>
        /// 资源的failed属性
        /// <summary>
        public bool Failed
        {
            get { return this._failed; }
            set { this._failed = value; }
        }
        /// <summary>
        /// 资源的openFileDetailUrl属性
        /// </summary>
        public string OpenFileDetailUrl
        {
            get { return this._openFileDetailUrl; }
            set { this._openFileDetailUrl = value; }
        }
        /// <summary>
        /// 资源的typename属性
        /// <summary>
        public string Typename
        {
            get { return this._typename; }
            set { this._typename = value; }
        }
        /// <summary>
        /// 资源的success属性
        /// </summary>
        public bool Success
        {
            get { return this._success; }
            set { this._success = value; }
        }
        /// <summary>
        /// 资源的rename属性
        /// </summary>
        public bool Rename
        {
            get { return this._rename; }
            set { this._rename = value; }
        }




    }
}

#region 模型自动生成代码
/*本模型由以下代码自动生成
 //string json = "{\"uclxList\":[{\"name\":\"dasdsafalse\",\"size\":728512512,\"size_fmt\":\"694.76M\",\"type\":\"magnet\",\"start_time\":\"2015-08-22 09:44:04\",\"remain_time\":3601,\"task_id\":\"7C0091D9E52A102C93B2C87B9D375AEA\",\"status\":\"\u8fc7\u671f\u65f6\u95f4\uff1a2015-08-29 09:45:34\",\"store_type\":5,\"lx_open_url\":\"http:\\/\\/disk.yun.uc.cn\\/netdisk\\/dirview#!2|all|list|normal\",\"status_code\":0,\"percent\":100,\"downloadurl\":false,\"renewtaskurl\":null,\"fullname\":\"05.\u300a\u7845\u8c37\u4f20\u5947\u300b\uff08Pirates of Silicon Valley\uff09\uff081999\uff09\",\"refer\":\"\",\"isIrreg\":false,\"errorcode\":0,\"fileid\":\"\",\"expired\":false,\"djangoID\":\"\",\"show_save_to_nd\":true,\"showOpenVideo\":false,\"task_status_class\":\"succ\",\"valid_info\":\"\u4fdd\u755916677\u5929\",\"failed\":false,\"openFileDetailUrl\":\"http:\\/\\/disk.yun.uc.cn\\/uclxmgr\\/btDetail?taskId=7C0091D9E52A102C93B2C87B9D375AEA\",\"typename\":\"\u5176\u4ed6\",\"success\":true,\"rename\":false},{\"name\":\"\u7845\u8c37\u4f20\u5947\",\"size\":1451580100,\"size_fmt\":\"1.35G\",\"type\":\"magnet\",\"start_time\":1440171579,\"remain_time\":3601,\"task_id\":\"E384D5CDF40BA8F845286652E388E0DE\",\"status\":\"\u4e0b\u8f7d\u5931\u8d25\",\"store_type\":5,\"lx_open_url\":null,\"status_code\":3,\"percent\":0,\"downloadurl\":false,\"renewtaskurl\":\"http:\\/\\/disk.yun.uc.cn\\/uclxmgr\\/lxrenewtask&taskid=E384D5CDF40BA8F845286652E388E0DE&storetype=5&size=1451580100&filename=%E7%A1%85%E8%B0%B7%E4%BC%A0%E5%A5%87\",\"fullname\":\"\u7845\u8c37\u4f20\u5947\",\"refer\":\"\",\"isIrreg\":false,\"errorcode\":10,\"task_status_class\":\"fail\",\"valid_info\":\"\u4fdd\u75590\u5c0f\u65f6\",\"failed\":true,\"typename\":\"\u5176\u4ed6\",\"success\":false,\"rename\":false},{\"name\":\"\u897f\u6e38\u8bb0\u4e4b\u5927\u5723\u5f52\u6765 [720HD-MTK-BD\u8d85\u9ad8\u6e05].wmv\",\"size\":1973891079,\"size_fmt\":\"1.84G\",\"type\":\"magnet\",\"start_time\":\"2015-08-13 12:48:09\",\"remain_time\":3601,\"task_id\":\"3E37F6C396FB4986CAE2FCC02425D31C\",\"status\":\"\u8fc7\u671f\u65f6\u95f4\uff1a2015-08-28 23:52:04\",\"store_type\":5,\"lx_open_url\":\"http:\\/\\/disk.yun.uc.cn\\/netdisk\\/dirview#!2|all|list|normal\",\"status_code\":0,\"percent\":100,\"downloadurl\":false,\"renewtaskurl\":null,\"fullname\":\"\u897f\u6e38\u8bb0\u4e4b\u5927\u5723\u5f52\u6765 [720HD-MTK-BD\u8d85\u9ad8\u6e05].wmv\",\"refer\":\"\",\"isIrreg\":false,\"errorcode\":0,\"fileid\":\"\",\"expired\":false,\"djangoID\":\"\",\"show_save_to_nd\":true,\"showOpenVideo\":false,\"task_status_class\":\"succ\",\"valid_info\":\"\u4fdd\u755916676\u5929\",\"failed\":false,\"openFileDetailUrl\":\"http:\\/\\/disk.yun.uc.cn\\/uclxmgr\\/btDetail?taskId=3E37F6C396FB4986CAE2FCC02425D31C\",\"typename\":\"\u5176\u4ed6\",\"success\":true,\"rename\":false},{\"name\":\"com.zdworks.android.toolbox.v1.4.77.apk\",\"size\":12114,\"size_fmt\":\"11.83K\",\"type\":\"apk\",\"start_time\":\"2011-11-28 15:14:29\",\"remain_time\":3601,\"task_id\":\"4E80546C31FF05D017D6253276BEEC4D\",\"status\":\"\u5df2\u5b58\u81f3\u7f51\u76d8\",\"store_type\":0,\"lx_open_url\":\"http:\\/\\/disk.yun.uc.cn\\/netdisk\\/dirview#!2|all|list|normal|2479017789747191809\",\"status_code\":0,\"percent\":100,\"downloadurl\":\"http:\\/\\/uc.dl.django.t.taobao.com\\/rest\\/1.0\\/file?token=48oGw-B_MNzT7ILLqoNBBgABUYAAAAFPU-pFjAAAABcAAQED&link-type=download-pc-low&fileIds=2ezxMO6-Thaui5dAKEOk1AAAABcAAQIC&timestamp=1440221939&uID=28782862&name=com.zdworks.android.toolbox.v1.4.77.apk&r=apk&imei=&fr=&acl=72e0c9571681cb44ce5c1429c52f8d4f\",\"renewtaskurl\":null,\"fullname\":\"com.zdworks.android.toolbox.v1.4.77.apk\",\"refer\":\"\",\"isIrreg\":false,\"errorcode\":0,\"fileid\":\"2479017789747191809\",\"expired\":false,\"djangoID\":\"2ezxMO6-Thaui5dAKEOk1AAAABcAAQIC\",\"show_save_to_nd\":true,\"showOpenVideo\":false,\"task_status_class\":\"succ\",\"valid_info\":\"\u4fdd\u755915314\u5929\",\"failed\":false,\"typename\":\"\u5b89\u88c5\u5305\",\"success\":true,\"rename\":true},{\"name\":\"\u6b63\u70b9\u5de5\u5177\u7bb1_2.0.112.apk\",\"size\":14275,\"size_fmt\":\"13.94K\",\"type\":\"apk\",\"start_time\":\"2011-11-28 15:10:19\",\"remain_time\":3601,\"task_id\":\"3A848635E70F150D98C94F0CCCF98627\",\"status\":\"\u5df2\u5b58\u81f3\u7f51\u76d8\",\"store_type\":1,\"lx_open_url\":\"http:\\/\\/disk.yun.uc.cn\\/netdisk\\/dirview#!2|all|list|normal|17007630187388090370\",\"status_code\":0,\"percent\":100,\"downloadurl\":\"http:\\/\\/uc.dl.django.t.taobao.com\\/rest\\/1.0\\/file?token=48oGw-B_MNzT7ILLqoNBBgABUYAAAAFPU-pFjAAAABcAAQED&link-type=download-pc-low&fileIds=8zdmfkG8SCWRyk2lh3S-XAAAABcAAQIC&timestamp=1440221939&uID=28782862&name=%E6%AD%A3%E7%82%B9%E5%B7%A5%E5%85%B7%E7%AE%B1_2.0.112.apk&r=apk&imei=&fr=&acl=6efe9825c6140962349e8ab86e5379d1\",\"renewtaskurl\":null,\"fullname\":\"\u6b63\u70b9\u5de5\u5177\u7bb1_2.0.112.apk\",\"refer\":\"\",\"isIrreg\":false,\"errorcode\":0,\"fileid\":\"17007630187388090370\",\"expired\":false,\"djangoID\":\"8zdmfkG8SCWRyk2lh3S-XAAAABcAAQIC\",\"show_save_to_nd\":true,\"showOpenVideo\":false,\"task_status_class\":\"succ\",\"valid_info\":\"\u4fdd\u755915314\u5929\",\"failed\":false,\"typename\":\"\u5b89\u88c5\u5305\",\"success\":true,\"rename\":true}],\"loadAll\":true,\"offset\":0}";
    //#endregion

            //json = HttpUtility.UrlDecode(json);
            //JObject jObject = JObject.Parse(json);
            //var date = jObject["uclxList"][0];
            //StringBuilder sb = new StringBuilder();
            //foreach (JToken token in date)
            //{
            //    JProperty jProperty = (JProperty)token;
            //    if (jProperty.Name != "downloadurl" && jProperty.Value.ToString() == "False" || jProperty.Value.ToString() == "True")
            //    {
            //        sb.AppendLine("private bool _" + jProperty.Name + ";");
            //    }
            //    else if (jProperty.Name == "size")
            //    {
            //        sb.AppendLine("private long _" + jProperty.Name + ";");
            //    }
            //    else if (jProperty.Name == "status_code")
            //    {
            //        sb.AppendLine("private int _" + jProperty.Name + ";");
            //    }
            //    else if (jProperty.Name == "percent")
            //    {
            //        sb.AppendLine("private double _" + jProperty.Name + ";");
            //    }
            //    else
            //        sb.AppendLine("private string _" + jProperty.Name + ";");
            //}
            //sb.AppendLine();
            //sb.AppendLine();
            //foreach (JToken token in date)
            //{
            //    JProperty jProperty = (JProperty)token;
            //    if (jProperty.Name != "downloadurl" && jProperty.Value.ToString() == "False" || jProperty.Value.ToString() == "True")
            //    {
            //        sb.AppendLine("/// <summary>");
            //        sb.AppendLine($"/// 资源的{jProperty.Name}属性");
            //        sb.AppendLine("/// <summary>");
            //        sb.AppendLine("public bool " + jProperty.Name.ToUpper().Substring(0, 1) + jProperty.Name.Remove(0, 1));
            //        sb.AppendLine("{");
            //        sb.AppendLine("get{return this._" + jProperty.Name + ";}");
            //        sb.AppendLine("set{this._" + jProperty.Name + "=value;}");
            //        sb.AppendLine("}");
            //    }
            //    else if (jProperty.Name == "size")
            //    {
            //        sb.AppendLine("/// <summary>");
            //        sb.AppendLine($"/// 资源的{jProperty.Name}属性");
            //        sb.AppendLine("/// <summary>");
            //        sb.AppendLine("public long " + jProperty.Name.ToUpper().Substring(0, 1) + jProperty.Name.Remove(0, 1));
            //        sb.AppendLine("{");
            //        sb.AppendLine("get{return this._" + jProperty.Name + ";}");
            //        sb.AppendLine("set{this._" + jProperty.Name + "=value;}");
            //        sb.AppendLine("}");
            //    }
            //    else if (jProperty.Name == "status_code")
            //    {
            //        sb.AppendLine("/// <summary>");
            //        sb.AppendLine($"/// 资源的{jProperty.Name}属性");
            //        sb.AppendLine("/// <summary>");
            //        sb.AppendLine("public int " + jProperty.Name.ToUpper().Substring(0, 1) + jProperty.Name.Remove(0, 1));
            //        sb.AppendLine("{");
            //        sb.AppendLine("get{return this._" + jProperty.Name + ";}");
            //        sb.AppendLine("set{this._" + jProperty.Name + "=value;}");
            //        sb.AppendLine("}");
            //    }
            //    else if (jProperty.Name == "percent")
            //    {
            //        sb.AppendLine("/// <summary>");
            //        sb.AppendLine($"/// 资源的{jProperty.Name}属性");
            //        sb.AppendLine("/// <summary>");
            //        sb.AppendLine("public double " + jProperty.Name.ToUpper().Substring(0, 1) + jProperty.Name.Remove(0, 1));
            //        sb.AppendLine("{");
            //        sb.AppendLine("get{return this._" + jProperty.Name + ";}");
            //        sb.AppendLine("set{this._" + jProperty.Name + "=value;}");
            //        sb.AppendLine("}");
            //    }
            //    else
            //    {
            //        sb.AppendLine("/// <summary>");
            //        sb.AppendLine($"/// 资源的{jProperty.Name}属性");
            //        sb.AppendLine("/// <summary>");
            //        sb.AppendLine("public string " + jProperty.Name.ToUpper().Substring(0, 1) + jProperty.Name.Remove(0, 1));
            //        sb.AppendLine("{");
            //        sb.AppendLine("get{return this._" + jProperty.Name + ";}");
            //        sb.AppendLine("set{this._" + jProperty.Name + "=value;}");
            //        sb.AppendLine("}");
            //    }
            //}
*/

#endregion

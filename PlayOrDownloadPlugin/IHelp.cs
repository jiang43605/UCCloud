using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Chengf;
using DisplayUserTaskInfoPlugin;

namespace PlayOrDownloadPlugin
{
    /// <summary>
    /// 内部帮助类
    /// </summary>
    internal static class IHelp
    {
        private static string _djangoidlink = "http://mydiskm.uc.cn/uclxmgr/btDetail?uc_param_str=frpfvesscplaprnisieint&fr=iphone&pf=44&ve=10.6.5.627&ss=320x568&cp=isp%3A%E8%81%94%E9%80%9A%3Bprov%3A%E6%B2%B3%E5%8C%97%3Bcity%3A%E7%9F%B3%E5%AE%B6%E5%BA%84%3Bna%3A%E4%B8%AD%E5%9B%BD%3Bcc%3ACN%3Bac%3A&la=zh-cn&pr=UCBrowser&ni=bTkwBQ41wdvmqWmo1fL6Mnw9s%2ByNelKXlBeXfbTnZCsSG9E%3D&si=bTkwBSo1xRxmMA%3D%3D&ei=bTkwBT81yF0%2FrlgQGvCWv4c%2Bu6MVZEyeDUCRmFjmKuPe0txVVB34t9EOXGLkOA%3D%3D&nt=2&taskId=";

        private static string _downloadlink = "http://mydiskm.uc.cn/module/getMovLink?uc_param_str=frpfvesscplaprnisieint&fr=android&pf=151&ve=10.6.0.620&ss=400x682&cp=isp:%E8%81%94%E9%80%9A;prov:%E6%B2%B3%E5%8C%97;city:%E7%9F%B3%E5%AE%B6%E5%BA%84;na:%E4%B8%AD%E5%9B%BD;cc:CN;ac:&la=zh-CN&pr=UCMobile&ni=bTkwBDINtoXJQhLSD64enLyOOZUjyt8Ef2bCbzdp1pZJVA%3D%3D&si=bTkwBBdOQ0LuiZ%2BVMenbvg%3D%3D&ei=bTkwBCE9VKzTnvMnKPH851gufsLZbRlRJQ%3D%3D&nt=2";
        internal static IEnumerable<DownloadModel[]> GetDownloadList(IEnumerable<TaskModel> taskModels)
        {
            // 初始化_djangoidlink和_downloadlink参数
            GetLinkInfo();

            List<DownloadModel[]> downloadModelses = new List<DownloadModel[]>();

            Cf_HttpWeb httpWeb = new Cf_HttpWeb();
            foreach (TaskModel taskModel in taskModels)
            {
                httpWeb.EncodingSet = "utf-8";
                httpWeb.HeaderSet = "X-UCBrowser-UA,dv(TianTian);pr(UCBrowser/10.6.0.620);ov(Android 4.3);ss(400*682);pi(600*1024);bt(UC);pm(1);bv(1);nm(0);im(0);sr(0);nt(2);";
                httpWeb.UserAgent = "Mozilla/5.0 (Linux; U; Android 4.3; zh-CN; TianTian Build/tt) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 UCBrowser/10.6.0.620 U3/0.8.0 Mobile Safari/534.30";
                httpWeb.Referer = "http://mydiskm.uc.cn/uclxmgr/index?uc_param_str=frpfvesscplaprnisieint&fromdirid=1";
                httpWeb.Accpet = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8,UC/151,alipay/un";
                var html = httpWeb.CPostOrGet(_djangoidlink + taskModel.Task_id, HttpMethod.GET).HtmlValue;
                downloadModelses.Add(GetDownloadModelForHtml(html, httpWeb, taskModel.Task_id));
            }
            return downloadModelses;
        }

        /// <summary>
        /// 获得一个资源下面的所有可播放格式的DownloadModel模型
        /// </summary>
        /// <param name="html"></param>
        /// <param name="httpWeb"></param>
        /// <param name="taskid"></param>
        /// <returns></returns>
        private static DownloadModel[] GetDownloadModelForHtml(string html, Cf_HttpWeb httpWeb, string taskid)
        {
            var data = html.LastExtractString("在线播放</a></li>", "<a onclick=");
            var djangoid = data.Select(o => o.ExtractStringNoQH("djangoid=\"", "\"").FirstOrDefault()).ToArray();
            var name = data.Select(o => o.ExtractStringNoQH("filename=\"", "\"").FirstOrDefault()).ToArray();

            DownloadModel[] downloadModels = new DownloadModel[djangoid.Length];

            // 注意此处要求djangoid和name的数量必须相等，这里不做判断了，默认相等，出异常再说
            for (int i = 0; i < djangoid.Length; i++)
            {
                var link = $"{_downloadlink}&djangoID={djangoid[i]}&filename={name[i]}&_={Cf_Web.currenttime()}";
                httpWeb.Referer =
                    "http://mydiskm.uc.cn/uclxmgr/btDetail?uc_param_str=frpfvesscplaprnisieint&taskId=" + taskid;
                httpWeb.Accpet = "application/json, text/javascript, */*";
                httpWeb.UserAgent = "Mozilla/5.0 (Linux; U; Android 4.3; zh-CN; TianTian Build/tt) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 UCBrowser/10.6.0.620 U3/0.8.0 Mobile Safari/534.30";
                httpWeb.EncodingSet = "utf-8";
                var jsondata = httpWeb.CPostOrGet(link, HttpMethod.GET).HtmlValue;
                jsondata = Regex.Unescape(jsondata);

                DownloadModel dm = new DownloadModel();
                dm.Link_low = jsondata.ExtractStringNoQH("low\":\"", "\"").FirstOrDefault();
                dm.Link_hi = jsondata.ExtractStringNoQH("hi\":\"", "\"").FirstOrDefault();
                dm.Name = name[i];
                downloadModels[i] = dm;
            }
            return downloadModels;

        }
        /// <summary>
        /// 获取_djangoidlink和_downloadlink的地址，缺省为本人手机值
        /// </summary>
        private static void GetLinkInfo()
        {
            string path = Path.GetFullPath("LinkInfo.txt");

            if (!File.Exists(path))
            {
                File.Create(path);
                return;
            }

            string linkstring = File.ReadAllText(path);
            if (string.IsNullOrEmpty(linkstring)) return;
            string[] link = linkstring.Split('|');
            if (link == null || link.Length <= 0) return;
            _djangoidlink = link[0];
            _downloadlink = link[1];
        }

        /// <summary>
        /// 检查是否是视频格式，是为true，否则为false，出现异常为false
        /// </summary>
        /// <returns></returns>
        internal static bool IsMovie(string moviestring)
        {
            string movieformat;
            try
            {
                movieformat = ConfigurationManager.AppSettings["MovieFormat"];
            }
            catch
            {
                return false;
            }
            if (movieformat == null) return false;

            string[] formatStrings = movieformat.Split('|');
            moviestring = moviestring.Reverse();
            int index = moviestring.IndexOf('.');
            if (index == -1) return false;

            string moviesuffix = moviestring.Substring(0, index + 1).Reverse();
            return formatStrings.FirstOrDefault(o => moviesuffix.ContainsIgnoreCase(o)) == null ? false : true;
        }

        private static string SetNameLast(this string str)
        {
            if (string.IsNullOrEmpty(str) || !str.Contains("name=")) return str;
            try
            {

                const string star = "name=";
                const string end = "&";
                var namelist = str.ExtractString(star, end);
                var name = namelist[0] == "name=&" ? namelist[1] : namelist[0];
                var newstring = str.DeleteSpecificString(star, end);

                name = name.Remove(name.Length - 1, 1);
                return newstring + "&" + name;
            }
            catch
            {
                return str;
            }
        }
    }
}
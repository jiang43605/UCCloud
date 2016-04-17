using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Chengf;
using UCCloudDisc.Model;

namespace UCCloudDisc
{
    /// <summary>
    /// UC云盘登录
    /// </summary>
    public class UCLogin
    {
        private Cf_HttpWeb _httpWeb = new Cf_HttpWeb();
        private string _captchaid;
        private string _loginurl;
        private string _username;
        private string _password;
        private LoginStatus _status;
        private string _loginmsg;
        private LoginMsgModel _loginResultMsg = new LoginMsgModel();

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

        /// <summary>
        /// 调用Login后的状态查询
        /// </summary>
        public LoginStatus Status
        {
            get
            {
                return this._status;
            }

            private set
            {
                this._status = value;
            }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username
        {
            get
            {
                return this._username;
            }

            set
            {
                this._username = value;
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get
            {
                return this._password;
            }

            set
            {
                this._password = value;
            }
        }

        /// <summary>
        /// 当成功登录时返回的消息模型
        /// </summary>
        public LoginMsgModel LoginResultMsg
        {
            get
            {
                return this._loginResultMsg;
            }

            set
            {
                this._loginResultMsg = value;
            }
        }

        /// <summary>
        /// 实时返回最新的登录状态，true表示已经登录，false表示未登录
        /// </summary>
        public bool IsLogin
        {
            get
            {
                this._httpWeb.EncodingSet = "utf-8";
                this._httpWeb.AllowAutoRedirect = true;
                var mianhtml = this._httpWeb.CPostOrGet("http://disk.yun.uc.cn/", HttpMethod.GET).HtmlValue;

                if (!mianhtml.Contains("离线任务")) return false;
                this._loginResultMsg.MainHtml = mianhtml;
                return true;
            }
        }

        public UCLogin()
        {

        }

        public UCLogin(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
        /// <summary>
        /// 登录
        /// <param name="captcha"></param>
        /// </summary>
        public virtual LoginStatus Login(string captcha)
        {
            // 参数准备
            this._httpWeb.UserDate =
                $"login_name={this.Username}&password={this.Password}&captcha_code={captcha}&captcha_id={this._captchaid}";
            this._httpWeb.Referer =
                "http://yun.uc.cn/cloud/login?redirecturl=http%3A%2F%2Fdisk.yun.uc.cn%2F%3Fchannel_id%3D103";
            this._httpWeb.HeaderSet = "Origin,http://yun.uc.cn";

            // 开始访问并填充相应参数
            string loginmsg = this._httpWeb.CPostOrGet(this._loginurl, HttpMethod.POST).HtmlValue;
            loginmsg = HttpUtility.UrlDecode(loginmsg);
            string service_ticket = loginmsg.ExtractStringNoQH("service_ticket\":\"", "\"").FirstOrDefault();
            this._status = UCHelp.MsgToStatus(loginmsg.ExtractStringNoQH("message\":\"", "\"").FirstOrDefault());

            // 登录成功
            if (this._status == LoginStatus.Success)
            {
                this._httpWeb.Referer =
                    "http://yun.uc.cn/cloud/login?redirecturl=http%3A%2F%2Fdisk.yun.uc.cn%2F%3Fchannel_id%3D103";
                var msg = this._httpWeb.CPostOrGet("http://yun.uc.cn/exter/basicinfo?service_ticket=" + service_ticket,
                    HttpMethod.GET);

                this._httpWeb.Referer =
                    "http://yun.uc.cn/cloud/login?redirecturl=http%3A%2F%2Fdisk.yun.uc.cn%2F%3Fchannel_id%3D103";
                this._httpWeb.UserDate = "ln=" + this.Username;
                this._httpWeb.CPostOrGet("http://yun.uc.cn/index.php/netdisk_service/ajax/setLn", HttpMethod.POST);

                this._httpWeb.Referer =
                    "http://yun.uc.cn/cloud/login?redirecturl=http%3A%2F%2Fdisk.yun.uc.cn%2F%3Fchannel_id%3D103";
                var mianhtml = this._httpWeb.CPostOrGet("http://disk.yun.uc.cn/", HttpMethod.GET).HtmlValue;
                this._loginResultMsg.MainHtml = mianhtml;

                // 设置登录指示器
                return this._status;
            }

            // 验证码或者帐号密码错误时
            this._captchaid = loginmsg.ExtractStringNoQH("captcha_id\":\"", "\"").FirstOrDefault();
            this._loginmsg = loginmsg;
            return this._status;
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        public virtual void LogOut()
        {
            this._httpWeb.HttpCookieContainer = new CookieContainer();
        }
        /// <summary>
        /// 获取验证码
        /// </summary>
        public virtual byte[] GetCaptchaBytes()
        {
            const string link_1 = "http://yun.uc.cn/cloud/login";
            string captchaid_2 = "https://api.open.uc.cn/cas/commonjson/needCaptcha?callback=callback&_=" + Cf_Web.currenttime();
            const string captcha_3 = "https://api.open.uc.cn/cas/commonjson/captcha?captchaId=";

            // 1、访问主页并且得到_loginurl
            this._httpWeb.AllowAutoRedirect = true;
            this._httpWeb.EncodingSet = "utf-8";
            const string starstring = "<form id=\"loginForm\" target=\"loginiframe\" action=\"";
            string tempstring = this._httpWeb.CPostOrGet(link_1, HttpMethod.GET).HtmlValue;
            this._loginurl=tempstring.ExtractStringNoQH(starstring, "\"").FirstOrDefault();

            // 2、获取其他captchaid
            this._httpWeb.Referer = "http://yun.uc.cn/cloud/login";
            this._captchaid = this._httpWeb.CPostOrGet(captchaid_2, HttpMethod.GET).HtmlValue.ExtractStringNoQH("'captchaId':'", "'").FirstOrDefault();

            // 3、获取captchaBytes
            this._httpWeb.Referer = "http://yun.uc.cn/cloud/login";
            byte[] codeBytes = this._httpWeb.CPostOrGet(captcha_3 + this._captchaid, HttpMethod.GET).Bytes;

            return codeBytes;
        }

        /// <summary>
        /// 刷新验证码，请在调用GetCaptchaBytes()方法之后有必要时再使用
        /// </summary>
        /// <returns></returns>
        public virtual byte[] RefreshCaptcha()
        {
            const string captcha_3 = "https://api.open.uc.cn/cas/commonjson/captcha?captchaId=";

            string url = "https://api.open.uc.cn/cas/commonjson/refreshCaptchaByIframe?captchaId=&callback=callback&_=" + Cf_Web.currenttime();
            this._httpWeb.Referer = "http://yun.uc.cn/cloud/login";
            this._captchaid = this._httpWeb.CPostOrGet(url, HttpMethod.GET).HtmlValue.ExtractStringNoQH("'captchaId':'", "'").FirstOrDefault();

            this._httpWeb.Referer = "http://yun.uc.cn/cloud/login";
            byte[] codeBytes = this._httpWeb.CPostOrGet(captcha_3 + this._captchaid, HttpMethod.GET).Bytes;

            return codeBytes;
        }

        /// <summary>
        /// 输入的帐号密码或者验证码错误时请调用这个
        /// </summary>
        /// <returns></returns>
        public virtual byte[] ErrorCaptcha()
        {
            const string captcha_3 = "https://api.open.uc.cn/cas/commonjson/captcha?captchaId=";
            string url =
                this._loginmsg?.ExtractStringNoQH("document.location.href=\"", ";")
                    .FirstOrDefault()
                    .LastStringRemove(0, 1);

            // 获取数据时先访问这个
            this._httpWeb.CPostOrGet(url, HttpMethod.GET);

            this._httpWeb.Referer = "http://yun.uc.cn/cloud/login";
            byte[] codeBytes = this._httpWeb.CPostOrGet(captcha_3 + this._captchaid, HttpMethod.GET).Bytes;

            return codeBytes;
        }

    }
}

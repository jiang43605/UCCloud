using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CefSharp;
using CefCookie = CefSharp.Cookie;
using Cookie = System.Net.Cookie;

namespace UC.Windows
{
    /// <summary>
    /// 适合uc下的cookie获取器
    /// </summary>
    public class UCCookieVisitor : ICookieVisitor
    {
        //private readonly List<Cookie> _uCCookieContainer = new List<Cookie>();
        private Action<Cookie> _actionCookie;
        ///// <summary>
        ///// 获取到的cookie
        ///// </summary>
        //public List<Cookie> UCCookieContainer
        //{
        //    get { return this._uCCookieContainer; }
        //}

        public UCCookieVisitor(Action<Cookie> cookieAction)
        {
            this._actionCookie = cookieAction;
        }
        public bool Visit(CefCookie cookie, int count, int total, ref bool deleteCookie)
        {
            this._actionCookie(this.tranCefCookieToCookie(cookie));
            return true;
        }

        /// <summary>
        /// 将cefcookie转换到cookie
        /// </summary>
        /// <param name="cefcookie"></param>
        /// <returns></returns>
        private Cookie tranCefCookieToCookie(CefCookie cefcookie)
        {
            Cookie cookie = new Cookie()
            {
                Value = cefcookie.Value,
                Domain = cefcookie.Domain,
                Expires = cefcookie.Expires.GetValueOrDefault(),
                Name = cefcookie.Name,
                Path = cefcookie.Path,
            };
            return cookie;
        }
    }
}

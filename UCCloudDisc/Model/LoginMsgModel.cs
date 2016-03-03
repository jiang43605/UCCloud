using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCCloudDisc.Model
{
    /// <summary>
    /// 成功登录后的一些回执信息
    /// </summary>
    public class LoginMsgModel
    {
        private string _mainHtml;

        /// <summary>
        /// 返回的主页代码
        /// </summary>
        public string MainHtml
        {
            get
            {
                return this._mainHtml;
            }

            set
            {
                this._mainHtml = value;
            }
        }

        /// <summary>
        /// 用于添加新任务post的时候
        /// </summary>
        public string Token
        {
            get
            {
                return this._mainHtml.ExtractStringNoQH("token\" value=\"", "\"").FirstOrDefault();
            }
        }

        /// <summary>
        /// 登录后的用户昵称
        /// </summary>
        public string Name
        {
            get
            {
                return this._mainHtml.ExtractStringNoQH("class=\"mName\">", "<").FirstOrDefault();
            }
        }

    }
}

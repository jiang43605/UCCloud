using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCCloudDisc
{
    /// <summary>
    /// 登录状态码
    /// </summary>
    public enum LoginStatus
    {
        /// <summary>
        /// 验证码错误，需要从新获取验证码
        /// </summary>
        InvalidCaptcha,
        /// <summary>
        /// 帐号或者密码错误，需要重新获取验证码
        /// </summary>
        InvalidLoginnameOrPassword,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 未查明的错误
        /// </summary>
        Error
    }
}

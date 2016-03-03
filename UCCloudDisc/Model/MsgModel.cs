using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UCCloudDisc.Model
{
    /// <summary>
    /// 任务删除返回状态模型
    /// </summary>
    public class MsgModel
    {
        private string _msg;
        /// <summary>
        /// 新建任务回执状态
        /// </summary>
        public int Status { set; get; }

        /// <summary>
        /// 删除任务回执状态
        /// </summary>
        public int StatusCode { set; get; }

        /// <summary>
        /// 回执消息
        /// </summary>
        public string Msg
        {
            set
            {
                value = Regex.Escape(value);
                this._msg = value;
            }
            get { return this._msg; }
        }
    }
}

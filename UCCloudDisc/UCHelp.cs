using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace UCCloudDisc
{
    public static class UCHelp
    {
        /// <summary>
        /// 将string信息转换为Enum
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static LoginStatus MsgToStatus(string msg)
        {
            switch (msg)
            {
                case "Success":
                    return LoginStatus.Success;
                case "Invalid captcha":
                    return LoginStatus.InvalidCaptcha;
                case "Invalid login name or password":
                    return LoginStatus.InvalidLoginnameOrPassword;
                default:
                    return LoginStatus.Error;
            }
        }

        /// <summary>
        /// 序列化cookiecontaniner
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cookieContainer"></param>
        public static void BinarySerializeCookieContainer(string path, CookieContainer cookieContainer)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, cookieContainer);
            }
        }

        /// <summary>
        /// 反序列化成cookiecontaniner
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static void BinaryDeserializeCookieContainer(string path, ref UCLogin ucLogin)
        {
            if (!File.Exists(path)) return;

            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                ucLogin.HttpWeb.HttpCookieContainer = binaryFormatter.Deserialize(fileStream) as CookieContainer;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
// using Chengf;
// using Newtonsoft.Json.Linq;
// using UCCloudDisc;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine(SetNameLast("/rest/1.0/file?timestamp=1451927499&token=E0swHaesyh_TNCCFeW-IigABUYAAAAFSDZFZwAAAABcAAQED&imei=&fileIds=4VatKm11TFmIX58HQrLuHgAAAAsAAQED&name=&format=mp4_low&acl=bb909a08fdbd808f1fec9d6e586c6c19&link-type=play&sc=0&name=老炮儿HDTC1280标清国语中英双字.mp4&uID=28782862&fr=iphone"));
            // Console.ReadKey();
			while(true)
			{
				try{
				Console.WriteLine("请输入一组数据，以空格为分界符");
				var str = Console.ReadLine();
				var arys = str.Split(' ').Select(o => Convert.ToInt32(o)).ToArray();
				Console.WriteLine("计算的结果如下所示：");
				var t = GetMaxChilrenArry(arys,0);
				Console.WriteLine(t[0]);
				Console.WriteLine(t[1]);
				Console.WriteLine(t[2]);
				}
				catch{
					Console.WriteLine("输入出现错误，请重新尝试");
				}
			}
        }

		//获得最大子字符串
		static int[] GetMaxChilrenArry(int[] arys , int star)
		{
			int head = star, end = star;
			int tempint = arys[star], maxitem = arys[star];
			for(int i = star + 1; i < arys.Length; i++)
			{
				tempint = tempint + arys[i];
				if(maxitem < tempint)
				{
					maxitem = tempint;
					end = i;
				}
			}
			
			if(++star > arys.Length - 1) return new int[]{maxitem,head,end};
			
			var ints = GetMaxChilrenArry(arys,star);
			if(ints[0] > maxitem) return ints;
			return new int[]{maxitem,head,end};
		}
        // static void SetHead(Cf_HttpWeb httpweb)
        // {
            // httpweb.EncodingSet = "utf-8";
            // httpweb.Accpet = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8,UC/44,ios_plugin/1";
            // httpweb.HeaderAdd.Add("Accept-Language", "zh-CN");
            // httpweb.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 9_0 like Mac OS X; zh-CN) AppleWebKit/537.51.1 (KHTML, like Gecko) Mobile/13A4325c UCBrowser/10.6.5.627 Mobile";
            // httpweb.HeaderAdd.Add("X-UCBrowser-UA",
                // "dv(iPh6,1);pr(UCBrowser/10.6.5.627);ov(9_0);ss(320x568);bt(UC);pm(0);bv(0);nm(0);im(0);nt(2);");
        // }


        // private static string SetNameLast(string str)
        // {
            // // if (string.IsNullOrEmpty(str) || !str.Contains("name=")) return str;

            // // const string star = "name=";
            // // const string end = "&";
            // // var name = str.ExtractString(star, end).FirstOrDefault();
            // // var newstring = str.DeleteSpecificString(star, end);

            // // name = name.Remove(name.Length - 1, 1);
            // // return newstring + "&" + name;
        // }
    }


}

using MCF.Classes.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchFIFASales
{
    static class Program
    {
        static int programRevision = 5;
        private static CookieContainer _cookie;
        public static CookieContainer Cookie
        {
            get
            {
                return _cookie;
            }
            private set
            {
            }
        }
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if !DEBUG
            string sql = string.Format(@"SELECT UPDATE_INDEX FROM C_VERSION ORDER BY UPDATE_INDEX DESC LIMIT 1;");
            int revision = Convert.ToInt16(MySqlHelper.ExecuteDataTable(sql).Rows[0][0].ToString());

            if (revision > programRevision)
            {
                MessageBox.Show("새로운 업데이트가 있습니다. \nUpdate.exe를 실행해주세요.", "업데이트 알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
#else
#endif
            Application.Run(new FormMain());
        }

        static string GetPage(string url)
        {
            string readUrl = url;
            string result = "";

            CookieCollection Cookies = new CookieCollection();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(readUrl);
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.93 Safari/537.36";
            request.Host = url.Split(new string[] { "://" }, StringSplitOptions.None)[1].Split('/')[0].ToString(); ;
            //request.Headers.Add("Origin", mainUrl);
            request.Referer = url;
            request.Method = "GET";


            request.Accept = "application/json, text/javascript, */*; q=0.01";

            request.KeepAlive = true;

            request.ContentType = @"application/x-www-form-urlencoded";
            request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            request.Headers.Add("Accept-Language", "ko-KR,ko;q=0.8,en-US;q=0.6,en;q=0.4");


            request.CookieContainer = Cookie;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;


            //string sendData = "mode=SA&dividend=" + devidend + "&bet_money=" + Price + "&win_money=" + Convert.ToInt32(Convert.ToInt32(Price) * Convert.ToDouble(devidend)) + "&betting_length=1&bet_money_tmp=" + Price + "&game_idx_0=" + GameID + "&game_select_0=" + Bet + "&home_rate_0=" + devidend + "&tie_rate_0=0&away_rate_0=" + devidend + "&jongmok_idx_0=20&league_idx=0";

            //byte[] buffer = Encoding.Default.GetBytes(sendData);

            //request.ContentLength = buffer.Length;

            //Stream sendStream = request.GetRequestStream();

            //sendStream.Write(buffer, 0, buffer.Length);

            //sendStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);

            ////response.Headers.Add("Content-Encoding", "gzip");
            ////response.Headers.Add("CF-RAY", "2272c173eb191279-ICN");
            ////response.Headers.Add("Transfer-Encoding", "chunked");
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            result = reader.ReadToEnd();
            stream.Close();
            response.Close();

            if (result.Contains("ErrorMessage"))
            {
                GetPage(url);
            }
            return result;
        }
    }
}

using MCF.Classes.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace SearchFIFASales
{
    static class Program
    {
        static int programRevision = 14;
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
    }
}

using MCF.Classes.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Windows.Forms;

namespace SearchFIFASales
{
    static class Program
    {
        static int programRevision = 16;
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
            Application.ThreadException += Application_ThreadException;
#if !DEBUG
            string sql = string.Format(@"SELECT UPDATE_INDEX FROM C_VERSION ORDER BY UPDATE_INDEX DESC LIMIT 1;");
            int revision = Convert.ToInt16(MySqlHelper.ExecuteDataTable(sql).Rows[0][0].ToString());

            if (revision > programRevision)
            {
                MessageBox.Show("새로운 업데이트가 있습니다. \nUpdate.exe를 꼭 관리자권한으로 실행해주세요.", "업데이트 알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
#else
#endif
            Application.Run(new FormMain());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try
            {
                Form f = new Form();

                if (!System.IO.Directory.Exists(Environment.CurrentDirectory + "\\ERROR_LOG"))
                    System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + "\\ERROR_LOG");

                string exceptionMessage = e.Exception.StackTrace.Replace("\r\n", "^");
                string[] errorMessage = exceptionMessage.Split('^');
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Environment.CurrentDirectory+ "\\ERROR_LOG\\ERROR_LOG_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt"))
                {
                    file.WriteLine("   Error Message: " + e.Exception.ToString() + "\r\n");
                    foreach (string line in errorMessage)
                    {
                        file.WriteLine(line);
                    }
                    file.WriteLine("\r\n\r\n");
                    file.WriteLine(Environment.OSVersion.VersionString);

                    int size = IntPtr.Size;
                    if (size == 4)
                        file.WriteLine("32bit");
                    else
                        file.WriteLine("64bit");

                    file.WriteLine();

                    ManagementClass ramcls = new ManagementClass("Win32_OperatingSystem");
                    ManagementObjectCollection ranInstances = ramcls.GetInstances();

                    foreach (ManagementObject info in ranInstances)
                    {
                        file.WriteLine("Memory Information ================================");
                        file.WriteLine("Total Physical Memory : {0:#,###} KB", info["TotalVisibleMemorySize"]);
                        file.WriteLine("Total Page File Size : {0:#,###} KB", info["SizeStoredInPagingFiles"]);
                        file.WriteLine("Total Virtual Memory : {0:#,###} KB", info["TotalVirtualMemorySize"]);
                        file.WriteLine("Free Physical Memory : {0:#,###} MB", info["FreePhysicalMemory"]);
                        file.WriteLine("Free Virtual Memory : {0:#,###} MB", info["FreeVirtualMemory"]);
                        file.WriteLine("Free Space in Page File : {0:#,###} KB", info["FreeSpaceInPagingFiles"]);
                    }

                    ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_processor");
                    ManagementObjectCollection queryCollection1 = query.Get();
                    foreach (ManagementObject mo in queryCollection1)
                    {
                        file.WriteLine("AddressWidth:" + Convert.ToString(mo["AddressWidth"]));
                        file.WriteLine("Architecture:" + Convert.ToString(mo["Architecture"]));
                        file.WriteLine("Availability:" + Convert.ToString(mo["Availability"]));
                        file.WriteLine("Caption:" + Convert.ToString(mo["Caption"]));
                        file.WriteLine("ConfigManagerErrorCode:" + Convert.ToString(mo["ConfigManagerErrorCode"]));
                        file.WriteLine("ConfigManagerUserConfig:" + Convert.ToString(mo["ConfigManagerUserConfig"]));
                        file.WriteLine("CpuStatus:" + Convert.ToString(mo["CpuStatus"]));
                        file.WriteLine("CreationClassName:" + Convert.ToString(mo["CreationClassName"]));
                        file.WriteLine("CurrentClockSpeed:" + Convert.ToString(mo["CurrentClockSpeed"]));
                        file.WriteLine("CurrentVoltage:" + Convert.ToString(mo["CurrentVoltage"]));
                        file.WriteLine("DataWidth:" + Convert.ToString(mo["DataWidth"]));
                        file.WriteLine("Description:" + Convert.ToString(mo["Description"]));
                        file.WriteLine("DeviceID:" + Convert.ToString(mo["DeviceID"]));
                        file.WriteLine("ErrorCleared:" + Convert.ToString(mo["ErrorCleared"]));
                        file.WriteLine("ErrorDescription:" + Convert.ToString(mo["ErrorDescription"]));
                        file.WriteLine("ExtClock:" + Convert.ToString(mo["ExtClock"]));
                        file.WriteLine("Family:" + Convert.ToString(mo["Family"]));
                        file.WriteLine("InstallDate:" + Convert.ToString(mo["InstallDate"]));
                        file.WriteLine("L2CacheSize:" + Convert.ToString(mo["L2CacheSize"]));
                        file.WriteLine("L2CacheSpeed:" + Convert.ToString(mo["L2CacheSpeed"]));
                        file.WriteLine("L2CacheSpeed:" + Convert.ToString(mo["L2CacheSpeed"]));
                        file.WriteLine("LastErrorCode:" + Convert.ToString(mo["LastErrorCode"]));
                        file.WriteLine("Level:" + Convert.ToString(mo["Level"]));
                        file.WriteLine("LoadPercentage:" + Convert.ToString(mo["LoadPercentage"]));
                        file.WriteLine("Manufacturer:" + Convert.ToString(mo["Manufacturer"]));
                        file.WriteLine("MaxClockSpeed:" + Convert.ToString(mo["MaxClockSpeed"]));
                        file.WriteLine("Name:" + Convert.ToString(mo["Name"]));
                        file.WriteLine("OtherFamilyDescription:" + Convert.ToString(mo["OtherFamilyDescription"]));
                        file.WriteLine("PNPDeviceID:" + Convert.ToString(mo["PNPDeviceID"]));
                        file.WriteLine("PowerManagementCapabilities:" + Convert.ToString(mo["PowerManagementCapabilities"]));
                        file.WriteLine("PowerManagementSupported:" + Convert.ToString(mo["PowerManagementSupported"]));
                        file.WriteLine("ProcessorId:" + Convert.ToString(mo["ProcessorId"]));
                        file.WriteLine("ProcessorType:" + Convert.ToString(mo["ProcessorType"]));
                        file.WriteLine("Revision:" + Convert.ToString(mo["Revision"]));
                        file.WriteLine("SocketDesignation:" + Convert.ToString(mo["SocketDesignation"]));
                        file.WriteLine("Status:" + Convert.ToString(mo["Status"]));
                        file.WriteLine("StatusInfo:" + Convert.ToString(mo["StatusInfo"]));
                        file.WriteLine("Stepping:" + Convert.ToString(mo["Stepping"]));
                        file.WriteLine("SystemCreationClassName:" + Convert.ToString(mo["SystemCreationClassName"]));
                        file.WriteLine("SystemName:" + Convert.ToString(mo["SystemName"]));
                        file.WriteLine("UniqueId:" + Convert.ToString(mo["UniqueId"]));
                        file.WriteLine("UpgradeMethod:" + Convert.ToString(mo["UpgradeMethod"]));
                        file.WriteLine("Version:" + Convert.ToString(mo["Version"]));
                        file.WriteLine("VoltageCaps:" + Convert.ToString(mo["VoltageCaps"]));
                        file.Close();
                    }
                }
            }
            finally
            {
                //MessageBox.Show(string.Format("오류가 발생하였습니다. 오류 메시지는 아래와 같습니다.\n\n{0}", e.Exception.Message), "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Thread Exception : {0}", e.Exception.Message);
            }
        }
    }
}

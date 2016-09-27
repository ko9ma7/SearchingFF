using MCF.Classes.Data;
using Module;
using SearchFIFASales.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Imaging;
using Module.Handling;

namespace SearchFIFASales
{
    public partial class FormMain : Form
    {
        #region 멤버, 변수
        string mainUrl = "http://fifaonline3.nexon.com";
        string dataCenterUrl = "/datacenter/player/list.aspx?";
        string detailUrl = "/datacenter/player/view.aspx?id=";
        string leagueUrl = "&n4legid=";
        string priceFirstUrl = "&n8pg1=";
        string priceSecondUrl = "&n8pg2=";
        string seasonUrl = "&n1sesid=";
        string pageNoUrl = "&n4pageno=";
        Dictionary<string, string> dictLeague = new Dictionary<string, string>();
        Dictionary<string, DateTime> dictAuth = new Dictionary<string, DateTime>();
        Dictionary<string, string> dictSeason = new Dictionary<string, string>();
        List<PlayerInfo> lstPlayers = new List<PlayerInfo>();
        string mainContent = "";
        DataTable authDT = new DataTable();

        Dictionary<string, Module.Handling.Imaging.ImageRange> dictRange = new Dictionary<string, Imaging.ImageRange>();


        private CookieContainer _cookie;
        public CookieContainer Cookie
        {
            get
            {
                return this._cookie;
            }
            private set
            {
            }
        }

        private string _uID;
        private string _uIDW;
        private bool _includeWindows = false;
        public string SimpleUID
        {
            get { return _uID; }
        }
        public string AdvancedUID
        {
            get { return _uIDW; }
        }
        #endregion


        #region 초기화
        public FormMain()
        {
            InitializeComponent();
            this.Load += FormMain_Load;
        }

        void FormMain_Load(object sender, EventArgs e)
        {
            //Process[] p = Process.GetProcessesByName("notepad");
            //PrintWindow(p[0].Handle);
            this.WindowState = FormWindowState.Maximized;
            string sql = string.Format(@"SELECT * FROM C_USER LIMIT 1");
            DataTable dt = MySqlHelper.ExecuteDataTable(sql);
            InitMacro();
            Init();
            
            //MainProcess();
        }

        void Init()
        {
            GetUniqueID();
            lbAuth.Text = "";

            string sql = string.Format(@"SELECT * FROM C_USER_AUTH WHERE C_USER_HWID = '{0}'", SimpleUID);
            authDT = MySqlHelper.ExecuteDataTable(sql);
            foreach (DataRow dr in authDT.Rows)
            {
                dictAuth.Add(dr["C_USER_HWID"].ToString(), Convert.ToDateTime(dr["EXPIRE_DATE"].ToString()));
            }

            DateTime serverTime = GetServerTime();
            if (!dictAuth.ContainsKey(SimpleUID) || dictAuth[SimpleUID] < serverTime)
            {
                btnSearch.Enabled = false;
                lbAuth.Text = "사용인증이 필요합니다. \n\n관리자에게 문의하세요. \n\n카카오톡 : searchfifa \n\n(HWID : " + SimpleUID + ")";
                lbAuth.ForeColor = Color.Red;
                lbAuth.Font = new Font("돋움", 9, FontStyle.Bold);
            }

            _cookie = new CookieContainer();
            dictLeague.Add("잉글랜드 프리미어리그", "13");
            dictLeague.Add("독일 분데스리가 1", "19");
            dictLeague.Add("스페인 리가 BBVA", "53");
            dictLeague.Add("이탈리아 세리에 A", "31");
            dictLeague.Add("프랑스 리게 1", "16");


            dictSeason.Add("\'15시즌", "12");
            dictSeason.Add("\'14시즌", "14");
            dictSeason.Add("\'11시즌", "14");
            dictSeason.Add("\'10시즌", "10");
            dictSeason.Add("\'09시즌", "9");
            dictSeason.Add("\'08시즌", "8");
            dictSeason.Add("\'07시즌", "7");
            dictSeason.Add("\'06시즌", "6");
            dictSeason.Add("\'08유럽대륙", "67");
            dictSeason.Add("\'06유럽클럽", "63");
            dictSeason.Add("\'10유럽클럽", "69");
            dictSeason.Add("월드베스트", "91");
            dictSeason.Add("한국전설", "97");
            dictSeason.Add("2002전설", "95");
            dictSeason.Add("U-23", "29");
            dictSeason.Add("\'14WC", "77");
            dictSeason.Add("\'06WC", "65");
            dictSeason.Add("\'10WC", "47");
            dictSeason.Add("\'16EC", "35");
            dictSeason.Add("월드전설", "93");
            dictSeason.Add("유럽리그전설", "27");
            dictSeason.Add("SPECIAL", "57");
            dictSeason.Add("\'14T", "53");
            dictSeason.Add("맨유엠버서더", "55");
            dictSeason.Add("중국리그", "85");

            foreach (string item in dictLeague.Keys)
            {
                cboLeague.Items.Add(item);
            }


            cboSeason.Items.Add("전체");
            foreach (string item in dictSeason.Keys)
            {
                cboSeason.Items.Add(item);
            }

            cboLeague.SelectedIndex = 0;
            cboSeason.SelectedIndex = 0;
            cboCompareCard.SelectedIndex = 0;
            cboCard.SelectedIndex = 0;

            #region 이벤트
            btnSearch.Click += (object sender, EventArgs e) =>
            {
                lstPlayers.Clear();
                new Thread(() => MainProcess()).Start();
            };

            btnCapture.Click += (object sender, EventArgs e) =>
            {
                Imaging.GetScreen().Save(Environment.CurrentDirectory + "\\test.png");
                //Imaging.CropImage(Imaging.GetScreen(), new Point(400, 550), 150, 150).Save("C:\\test.png");
                //MessageCtr.SendKey(new Point(112, 191));
            };

            btnFind.Click += (object sender, EventArgs e) =>
            {
                //OpenFileDialog dialog = new OpenFileDialog();
                //DialogResult result = dialog.ShowDialog();

                //if (result == System.Windows.Forms.DialogResult.OK)
                //{

                    //string path = dialog.FileName;
                    //Bitmap small = (Bitmap)Bitmap.FromFile(path);
                    //Bitmap big = (Bitmap)Imaging.GetScreen();

                    //Module.Handling.Imaging.ImageRange range = dictRange["이적시장_즐겨찾기"];
                    
                    
                    //MessageBox.Show(p.ToString());
                    new Thread(() => TradeMacro()).Start();
                //}

            };
            #endregion
        }

        void InitMacro()
        {
            Module.Handling.Imaging.ImageRange range = new Imaging.ImageRange(400, 525);
            dictRange.Add("이적시장_버튼", range);

            range = new Imaging.ImageRange(211 - 50, 125 - 50);
            dictRange.Add("이적시장_즐겨찾기_버튼", range);


        }
        #endregion

        #region 함수

        #region 데이터센터 파싱
        void MainProcess()
        {
            this.Invoke(new MethodInvoker(delegate()
            {
                string where = "n1o1=70";
                where += leagueUrl + dictLeague[cboLeague.Text];
                where += txtFirstPrice.Text != "" ? priceFirstUrl + txtFirstPrice.Text : "";
                where += txtSecondPrice.Text != "" ? priceSecondUrl + txtSecondPrice.Text : "";
                where += cboSeason.Text != "전체" ? seasonUrl + dictSeason[cboSeason.Text] : "";
                mainContent = GetPage(mainUrl + dataCenterUrl + where);

                string[] tempLastPageArr = mainContent.Split(new string[] { "마지막 페이지" }, StringSplitOptions.None)[0].Split(new string[] { "<a href=\'" }, StringSplitOptions.None);
                int lastPageNo = 0;

                if (tempLastPageArr.Length < 15)
                {
                    try
                    {
                        lastPageNo = Convert.ToInt16(tempLastPageArr[tempLastPageArr.Length - 3].Split(new string[] { "' class" }, StringSplitOptions.None)[0].Split(new string[] { "pageno=" }, StringSplitOptions.None)[1].Split('\'')[0]);
                    }
                    catch (Exception)
                    {
                        lastPageNo = 1;
                    }

                }
                else
                    lastPageNo = Convert.ToInt16(tempLastPageArr[tempLastPageArr.Length - 1].Split(new string[] { "' class" }, StringSplitOptions.None)[0].Split(new string[] { "pageno=" }, StringSplitOptions.None)[1]);

                for (int i = 1; i < lastPageNo + 1; i++)
                {
                    string content = GetPage((mainUrl + dataCenterUrl + where + pageNoUrl + i));
                    string[] playerArr = content.Split(new string[] { "<td class=\"vs\">" }, StringSplitOptions.None);

                    for (int j = 1; j < playerArr.Length; j++)
                    {
                        string[] info = playerArr[j].Split(new string[] { "addPlayerVs(" }, StringSplitOptions.None)[1].Split(';')[0].Replace("\'", "").Split(',');
                        PlayerInfo pi = new PlayerInfo();
                        pi.playerIcon = info[0];
                        pi.playerID = info[1];
                        pi.playerName = info[2];

                        //new Thread(() => DetailAnalysis(pi)).Start();
                        DetailAnalysis(pi);
                    }
                }

                dataGridView1.Columns.Clear();

                DataTable dt = new DataTable();
                dt.Columns.Add("Season");
                dt.Columns.Add("PName");
                dt.Columns.Add("Multiply", typeof(Double));
                dt.Columns.Add("Detail");


                foreach (PlayerInfo item in lstPlayers)
                {
                    DataRow dr = dt.NewRow();
                    dr["Season"] = item.playerIcon;
                    dr["PName"] = item.playerName;
                    dr["Multiply"] = item.playerMultiply;
                    dr["Detail"] = mainUrl + detailUrl + item.playerID;
                    dt.Rows.Add(dr);
                }
                //DataView dv = dt.DefaultView;
                //dv.Sort = "Multiply DESC";
                //dt = dv.ToTable();
                dt.DefaultView.Sort = "Multiply DESC";
                dataGridView1.DataSource = dt;
                dataGridView1.Columns["Season"].Visible = false;
                DataGridViewImageColumn iconColumn = new DataGridViewImageColumn();
                iconColumn.Name = "img";
                iconColumn.HeaderText = "시즌";
                iconColumn.ValuesAreIcons = true;
                dataGridView1.Columns.Insert(0, iconColumn);
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                foreach (DataGridViewColumn item in dataGridView1.Columns)
                {
                    item.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                try
                {
                    foreach (DataGridViewRow item in dataGridView1.Rows)
                    {
                        if (item.Cells["Season"].Value != "")
                        {
                            ResourceManager rm = Resources.ResourceManager;
                            Icon rowIcon = Icon.FromHandle(((Bitmap)rm.GetObject("_" + item.Cells["Season"].Value)).GetHicon());
                            item.Cells["img"].Value = rowIcon;
                        }

                    }
                }
                catch (Exception)
                {

                }
                this.Activate();
                this.Focus();
                //this.WindowState = FormWindowState.Normal;
                MessageBox.Show("Search Complete");

                string query = string.Format(@"INSERT INTO c_search_log (
                                                   C_USER_ID
                                                ) VALUES (
                                                   '{0}'  -- C_USER_ID - IN varchar(100)
                                                );", authDT.Rows[0]["C_USER_ID"].ToString());
                MySqlHelper.ExecuteNonQuery(query);
            }));
        }
        void DetailAnalysis(PlayerInfo pi)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                try
                {
                    string detailContent = GetPage(mainUrl + detailUrl + pi.playerID);

                    double price = Convert.ToDouble(detailContent.Split(new string[] { "<span class=\"price\">" }, StringSplitOptions.None)[Convert.ToInt16(cboCard.Text)].Split(new string[] { " EP" }, StringSplitOptions.None)[0].Replace(",", ""));
                    double comparePrice = Convert.ToDouble(detailContent.Split(new string[] { "<span class=\"price\">" }, StringSplitOptions.None)[Convert.ToInt16(cboCompareCard.Text)].Split(new string[] { " EP" }, StringSplitOptions.None)[0].Replace(",", ""));

                    pi.playerMultiply = Convert.ToDouble((comparePrice / price).ToString(".##"));

                    if (pi.playerMultiply >= Convert.ToDouble(txtMultiply.Text))
                    {
                        lstPlayers.Add(pi);
                    }
                }
                catch (Exception)
                {

                }
            }));

        }
        string GetPage(string url)
        {
            string readUrl = url;
            string result = "";

            CookieCollection Cookies = new CookieCollection();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(readUrl);
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.93 Safari/537.36";
            request.Host = mainUrl.Split(new string[] { "://" }, StringSplitOptions.None)[1].Split('/')[0].ToString(); ;
            //request.Headers.Add("Origin", mainUrl);
            request.Referer = mainUrl;
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
        #endregion

        #region DB Query
        DateTime GetServerTime()
        {
            string sql = string.Format(@"SELECT NOW();");
            DateTime nowTIme = Convert.ToDateTime(MySqlHelper.ExecuteDataTable(sql).Rows[0][0].ToString());
            return nowTIme;
        }
        #endregion

        #region HWID

        private void GetUniqueID()
        {
            string volumeSerial = DiskID.getDiskID();
            string cpuID = CpuID.getCpuID();
            string windowsID = WindowsID.getWindowsID();
            _uID = volumeSerial + cpuID;
            _uIDW = _uID + windowsID;
        }
        #endregion


        #region 매크로
        void TradeMacro()
        {
            GoTrade();
            GoFavorite();
            TradeMacro();
        }


        void UpdateStatus(string msg)
        {
            this.Invoke(new MethodInvoker(delegate()
                {
                    txtLog.AppendText("[" + GetServerTime().ToString("MM-dd HH:mm:ss") + "] " + msg);
                    txtLog.AppendText("\r\n");
                }));
        }
        Point ImgMatch(Bitmap big, Bitmap small, Module.Handling.Imaging.ImageRange range)
        {
            Point p = Imaging.ImageMatching(big, small, range.loc, range.width, range.height);

            return p;
        }

        void SendKey(Point targetPoint, Module.Handling.Imaging.ImageRange range)
        {
            MessageCtr.SendKey(new Point(targetPoint.X + range.loc.X, targetPoint.Y + range.loc.Y));
            Thread.Sleep(200);
        }

        #region 강화장사

        void GoTrade()
        {
            Bitmap big = (Bitmap)Imaging.GetScreen();
            //Imaging.GetScreen().Save("C:\\test.png");
            Bitmap small = global::SearchFIFASales.Properties.Resources.이적시장_버튼;
            Module.Handling.Imaging.ImageRange range = dictRange["이적시장_버튼"];
            Point targetPoint = ImgMatch(big, small, range);
            if (targetPoint != new Point(0, 0))
            {
                SendKey(targetPoint, range);
                UpdateStatus("이적시장으로 이동");
            }
        }

        void GoFavorite()
        {
            Bitmap big = (Bitmap)Imaging.GetScreen();
            Bitmap small = global::SearchFIFASales.Properties.Resources.이적시장_즐겨찾기_버튼;
            Module.Handling.Imaging.ImageRange range = dictRange["이적시장_즐겨찾기_버튼"];
            Point targetPoint = ImgMatch(big, small, range);
            if (targetPoint != new Point(0, 0))
            {
                SendKey(targetPoint, range);
                UpdateStatus("이적시장_즐겨찾기로 이동");
            }
        }
        #endregion


        #endregion
        #endregion
    }

    #region 클래스
    public class PlayerInfo
    {
        public string playerIcon;
        public string playerName;
        public string playerID;
        public double playerMultiply;
    }

    public static class CpuID
    {
        public static string getCpuID()
        {
            return ProcessorId();
        }

        [DllImport("user32", EntryPoint = "CallWindowProcW", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr CallWindowProcW([In] byte[] bytes, IntPtr hWnd, int msg, [In, Out] byte[] wParam, IntPtr lParam);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool VirtualProtect([In] byte[] bytes, IntPtr size, int newProtect, out int oldProtect);

        const int PAGE_EXECUTE_READWRITE = 0x40;

        private static string ProcessorId()
        {
            byte[] sn = new byte[8];

            if (!ExecuteCode(ref sn))
                return "ND";

            return string.Format("{0}{1}", BitConverter.ToUInt32(sn, 4).ToString("X8"), BitConverter.ToUInt32(sn, 0).ToString("X8"));
        }

        private static bool ExecuteCode(ref byte[] result)
        {
            int num;

            /* The opcodes below implement a C function with the signature:
             * __stdcall CpuIdWindowProc(hWnd, Msg, wParam, lParam);
             * with wParam interpreted as an 8 byte unsigned character buffer.
             * */

            byte[] code_x86 = new byte[] {
                0x55,                      /* push ebp */
                0x89, 0xe5,                /* mov  ebp, esp */
                0x57,                      /* push edi */
                0x8b, 0x7d, 0x10,          /* mov  edi, [ebp+0x10] */
                0x6a, 0x01,                /* push 0x1 */
                0x58,                      /* pop  eax */
                0x53,                      /* push ebx */
                0x0f, 0xa2,                /* cpuid    */
                0x89, 0x07,                /* mov  [edi], eax */
                0x89, 0x57, 0x04,          /* mov  [edi+0x4], edx */
                0x5b,                      /* pop  ebx */
                0x5f,                      /* pop  edi */
                0x89, 0xec,                /* mov  esp, ebp */
                0x5d,                      /* pop  ebp */
                0xc2, 0x10, 0x00,          /* ret  0x10 */
            };
            byte[] code_x64 = new byte[] {
                0x53,                                     /* push rbx */
                0x48, 0xc7, 0xc0, 0x01, 0x00, 0x00, 0x00, /* mov rax, 0x1 */
                0x0f, 0xa2,                               /* cpuid */
                0x41, 0x89, 0x00,                         /* mov [r8], eax */
                0x41, 0x89, 0x50, 0x04,                   /* mov [r8+0x4], edx */
                0x5b,                                     /* pop rbx */
                0xc3,                                     /* ret */
            };

            if (IsX64Process())
            {
                IntPtr ptr = new IntPtr(code_x64.Length);

                if (!VirtualProtect(code_x64, ptr, PAGE_EXECUTE_READWRITE, out num))
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                ptr = new IntPtr(result.Length);
                return (CallWindowProcW(code_x64, IntPtr.Zero, 0, result, ptr) != IntPtr.Zero);

            }
            else
            {
                IntPtr ptr = new IntPtr(code_x86.Length);

                if (!VirtualProtect(code_x86, ptr, PAGE_EXECUTE_READWRITE, out num))
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                ptr = new IntPtr(result.Length);
                return (CallWindowProcW(code_x86, IntPtr.Zero, 0, result, ptr) != IntPtr.Zero);
            }

        }

        private static bool IsX64Process()
        {
            return IntPtr.Size == 8;
        }
    }
    class DiskID
    {
        public static string getDiskID()
        {
            return DiskId();
        }
        public static string getDiskID(String diskLetter)
        {
            return DiskId(diskLetter);
        }
        private static string DiskId()
        {
            return DiskId("");
        }
        private static string DiskId(String diskLetter)
        {
            //Find first drive
            if (diskLetter == "")
            {
                foreach (DriveInfo compDrive in DriveInfo.GetDrives())
                {
                    if (compDrive.IsReady)
                    {
                        diskLetter = compDrive.RootDirectory.ToString();
                        break;
                    }
                }
            }
            if (diskLetter.EndsWith(":\\"))
            {
                //C:\ -> C
                diskLetter = diskLetter.Substring(0, diskLetter.Length - 2);
            }
            ManagementObject disk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + diskLetter + @":""");
            disk.Get();

            string volumeSerial = disk["VolumeSerialNumber"].ToString();
            disk.Dispose();

            return volumeSerial;
        }
    }

    class WindowsID
    {
        public static string getWindowsID()
        {
            return WindowsId();
        }
        private static string WindowsId()
        {
            string windowsInfo = "";
            ManagementObjectSearcher managClass = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");

            ManagementObjectCollection managCollec = managClass.Get();

            bool is64bits = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"));

            foreach (ManagementObject managObj in managCollec)
            {
                windowsInfo = managObj.Properties["Caption"].Value.ToString() + Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432") + managObj.Properties["Version"].Value.ToString();
                break;
            }
            windowsInfo.Replace(" ", "");
            windowsInfo.Replace("Windows", "");
            windowsInfo.Replace("windows", "");
            windowsInfo += (is64bits) ? ":64" : "=32";

            //md5 hash of the windows version
            MD5 md5Hasher = MD5.Create();
            byte[] wi = md5Hasher.ComputeHash(Encoding.Default.GetBytes(windowsInfo));
            string wiHex = BitConverter.ToString(wi);
            wiHex = wiHex.Replace("-", "");

            return wiHex;
        }
    }
    #endregion
}

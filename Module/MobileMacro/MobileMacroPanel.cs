using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Module.Handling;
using System.Threading;
using System.IO;
using System.Diagnostics;
using Module.Properties;

namespace Module.MobileMacro
{
    public partial class MobileMacroPanel : UserControl
    {
        Thread t;
        Adb mAdb = new Adb();
        Dictionary<string, Module.Handling.Imaging.ImageRange> dictRange = new Dictionary<string, Imaging.ImageRange>();


        #region 이미지
        Bitmap Mobile_shop_button = global::Module.Properties.Resources.Mobile_shop_button;
        #endregion
        public MobileMacroPanel()
        {
            InitializeComponent();
            this.Load += MobileMacroPanel_Load;
        }

        void MobileMacroPanel_Load(object sender, EventArgs e)
        {
            InitImage();
            mAdb.adbPath = SearchADBFilename();
            btnFind.Click += btnFind_Click;
            btnRefresh.Click += btnRefresh_Click;
            btnMonitoring.Click += btnMonitoring_Click;

            btnRefresh_Click(null, null);

            if (cboADBList.Items.Count > 0)
                cboADBList.SelectedIndex = 0;

            btnMonitoring.PerformClick();
        }

        void InitImage()
        {
            dictRange.Add("Mobile_shop_button", new Imaging.ImageRange(0, 700, 480, 100));
        }
        #region 이벤트
        void btnRefresh_Click(object sender, EventArgs e)
        {
            cboADBList.Items.Clear();
            Cursor = Cursors.WaitCursor;
            btnRefresh.Text = "갱신중";
            btnRefresh.Enabled = false;

            try
            {
                string[] devices = mAdb.getDievices();
                foreach (string device in devices)
                    cboADBList.Items.Add(device);
            }
            catch (Exception exc)
            {
                MessageBox.Show(this, exc.ToString(), "예외 발생", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            btnRefresh.Enabled = true;
            btnRefresh.Text = "새로고침";
            Cursor = Cursors.Default;
        }

        void btnMonitoring_Click(object sender, EventArgs e)
        {
            if (btnMonitoring.Text.Equals("시작"))
            {
                if (cboADBList.SelectedIndex < 0)
                    return;

                btnRefresh.Enabled = false;
                cboADBList.Enabled = false;

                btnMonitoring.Text = "중지";
                mAdb.device = cboADBList.Text;
                t = new Thread(Monitoring);
                t.Start();
            }
            else
            {
                btnRefresh.Enabled = true;
                cboADBList.Enabled = true;

                btnMonitoring.Text = "시작";
                t.Abort();
            }
        }

        void btnFind_Click(object sender, EventArgs e)
        {
            //mAdb.bmp.Save("C:\\test.png");
            //mAdb.Touch(145, 155);
            
            Point p = ImageMatch("Mobile_shop_button");
            mAdb.Touch(p.X, p.Y + dictRange["Mobile_shop_button"].loc.Y);
            //p = new Point(p.X, p.Y);
            //Handling.MessageCtr.SendKey(p);
            Monitoring();
        }
        #endregion

        #region 사용자함수
        private void Monitoring()
        {
            //while (true)
            //{
                mAdb.Capture();
                image.Image = mAdb.bmp;
            //}
        }

        private string SearchADBFilename()
        {
            string ADBFilename;

            //블루스택 설치된 폴더로 찾기
            ADBFilename = string.Format(@"{0}\BlueStacks\HD-Adb.exe", Environment.GetEnvironmentVariable(
                (8 == IntPtr.Size || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))) ?
                "ProgramFiles(x86)" : "ProgramFiles"));
            if (File.Exists(ADBFilename))
                return ADBFilename;

            //녹스 설치된 폴더로 찾기
            ADBFilename = string.Format(@"{0}\Nox\bin\nox_adb.exe", Environment.GetEnvironmentVariable("APPDATA"));
            if (File.Exists(ADBFilename))
                return ADBFilename;

            //실행중인 프로세스로 찾기
            List<Process> processes = new List<Process>();
            processes.AddRange(Process.GetProcessesByName("HD-Frontend"));
            processes.AddRange(Process.GetProcessesByName("Nox"));
            if (processes.Count > 0)
            {
                string ADBName = null;
                switch (processes[0].ProcessName)
                {
                    case "HD-Frontend":
                        ADBName = "HD-Adb";
                        break;

                    case "Nox":
                        ADBName = "nox_adb";
                        break;
                }
                ADBFilename = processes[0].Modules[0].FileName;
                ADBFilename = string.Format("{0}{1}.exe", ADBFilename.Remove(ADBFilename.LastIndexOf("\\") + 1), ADBName);
                return ADBFilename;
            }

            return null;
        }

        private Point ImageMatch(string destName)
        {
            Monitoring();
            ((Bitmap)Resources.ResourceManager.GetObject(destName)).Save("C:\\test1.png");
            Rectangle r = new Rectangle(0, 0, mAdb.bmp.Width, mAdb.bmp.Height);
            Bitmap big = new Bitmap(r.Width, r.Height);
            big = mAdb.bmp.Clone(r, big.PixelFormat);
            return Imaging.ImgMatch(big, (Bitmap)Resources.ResourceManager.GetObject(destName), dictRange[destName]);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Module.Handling;
using System.Threading;
using System.IO;
using System.Diagnostics;
using Module.Properties;
using Patagames.Ocr;
using Patagames.Ocr.Enums;


namespace Module.MobileMacro
{
    public partial class MobileMacroPanel : UserControl
    {
        Thread t;
        Adb mAdb = new Adb();
        Dictionary<string, Module.Handling.Imaging.ImageRange> dictRange = new Dictionary<string, Imaging.ImageRange>();


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
            btnStart.Click += btnStart_Click;

            btnRefresh_Click(null, null);

            if (cboADBList.Items.Count > 0)
                cboADBList.SelectedIndex = 0;

            //btnStart.PerformClick();
        }

        void InitImage()
        {
            dictRange.Add("Mobile_shop_button", new Imaging.ImageRange(0, 700, 480, 200));
            dictRange.Add("Mobile_player_scout_empty", new Imaging.ImageRange(0, 690, 480, 210));
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

        void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text.Equals("시작"))
            {
                if (cboADBList.SelectedIndex < 0)
                    return;

                btnRefresh.Enabled = false;
                cboADBList.Enabled = false;
                chkReceive.Enabled = false;
                chkSell.Enabled = false;
                chkTrade.Enabled = false;
                txtSell.Enabled = false;
                txtTrade.Enabled = false;
                

                btnStart.Text = "중지";
                mAdb.device = cboADBList.Text;
                t = new Thread(Macro);
                t.Start();
            }
            else
            {
                btnRefresh.Enabled = true;
                cboADBList.Enabled = true;
                chkReceive.Enabled = true;
                chkSell.Enabled = true;
                chkTrade.Enabled = true;
                txtSell.Enabled = true;
                txtTrade.Enabled = true;
                
                btnStart.Text = "시작";
                t.Abort();
            }
        }

        void btnFind_Click(object sender, EventArgs e)
        {
            //mAdb.bmp.Save("C:\\test.png");
            //mAdb.Touch(150, 480);
            
            //Point p = ImageMatch("Mobile_shop_button");
            //mAdb.Touch(p.X, p.Y + dictRange["Mobile_shop_button"].loc.Y);
            ////p = new Point(p.X, p.Y);
            ////Handling.MessageCtr.SendKey(p);
            //Monitoring();

            //Imaging.GetScreen().Save("C:\\test123.png");
            //Handling.MessageCtr.SendKey(new Point(156, 511));

            
        }
        #endregion

        #region 매크로 영역
        Point NullPoint = new Point(0, 0);
        private void PlayerScoutEmpty(Bitmap big)
        {
            if (ImageMatch(big, "Mobile_player_scout_empty") != NullPoint)
            {
                Point p = ImageMatch(big, "Mobile_shop_button");
                big.Save("C:\\testest.png");
                if (p != NullPoint)
                {
                    mAdb.Touch(p.X, p.Y + dictRange["Mobile_shop_button"].loc.Y);
                    //Console.WriteLine(Stopwatch.);

                }
            }
        }
        #endregion
        #region 사용자함수
        private void Macro()
        {
            Bitmap big = Imaging.GetScreen();

            //big.Save("C:\\test.png");
            image.Image = big;
            PlayerScoutEmpty(big);
            //GetOCR(big, new Point(170, 436), 201, 36);
            
            //OcrEngine ocr = new OcrEngine();
            //ocr.Image = ImageStream.FromFile("C:\\test.png");
            
            //Tesseract ocr = new Tesseract();
            //ocr.SetVariable("tessedit_char_whitelist", "0123456789");
            //ocr.Init(@"C:\\temp", "kor", true);
            //List<Word> result = ocr.DoOCR(big, Rectangle.Empty);
            //foreach (Word item in result)
            //{
            //    Console.WriteLine(item.Confidence);
            //    Console.WriteLine(item.Text);
            //}


            //if (ocr.Process())
            //{
            //    Console.WriteLine(ocr.Text);
            //}
            //Thread.Sleep(200);

        }

        private string GetOCR(Bitmap big, Point p, int width, int height)
        {
            big = Imaging.CropImage(big, p, width, height);
            OcrApi.PathToEngine = @"C:\tesseract.dll";
            var api = OcrApi.Create();
            Languages[] lang = { Languages.English, Languages.Korean };
            api.Init(lang);
            string plainText = api.GetTextFromImage(big);
            Console.WriteLine(plainText);
            this.Invoke(new MethodInvoker(delegate() { txtLog.AppendText(plainText + Environment.NewLine); }));

            return plainText;
        }

        //private void Monitoring()
        //{
        //    while (true)
        //    {
        //        //mAdb.Capture();
        //        //image.Image = mAdb.bmp;
        //        image.Image = Imaging.GetScreen();
        //        Thread.Sleep(200);
        //    }
            
        //}

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

        private Point ImageMatch(Bitmap big, string destName)
        {
            //((Bitmap)Resources.ResourceManager.GetObject(destName)).Save("C:\\test1.png");
            return Imaging.ImgMatch(big, (Bitmap)Resources.ResourceManager.GetObject(destName), dictRange[destName]);
        }
        #endregion
    }
}

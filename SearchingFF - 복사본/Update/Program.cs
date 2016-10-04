using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Update.Data;

namespace Update
{
    class Program
    {
        static string retVal = "";
        static List<FileInfo> fileList = new List<FileInfo>();
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
        static void Main(string[] args)
        {
            _cookie = new CookieContainer();
            if (args.Length > 0)
                retVal = args[0];
            else
                retVal = "0";

            DownLoad();
            //WebClient web = new WebClient();
            //web.DownloadFile(link, Environment.CurrentDirectory);
            Console.WriteLine("Update Complete");
            Console.WriteLine("종료 후, SearchFIFASales.exe를 다시 실행해주세요.");
            Console.Read();

        }

        static Thread t = null;
        private static void DownLoad()
        {
            if (GetFileList())
            {
                t = new Thread(new ThreadStart(ThreadProc));
                t.Start();
            }
            else
                Environment.Exit(0);
        }

        private static bool GetFileList()
        {

            try
            {

                DataParameters items = new DataParameters();
                items.Add(new DataParameter("?VERSION", int.Parse(retVal)));

                using (DataTable dt = GetUpdateInfo(items))
                {
                    fileList.Clear();

                    if (dt != null) // && dt.Rows.Count > 0)
                    {
                        foreach (DataRow reader in dt.Rows)
                        {
                            FileInfo fileInfo = new FileInfo(int.Parse(reader["UPDATE_INDEX"].ToString()), reader["FILE_PATH"].ToString(), reader["FILE_NAME"].ToString(), reader["FILE_TYPE"].ToString(), reader["FILE"] as Byte[], reader["DESCRIPTION"].ToString().Trim());

                            fileList.Add(fileInfo);
                        }
                    }
                    //else if (dt != null && dt.Rows.Count == 0)
                    //{
                    //    MessageBox.Show("현재 버전이 최신버전입니다.", "업데이트", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    //    return false;
                    //}
                    else
                    {
                        Console.WriteLine("업데이트 정보를 가져오는 중에 오류가 발생하였습니다. \n관리자에게 문의하세요. (카카오톡 : searchfifa)");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return true;
        }
        private static void ThreadProc()
        {
            try
            {
                if (fileList.Count > 0)
                {
                    FileStream fs;
                    BinaryWriter bw;

                    int bufferSize = 2048;
                    byte[] outbyte = new byte[bufferSize];

                    int index = 1;

                    foreach (FileInfo Info in fileList)
                    {
                        if (Info.FilePath.Equals(String.Empty))
                            Info.FilePath = Environment.CurrentDirectory;
                        else
                            Info.FilePath = Environment.CurrentDirectory + Info.FilePath;

                        fs = new FileStream(string.Format("{0}\\Temp_{1}", Info.FilePath, Info.FileName), FileMode.Create, FileAccess.Write);
                        bw = new BinaryWriter(fs);

                        bw.Write(Info.FileBlob, 0, Info.FileBlob.Length);
                        bw.Flush();

                        bw.Close();
                        fs.Close();

                        index += 1;

                        if (File.Exists(Info.FilePath + "\\" + Info.FileName))
                            File.Delete(Info.FilePath + "\\" + Info.FileName);

                        File.Move(Info.FilePath + "\\" + "Temp_" + Info.FileName,
                                  Info.FilePath + "\\" + Info.FileName);

                        if (Info.Type.ToUpper().Trim().Equals("ZIP"))
                        {
                            Process pc = Process.Start(Info.FilePath + "\\" + Info.FileName);

                            pc.WaitForExit();

                            File.Delete(Info.FilePath + "\\" + Info.FileName);
                        }
                        Thread.Sleep(1500);
                    }
                    Thread.Sleep(3500);
                }
                else
                {
                }

                Process.Start(Environment.CurrentDirectory + "\\" + "SearchFIFASales.exe");
            }
            catch (Exception ex)
            {
            }
            finally
            {
                Environment.Exit(0);
            }
        }

        private static DataTable GetUpdateInfo(DataParameters items)
        {
            string sql = "";

            sql = string.Format(@"
                SELECT * FROM C_VERSION WHERE UPDATE_INDEX > ?VERSION AND USE_YN = 'Y' ORDER BY UPDATE_INDEX;
            ");

            return MySqlHelper.ExecuteDataTable(sql, items.ToMySqlParameter());
        }
    }
    public class FileInfo
    {
        public string FilePath { set; get; }
        public string FileName { private set; get; }
        public string Type { private set; get; }
        public Byte[] FileBlob { private set; get; }
        public int VerNo { private set; get; }
        public string Description { set; get; }

        public FileInfo() { }

        public FileInfo(int verNo, string path, String name, string fileType, Byte[] file, string description)
        {
            VerNo = verNo;
            FilePath = path.Trim();
            FileName = name.Trim();
            Type = fileType.Trim();
            FileBlob = file;
            Description = description.Trim();
        }
    }
}

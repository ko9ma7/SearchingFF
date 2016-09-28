using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Module.Handling
{
    class Adb
    {
        private string _adbPath = string.Empty;
        private string _device = string.Empty;
        private Process adbProcess = new Process();
        private Regex regexColor = new Regex("(?<=^[\\w\\d]{7})[\\w\\d ]+", RegexOptions.Multiline | RegexOptions.Compiled);
        private Regex regexColorReplace = new Regex("([\\w\\d]{2})([\\w\\d]{2})", RegexOptions.Compiled);
        private Regex regexPositionX = new Regex("(?<=ABS_MT_POSITION_X).+", RegexOptions.Compiled);
        private Regex regexPositionY = new Regex("(?<=ABS_MT_POSITION_Y).+", RegexOptions.Compiled);
        private Size displaySize = new Size(700, 420);

        public string adbPath
        {
            get
            {
                return _adbPath;
            }
            set
            {
                _adbPath = value;
                adbProcess.StartInfo.FileName = _adbPath;
            }
        }
        public string device
        {
            get
            {
                return _device;
            }
            set
            {
                _device = value;
                GetDisplaySize();
            }
        }
        public Bitmap bmp { get; set; }

        public Adb()
        {
            adbProcess.StartInfo.UseShellExecute = false;
            adbProcess.StartInfo.RedirectStandardOutput = true;
            adbProcess.StartInfo.CreateNoWindow = true;
        }

        public void Touch(int x, int y)
        {
            string comm = string.Format("-s {0} shell input tap {1} {2}", _device, x, y);
            Command(comm);
        }

        public void Command(string argument)
        {
            try
            {
                adbProcess.StartInfo.Arguments = argument;
                adbProcess.Start();
                adbProcess.WaitForExit();
            }
            catch { }
        }
        public Bitmap GetBitmap()
        {
            Bitmap bit = (Bitmap)bmp.Clone();
            return bit;
        }
        public string[] getDievices()
        {
            try
            {
                adbProcess.StartInfo.Arguments = "devices";
                adbProcess.Start();
                MatchCollection matchCollection = Regex.Matches(adbProcess.StandardOutput.ReadToEnd(), ".+(?=device\\s)");
                string[] strArray = new string[matchCollection.Count];
                for (int index = 0; index < matchCollection.Count; ++index)
                    strArray[index] = matchCollection[index].Groups[0].Value.Trim();
                return strArray;
            }
            catch
            {
                return null;
            }
        }
        private void GetDisplaySize()
        {
            try
            {
                adbProcess.StartInfo.Arguments = string.Format("-s {0} shell dumpsys window", _device);
                adbProcess.Start();
                Match match = Regex.Match(adbProcess.StandardOutput.ReadToEnd(), "mUnrestrictedScreen=.+?([\\d]+)x([\\d]+)");
                int num1 = int.Parse(match.Groups[1].Value);
                int num2 = int.Parse(match.Groups[2].Value);
                displaySize = new Size(num1, num2);
            }
            catch
            {
                displaySize = new Size(420, 700);
            }
        }
        public void Capture()
        {
            try
            {
                adbProcess.StartInfo.Arguments = string.Format("-s {0} shell screencap", _device);
                adbProcess.Start();
                byte[] data;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    adbProcess.StandardOutput.BaseStream.CopyTo((Stream)memoryStream);
                    data = memoryStream.ToArray();
                }
                bmp = rawToBitmap(ReplaceByte(data), displaySize.Width, displaySize.Height);
            }
            catch { }
        }
        private Bitmap rawToBitmap(byte[] data, int width, int height)
        {
            GCHandle gcHandle = GCHandle.Alloc((object)data, GCHandleType.Pinned);
            Bitmap bitmap = (Bitmap)new Bitmap(width, height, (width * 4 + 3) / 4 * 4,
                System.Drawing.Imaging.PixelFormat.Format32bppRgb,
                Marshal.UnsafeAddrOfPinnedArrayElement((Array)data, 0)).Clone();
            gcHandle.Free();
            return bitmap;
        }
        private byte[] ReplaceByte(byte[] data)
        {
            byte[] array = new byte[data.Length];
            int newSize = 0;
            for (int index = 0; index < data.Length - 2; ++index)
            {
                if ((int)data[index] == 13 && (int)data[index + 1] == 13 && (int)data[index + 2] == 10)
                {
                    array[newSize] = (byte)13;
                    ++index;
                }
                else
                {
                    array[newSize] = data[index];
                    ++newSize;
                }
            }
            Array.Resize<byte>(ref array, newSize);
            int num1 = displaySize.Width * displaySize.Height * 4;
            int index1 = 0;
            while (index1 < num1)
            {
                byte num2 = array[index1 + 2];
                array[index1 + 2] = array[index1];
                array[index1] = num2;
                index1 += 4;
            }
            return array;
        }
    }
}

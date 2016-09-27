using AForge.Imaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Module.Handling
{
    public static class Imaging
    {
        public static Bitmap GetScreen()
        {
            return Print(GetHWND("fifazf"));
        }

        public static IntPtr GetHWND(string procName)
        {
            Process proc = Process.GetProcessesByName(procName)[0];
            IntPtr hwnd = proc.MainWindowHandle;
            return hwnd;
        }

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);


        public static Bitmap Print(IntPtr hwnd)
        {
            RECT rc;
            RECT clientRc;
            GetClientRect(hwnd, out clientRc);
            GetWindowRect(hwnd, out rc);

            int borderSize = (rc.Width - clientRc.Width) / 2;
            int titlebarSize = (rc.Height - clientRc.Height) - borderSize;


            Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format24bppRgb);
            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();

            PrintWindow(hwnd, hdcBitmap, 0);

            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();

            return CropImage(bmp, new Point(borderSize, titlebarSize), rc.Width - borderSize, rc.Height - titlebarSize);
        }

        public static Point ImageMatching(Bitmap big, Bitmap small, Point cropPoint = new Point(), int width = 0, int height = 0)
        {
            // create template matching algorithm's instance
            // (set similarity threshold to 92.5%)
            small = Imaging.ConvertFormat(small, PixelFormat.Format24bppRgb);
            big = width == 0 && height == 0 ? Imaging.ConvertFormat(big, PixelFormat.Format24bppRgb) : Imaging.ConvertFormat(CropImage(big, cropPoint, width, height), PixelFormat.Format24bppRgb);
            //big = 

            ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.98f);
            // find all matchings with specified above similarity

            TemplateMatch[] matchings = tm.ProcessImage(big, small);
            // highlight found matchings

            BitmapData data = big.LockBits(
                new Rectangle(0, 0, big.Width, big.Height),
                ImageLockMode.ReadWrite, big.PixelFormat);
            foreach (TemplateMatch m in matchings)
            {

                //Drawing.Rectangle(data, m.Rectangle, Color.White);

                //MessageBox.Show(m.Rectangle.Location.ToString());
            }
            big.UnlockBits(data);

            return matchings.Length > 0 ? matchings[0].Rectangle.Location : new Point();
        }
        private static Bitmap ConvertFormat(System.Drawing.Image image, PixelFormat format)
        {
            Bitmap copy = new Bitmap(image.Width, image.Height, format);
            using (Graphics gr = Graphics.FromImage(copy))
            {
                gr.DrawImage(image, new Rectangle(0, 0, copy.Width, copy.Height));
            }
            return copy;
        }

        public static Bitmap CropImage(Bitmap originalImage, Point p, int width, int height)
        {
            Rectangle rc = new Rectangle(p, new Size(width, height));
            Bitmap _img = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(_img);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.DrawImage(originalImage, 0, 0, rc, GraphicsUnit.Pixel);

            return _img;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            private int _Left;
            private int _Top;
            private int _Right;
            private int _Bottom;

            public RECT(RECT Rectangle)
                : this(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom)
            {
            }
            public RECT(int Left, int Top, int Right, int Bottom)
            {
                _Left = Left;
                _Top = Top;
                _Right = Right;
                _Bottom = Bottom;
            }

            public int X
            {
                get { return _Left; }
                set { _Left = value; }
            }
            public int Y
            {
                get { return _Top; }
                set { _Top = value; }
            }
            public int Left
            {
                get { return _Left; }
                set { _Left = value; }
            }
            public int Top
            {
                get { return _Top; }
                set { _Top = value; }
            }
            public int Right
            {
                get { return _Right; }
                set { _Right = value; }
            }
            public int Bottom
            {
                get { return _Bottom; }
                set { _Bottom = value; }
            }
            public int Height
            {
                get { return _Bottom - _Top; }
                set { _Bottom = value + _Top; }
            }
            public int Width
            {
                get { return _Right - _Left; }
                set { _Right = value + _Left; }
            }
            public Point Location
            {
                get { return new Point(Left, Top); }
                set
                {
                    _Left = value.X;
                    _Top = value.Y;
                }
            }
            public Size Size
            {
                get { return new Size(Width, Height); }
                set
                {
                    _Right = value.Width + _Left;
                    _Bottom = value.Height + _Top;
                }
            }

            public static implicit operator Rectangle(RECT Rectangle)
            {
                return new Rectangle(Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height);
            }
            public static implicit operator RECT(Rectangle Rectangle)
            {
                return new RECT(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom);
            }
            public static bool operator ==(RECT Rectangle1, RECT Rectangle2)
            {
                return Rectangle1.Equals(Rectangle2);
            }
            public static bool operator !=(RECT Rectangle1, RECT Rectangle2)
            {
                return !Rectangle1.Equals(Rectangle2);
            }

            public override string ToString()
            {
                return "{Left: " + _Left + "; " + "Top: " + _Top + "; Right: " + _Right + "; Bottom: " + _Bottom + "}";
            }

            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }

            public bool Equals(RECT Rectangle)
            {
                return Rectangle.Left == _Left && Rectangle.Top == _Top && Rectangle.Right == _Right && Rectangle.Bottom == _Bottom;
            }

            public override bool Equals(object Object)
            {
                if (Object is RECT)
                {
                    return Equals((RECT)Object);
                }
                else if (Object is Rectangle)
                {
                    return Equals(new RECT((Rectangle)Object));
                }

                return false;
            }
        }


        public struct ImageRange
        {
            public Point loc;
            public int width;
            public int height;
            
            public ImageRange(int x, int y)
            {
                loc = new Point(x, y);
                width = 150;
                height = 150;
            }
        }
    }
}

using System;
using System.Runtime.InteropServices;
//alias Win32 types to Framework types
//IntPtr
using HANDLE = System.IntPtr;
using HDC = System.IntPtr;
using HBITMAP = System.IntPtr;
using LPVOID = System.IntPtr;
using HINSTANCE = System.IntPtr;

//String
using LPSTR = System.String;
using LPCSTR = System.String;
using LPWSTR = System.String;
using LPCWSTR = System.String;
using LPTSTR = System.String;
using LPCTSTR = System.String;

//UInt32
using DWORD = System.UInt32;
using UINT = System.UInt32;
using ULONG = System.UInt32;
using COLORREF = System.UInt32;

//Int32
using INT = System.Int32;
using LONG = System.Int32;
using BOOL = System.Int32;

//Other
using BYTE = System.Byte;
using SHORT = System.Int16;
using WORD = System.UInt16;
using CHAR = System.Char;
using FLOAT = System.Single;
using DOUBLE = System.Double;

namespace ControlEase.Inspec.DaHeng
{
    /// <summary>
    /// 
    /// </summary>
    public class CWin32Bitmaps
    {
        /// <summary>
        /// 
        /// </summary>
        [StructLayout ( LayoutKind.Sequential )]
        public struct BITMAPFILEHEADER
        {
            /// <summary>
            /// 
            /// </summary>
            public ushort bfType;
            /// <summary>
            /// 
            /// </summary>
            public int bfSize;
            /// <summary>
            /// 
            /// </summary>
            public ushort bfReserved1;
            /// <summary>
            /// 
            /// </summary>
            public ushort bfReserved2;
            /// <summary>
            /// 
            /// </summary>
            public uint bfOffBits;
        }
        /// <summary>
        /// 
        /// </summary>
        [StructLayout ( LayoutKind.Sequential )]
        public struct BITMAPINFOHEADER
        {
            /// <summary>
            /// 
            /// </summary>
            public uint biSize;
            /// <summary>
            /// 
            /// </summary>
            public int biWidth;
            /// <summary>
            /// 
            /// </summary>
            public int biHeight;
            /// <summary>
            /// 
            /// </summary>
            public ushort biPlanes;
            /// <summary>
            /// 
            /// </summary>
            public ushort biBitCount;
            /// <summary>
            /// 
            /// </summary>
            public uint biCompression;
            /// <summary>
            /// 
            /// </summary>
            public int biSizeImage;
            /// <summary>
            /// 
            /// </summary>
            public int biXPelsPerMeter;
            /// <summary>
            /// 
            /// </summary>
            public int biYPelsPerMeter;
            /// <summary>
            /// 
            /// </summary>
            public uint biClrUsed;
            /// <summary>
            /// 
            /// </summary>
            public uint biClrImportant;

        }
        /// <summary>
        /// 
        /// </summary>
        [StructLayout ( LayoutKind.Sequential )]
        public struct RGBQUAD
        {
            /// <summary>
            /// 
            /// </summary>
            public byte rgbBlue;
            /// <summary>
            /// 
            /// </summary>
            public byte rgbGreen;
            /// <summary>
            /// 
            /// </summary>
            public byte rgbRed;
            /// <summary>
            /// 
            /// </summary>
            public byte rgbReserved;
        }
        /// <summary>
        /// 
        /// </summary>
        [StructLayout ( LayoutKind.Sequential )]
        public struct BITMAPINFO
        {
            /// <summary>
            /// 
            /// </summary>
            public BITMAPINFOHEADER bmiHeader;
            /// <summary>
            /// 
            /// </summary>
            [MarshalAs ( UnmanagedType.ByValArray, SizeConst = 256 )]
            public RGBQUAD[] bmiColors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="iStretchMode"></param>
        /// <returns></returns>
        [DllImport ( "gdi32.dll", CharSet = CharSet.Auto )]
        public static extern int SetStretchBltMode (
            HDC hdc,          // handle to DC
            int iStretchMode  // bitmap stretching mode
            );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="XDest"></param>
        /// <param name="YDest"></param>
        /// <param name="nDestWidth"></param>
        /// <param name="nDestHeight"></param>
        /// <param name="XSrc"></param>
        /// <param name="YSrc"></param>
        /// <param name="nSrcWidth"></param>
        /// <param name="nSrcHeight"></param>
        /// <param name="lpBits"></param>
        /// <param name="lpBitsInfo"></param>
        /// <param name="iUsage"></param>
        /// <param name="dwRop"></param>
        /// <returns></returns>
        [DllImport ( "gdi32.dll", CharSet = CharSet.Auto )]
        public static extern int StretchDIBits (
            HDC hdc,                   // handle to DC
            int XDest,                 // x-coord of destination upper-left corner
            int YDest,                 // y-coord of destination upper-left corner
            int nDestWidth,            // width of destination rectangle
            int nDestHeight,           // height of destination rectangle
            int XSrc,                  // x-coord of source upper-left corner
            int YSrc,                  // y-coord of source upper-left corner
            int nSrcWidth,             // width of source rectangle
            int nSrcHeight,            // height of source rectangle
            byte[] lpBits,             // bitmap bits
            LPVOID lpBitsInfo,         // bitmap data            
            UINT iUsage,               // usage options
            DWORD dwRop                // raster operation code
            );
    }
}

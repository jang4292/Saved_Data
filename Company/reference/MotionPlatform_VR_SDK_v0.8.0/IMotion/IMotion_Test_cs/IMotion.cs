using System;
using System.Runtime.InteropServices;     // DLL support

namespace IMotionController
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IM_FORMAT
    {
        public uint nType;
        public uint nSampleRate;
        public uint nChannels;
        public uint nDataFormat;
        public uint nBlockAlign;

        public IM_FORMAT(uint uinit)
        {
            this.nType          = uinit;
            this.nSampleRate    = uinit;
            this.nChannels      = uinit;
            this.nDataFormat    = uinit;
            this.nBlockAlign    = uinit;
        }
    };
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IM_DEVICE_DESC
    {
        public uint nId;
        public uint nType;
        public uint nFormat;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public char[] szName;  
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public char[] szDetail;

        public IM_DEVICE_DESC(uint uinit, char[] initdata)
        {
            this.nId        = uinit;
            this.nType      = uinit;
            this.nFormat    = uinit;

            this.szName     = initdata;
            this.szDetail   = initdata;
        }
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IM_BUFFER
    {
        public uint nFlags;
        public uint nMotionBytes;
        public IntPtr pMotionData; // const uint8*
        public uint nPlayBegin;
        public uint nPlayLength;
        public uint nLoopBegin;
        public uint nLoopLength;
        public uint nLoopCount;
        public uint pContext; // void*

        public IM_BUFFER(uint uinit)
        {
            this.nFlags         = uinit;
            this.nMotionBytes   = uinit;
            this.pMotionData    = (IntPtr)uinit;
            this.nPlayBegin     = uinit;
            this.nPlayLength    = uinit;
            this.nLoopBegin     = uinit;
            this.nLoopLength    = uinit;
            this.nLoopCount     = uinit;
            this.pContext       = uinit;
        }
    };
        
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IM_DIAGNOSTIC_INFO
    {
        public uint bBusy;
        public uint bHome;
        public uint bAlarm;
        public uint bInpos;
        public uint bEmer;
        public double dCmd;
        public double dEnc;

        public IM_DIAGNOSTIC_INFO(uint uinit)
        {
            this.bBusy      = uinit;
            this.bHome      = uinit;
            this.bAlarm     = uinit;
            this.bInpos     = uinit;
            this.bEmer      = uinit;
            this.dCmd       = uinit;
            this.dEnc       = uinit;
        }
    };
    
    class IMotion
    {
        public const int IM_STRING_MAX	= 256;
        public const double IM_PI		= 3.14159265358979323846;
        public const int IM_MAXSHORT	= 0x7FFF;
        public const int IM_MAXINT		= 0x7FFFFFFF;
        public const int IM_MAX_ENUM	= 0x7FFFFFFF;

        public static int MOTION_MAX_VAL(int bits) { return ((1 << (bits - 1)) - 1); }
        public static int MOTION_MAX_16() { return MOTION_MAX_VAL(16); }
        public static int MOTION_MIN_VAL(int bits) { return -(1 << (bits - 1)); }
        public static int MOTION_MIN_16() { return MOTION_MIN_VAL(16); }
        
        /* IMotion type */
        // Used in IMotion_Create
        public const int IM_DEVICE_ID_DEFAULT	= 11;	/* default device IP */
        public const int IM_DEVICE_ID_MAX		= 255;	/* 1 ~ 255 */

        // Used in IMotion_CreateSource
        public const int IM_FORMAT_TAG_DEFAULT			= 0;	/* Motion TAG */
        public const int IM_FORMAT_SAMPLE_RATE_DEFAULT	= 50;	/* 20ms */
        public const int IM_FORMAT_CHANNELS_DEFAULT		= 3;	/* 3-DOF (heave, roll, pitch) */
        public const int IM_FORMAT_SAMPLE_BITS_DEFAULT	= 0x10; /* S16 only (-32768 ~ 32767) */

        public const int IM_FORMAT_DATA_S16 = 0x8010;	/**< Signed 16-bit samples */
        public const int IM_FORMAT_DATA_DEFAULT         = IM_FORMAT_DATA_S16;

        public const int IM_FORMAT_SAMPLE_RATE_MAX		= 200;	/* 5ms */
        public const int IM_FORMAT_CHANNELS_MAX			= 8;	/* 8-DOF (heave, sway, surge, roll, pitch, yaw) */


        // Used in IM_BUFFER.Flags
        public const int IM_ERROR_STATE		= 0x1;
        public const int IM_START_OF_BUFFER	= 0x2;
        public const int IM_END_OF_LOOP		= 0x4;
        public const int IM_END_OF_BUFFER	= 0x8;		// current buffer
        public const int IM_END_OF_STREAM	= 0x10;	

        // Used in IM_BUFFER.nLoopCount
        public const int IM_LOOP_MAX		= 254;
        public const int IM_LOOP_INFINITE	= 255;

        // Used in IMotion_Startup flags (bitmask)
        public const int IM_CFG_DEBUG_MODE	= 0x1;
        public const int IM_CFG_FILE_LOG	= 0x2;		// check "IMotion.log"
        public const int IM_CFG_EMUL_MODE = 0x4;

        /**********************************/

        /*
            Innosim Motion Driver
        */
        [DllImport("IMotion.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int IMotion_Startup(uint flags=0, uint param=0);
        [DllImport("IMotion.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int IMotion_Shutdown();
        [DllImport("IMotion.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int IMotion_GetDeviceCount();
        [DllImport("IMotion.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int IMotion_GetDeviceDescription(int index, out IM_DEVICE_DESC desc);
        [DllImport("IMotion.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static uint IMotion_Create(string devname=null, int devid=0);
        [DllImport("IMotion.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int IMotion_Destroy(uint motion);
        
        /*
            Innosim Motion
        */
        [DllImport("IMotion.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int IMotion_Start(uint device, byte[] init_data=null, uint init_size=0);
        [DllImport("IMotion.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int IMotion_Stop(uint device, uint flags=0);
        [DllImport("IMotion.dll", CallingConvention = CallingConvention.Cdecl)]
        //extern public static int IMotion_SendStream(uint device, byte[] data, uint size, uint count=1);
        extern public static int IMotion_SendStream(uint device, uint[] data, uint size, uint count = 1);
        [DllImport("IMotion.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int IMotion_GetInfo(uint device, out IM_DIAGNOSTIC_INFO info);
        [DllImport("IMotion.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int IMotion_GetDescription(uint device, out IM_DEVICE_DESC desc);
        [DllImport("IMotion.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int IMotion_GetID(uint device);
        [DllImport("IMotion.dll", CallingConvention = CallingConvention.Cdecl)]
        //extern public static uint IMotion_CreateSource(uint device, IM_FORMAT format, IMotionCallback user_func, uint cb_flags=0);
        extern public static uint IMotion_CreateSource(uint device, IntPtr format, IMotionCallback user_func = null, uint cb_flags = 0);
        [DllImport("IMotion.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int IMotion_DestroySource(uint device, uint source);


        /*
            Innosim Motion Source
        */
        [DllImport("IMotion.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int IMotionSource_Start(uint source, uint flags=0);
        [DllImport("IMotion.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int IMotionSource_Stop(uint source, uint flags=0);
        [DllImport("IMotion.dll", CallingConvention = CallingConvention.Cdecl)]
        //extern public static int IMotionSource_SubmitBuffer(uint source, IM_BUFFER buffer);
        extern public static int IMotionSource_SubmitBuffer(uint source, IntPtr buffer);
        [DllImport("IMotion.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static uint IMotionSource_GetQueuedBufferCount(uint source);
        [DllImport("IMotion.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static uint IMotionSource_GetDevice(uint source);
        [DllImport("IMotion.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int  IMotionSource_GetFormat(uint source, out IM_FORMAT format);
    };
};
//------------------------------------------------------------------------------




#define TEST

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using IMotionController;

public delegate void IMotionCallback(uint data_ptr, uint state);

namespace IMotion_test_cs
{
    class Program
    {
        private const int SAMPLE_CHANNELS = IMotion.IM_FORMAT_CHANNELS_DEFAULT;
        private const int SAMPLE_RATE = IMotion.IM_FORMAT_SAMPLE_RATE_DEFAULT;
        
        public void CallbackData(uint data_ptr, uint state)
        {
            uint data = data_ptr;
            uint stae = state;
        }

        static void Main(string[] args)
        {
            IMotionCallback IDataCallback = new IMotionCallback(new Program().CallbackData);
            

            uint motion;
            IM_DEVICE_DESC desc;

            float motion_frequency = 1; /* motion frequency in cycles per sample */
            float motion_amplitude = 1; /* motion scale, -32768 ~ 32767 */
        
            /* Start up */
            IMotion.IMotion_Startup(IMotion.IM_CFG_DEBUG_MODE);

            // Create device (default driver)
            motion = IMotion.IMotion_Create();
            IMotion.IMotion_Start(motion);

            IMotion.IMotion_GetDescription(motion, out desc);


            /**** Motion Stream test (1st wave) ****/
            short[,] buf = new short[SAMPLE_RATE, SAMPLE_CHANNELS];

            double wave;
            uint[] data;
            data = new uint[SAMPLE_CHANNELS];
            
            Array.Clear(buf, 0, SAMPLE_RATE * SAMPLE_CHANNELS); 
            for(int i=0; i<SAMPLE_RATE; i++) {
	            wave = motion_amplitude * Math.Sin(2 * IMotion.IM_PI * (float)i/SAMPLE_RATE * motion_frequency);
                buf[i, 0] = (short)(wave * IMotion.MOTION_MAX_16());
                
                #if TEST // manually device operation test

                for (int j = 0; j < SAMPLE_CHANNELS; j++)
                {
                    data[j] = (uint)buf[i, j];
                }

                IMotion.IMotion_SendStream(motion, data, sizeof(uint) * SAMPLE_CHANNELS, 1);
                System.Threading.Thread.Sleep(1000 / SAMPLE_RATE);
                #endif
            }
            // diagnostics
            IM_DIAGNOSTIC_INFO info;
            int error = IMotion.IMotion_GetInfo(motion, out info);
            if (error == 1)
            {
                Console.WriteLine("Driver Error (code : {0})", error);
            }

            /**** Motion Buffer test (2nd wave) ****/
            IM_FORMAT format;
            format = new IM_FORMAT(0);
            
            format.nSampleRate = SAMPLE_RATE;
            format.nChannels = SAMPLE_CHANNELS;
            format.nDataFormat = IMotion.IM_FORMAT_DATA_DEFAULT;

            int formatsize = Marshal.SizeOf(format);
            IntPtr pformatptr = Marshal.AllocHGlobal(formatsize);
            Marshal.StructureToPtr(format, pformatptr, false);
            byte[] pfotmatbyte = new byte[formatsize];
            Marshal.Copy(pformatptr, pfotmatbyte, 0, formatsize);


            uint source = IMotion.IMotion_CreateSource(motion, pformatptr, IDataCallback, 0);

            IM_BUFFER buffer;
            
            buffer = new IM_BUFFER(0);
            buffer.nMotionBytes = SAMPLE_RATE*SAMPLE_CHANNELS*sizeof(short);

            //1차 배열 
            short[] buffMotion = new short[SAMPLE_RATE * SAMPLE_CHANNELS];
            int cnt = 0;
            for (int i = 0; i < SAMPLE_RATE; i++)
            {
                for (int j = 0; j < SAMPLE_CHANNELS; j++)
                {
                    buffMotion[cnt] = (short)buf[i, j];
                    cnt++;
                }
            }

            IntPtr pMotiondataptr = Marshal.AllocHGlobal(buffMotion.Length * Marshal.SizeOf(typeof(short)));
            Marshal.Copy(buffMotion, 0, pMotiondataptr, buffMotion.Length);
            buffer.pMotionData = pMotiondataptr;

            ////////////////////////////////////////////////////////////

            int datasize = Marshal.SizeOf(buffer);
            IntPtr pbuffer = Marshal.AllocHGlobal(datasize);
            Marshal.StructureToPtr(buffer, pbuffer, false);
            byte[] pntdata = new byte[datasize];
            Marshal.Copy(pbuffer, pntdata, 0, datasize);

            IMotion.IMotionSource_SubmitBuffer(source, pbuffer);
            IMotion.IMotionSource_Start(source);

            while (IMotion.IMotionSource_GetQueuedBufferCount(source) > 0)
            {
                System.Threading.Thread.Sleep(10);
            }

            IMotion.IMotionSource_Stop(source);

            Marshal.FreeHGlobal(pformatptr);
            Marshal.FreeHGlobal(pMotiondataptr);
            Marshal.FreeHGlobal(pbuffer);

            IMotion.IMotion_DestroySource(motion, source);
            IMotion.IMotion_Destroy(motion);
            IMotion.IMotion_Shutdown();
        }
    }
}

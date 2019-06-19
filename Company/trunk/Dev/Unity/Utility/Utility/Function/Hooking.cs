using UnityEngine;
using System;
using System.Runtime.InteropServices;
using UnityEngine.Networking;
using CommonData;

namespace Utility.Function
{
    public class Hooking : MonoBehaviour
    {


        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto,
         CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn,
        IntPtr hInstance, int threadId);


        [DllImport("user32.dll", CharSet = CharSet.Auto,
         CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto,
         CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode,
        IntPtr wParam, IntPtr lParam);

        const int VK_PROCESSKEY = 0xE5;
        const int WM_IME_COMPOSITION = 0x10F;
        const int WM_IME_ENDCOMPOSITION = 0x10E;
        const int KEYEVENTF_EXTENDEDKEY = 0x1;
        const int KEYEVENTF_KEYUP = 0x2;

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        const int WH_KEYBOARD_LL = 13;
        const int WM_KEYDOWN = 0x100;

        private LowLevelKeyboardProc _proc = hookProc;

        private static IntPtr hhook = IntPtr.Zero;

        public void SetHook()
        {
            IntPtr hInstance = LoadLibrary("User32");
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, hInstance, 0);
        }


        public void UnHook()
        {
            UnhookWindowsHookEx(hhook);
        }

        private static IHookingInterface iHookingInterface;
        public void SetInterfaceHooking(IHookingInterface i)
        {
            iHookingInterface = i;
        }

        public interface IHookingInterface
        {
            // void Left(MyMessage msg);
            // void Right(MyMessage msg);
            // void Return(MyMessage msg);
            MessageBase Left(int key);
            MessageBase Right(int key);
            MessageBase Return(int key);
        }

        public static IntPtr hookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            var vk_value = (VirtualKeyData.VKeys)vkCode;

            if (WM_KEYDOWN.Equals((int)wParam))
            {

                //var msg = new MyMessage();

                switch (vk_value)
                {
                    case VirtualKeyData.VKeys.VK_LEFT:

                        //Debug.Log("left");
                        //msg.number = (int)VirtualKeyData.VKeys.VK_LEFT;
                        //Setting.Controller.Instance.indexDown();
                        //msg.title = VideoXMLDataList.lists[Setting.Controller.Instance.Index].title;
                        //msg.number = (int)VirtualKeyData.VKeys.VK_LEFT;
                        if (iHookingInterface != null)
                        {
                            //iHookingInterface.Left(msg);
                            var msg = iHookingInterface.Left((int)VirtualKeyData.VKeys.VK_LEFT);
                            NetworkServer.SendToAll(MsgType.Highest + 1, msg);
                        }
                        break;

                    case VirtualKeyData.VKeys.VK_RIGHT:
                        //Debug.Log("right");
                        //msg.number = (int)VirtualKeyData.VKeys.VK_RIGHT;
                        //Setting.Controller.Instance.indexUp();
                        //msg.title = VideoXMLDataList.lists[Setting.Controller.Instance.Index].title;
                        //msg.number = (int)VirtualKeyData.VKeys.VK_RIGHT;
                        if (iHookingInterface != null)
                        {
                            // iHookingInterface.Right(msg);
                            var msg = iHookingInterface.Right((int)VirtualKeyData.VKeys.VK_RIGHT);
                            NetworkServer.SendToAll(MsgType.Highest + 1, msg);
                        }

                        break;

                    case VirtualKeyData.VKeys.VK_RETURN:
                        //Debug.Log("return");
                        //                  msg.number = (int)VirtualKeyData.VKeys.VK_RETURN;
                        //                msg.title = VideoXMLDataList.lists[Setting.Controller.Instance.Index].title;
                        //              msg.delayTimeForStart = SettingData.DELAY_TIME_FOR_START_VIDEO_ABOUT_DEVICE;
                        //msg.number = (int)VirtualKeyData.VKeys.VK_RETURN;
                        if (iHookingInterface != null)
                        {
                            //iHookingInterface.Return(msg);
                            var msg = iHookingInterface.Return((int)VirtualKeyData.VKeys.VK_RETURN);
                            NetworkServer.SendToAll(MsgType.Highest + 1, msg);
                        }
                        //ServerAPI.Instance.SendStartMessageToServer();
                        //UnHook();
                        break;
                }
                //NetworkServer.SendToAll(MsgType.Highest + 1, msg);
            }
            return CallNextHookEx(hhook, code, (int)wParam, lParam);
        }
    }
}
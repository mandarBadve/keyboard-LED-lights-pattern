using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;

namespace LEDLights
{
    class Program
    {
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);

        static void Main(string[] args)
        {
            resetLED();

            List<Tuple<int, LED, int, LED, int, LED, int>> configData = Config.GetData();

            foreach (var tuple in configData)
            {
                for (int i = 0; i < tuple.Item1 * 2; i++)
                {
                    executeLED(tuple.Item2);
                    Thread.Sleep(tuple.Item3);
                    executeLED(tuple.Item4);
                    Thread.Sleep(tuple.Item5);
                    executeLED(tuple.Item6);
                    Thread.Sleep(tuple.Item7);
                }
            }
        }

        static void executeLED(LED led)
        {
            switch (led)
            {
                case LED.NUMLOCK:
                    numlock();
                    break;
                case LED.CAPSLOCK:
                    capslock();
                    break;
                case LED.SCROLLLOCK:
                    scrolllock();
                    break;
                default:
                    break;
            }
        }

        static void resetLED()
        {
            bool capsLock = (((ushort)GetKeyState(0x14)) & 0xffff) != 0;
            bool numLock = (((ushort)GetKeyState(0x90)) & 0xffff) != 0;
            bool scrollLock = (((ushort)GetKeyState(0x91)) & 0xffff) != 0;
            if (capsLock)
                capslock();
            if (numLock)
                numlock();
            if (scrollLock)
                scrolllock();
        }

        static void capslock()
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENTF_KEYUP = 0x2;

            keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        static void numlock()
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENTF_KEYUP = 0x2;

            keybd_event(0x90, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            keybd_event(0x90, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
        }

        static void scrolllock()
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENTF_KEYUP = 0x2;

            keybd_event(0x91, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            keybd_event(0x91, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
        }
    }

    enum LED
    {
        NUMLOCK,
        CAPSLOCK,
        SCROLLLOCK
    }

    class Config
    {
        protected const int _internval = 50;
        protected const int _pause = 100;

        public static List<Tuple<int, LED, int, LED, int, LED, int>> GetData()
        {
            List<Tuple<int, LED, int, LED, int, LED, int>> configData = new List<Tuple<int, LED, int, LED, int, LED, int>>();

            Tuple<int, LED, int, LED, int, LED, int> lightConfig =
                new Tuple<int, LED, int, LED, int, LED, int>(
                    30,
                    LED.NUMLOCK,
                    _internval,
                    LED.CAPSLOCK,
                    _internval,
                    LED.SCROLLLOCK,
                    _pause);

            configData.Add(lightConfig);

            return configData;
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using Interceptor;

namespace AzlBot
{

    public delegate bool EnumDelegate(IntPtr hWnd, int lParam);
    class Program
    {

        [DllImportAttribute("User32.dll")]
        private static extern IntPtr FindWindow(String ClassName, String WindowName);

        [DllImportAttribute("User32.dll")]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        //static extern bool GetCursorPos(out uint lpPoint);
        [DllImport("user32.dll")]
        static extern bool EnumDelegate(IntPtr hDesktop, EnumDelegate lpfn, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "GetWindowText",
    ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);
        static int ctr = 0;

        private static string _method { get; set; }
        private static int _minute { get; set; }

        #region callback
        static void callback(object state)
        {

            Input input = new Input();
            //Timer timer = new Timer(callback, "Run buff", TimeSpan.FromSeconds(320), TimeSpan.FromSeconds(320));

            input.KeyboardFilterMode = KeyboardFilterMode.All;
            input.Load();

            var collection = new List<string>();
            EnumDelegate filter = delegate (IntPtr hWnd, int lParam)
            {
                StringBuilder strbTitle = new StringBuilder(255);
                int nLength = GetWindowText(hWnd, strbTitle, strbTitle.Capacity + 1);
                string strTitle = strbTitle.ToString();

                if (IsWindowVisible(hWnd) && string.IsNullOrEmpty(strTitle) == false)
                {
                    collection.Add(strTitle);
                }
                return true;
            };

            if (EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero))
            {
                foreach (var item in collection)
                {

                    if (item == "RF Online") //"RF Online"
                    {
                        Console.WriteLine("passing by");
                        IntPtr RFOnlineHandle = FindWindow(null, item);
                        if (RFOnlineHandle != IntPtr.Zero)
                        {
                            SetForegroundWindow(RFOnlineHandle);
                            input.KeyPressDelay = 1;
                            input.SendKeys(Keys.Nine);
                            input.Unload();
                        }
                        else
                        {
                            Console.WriteLine("Rf not running");
                        }
                        // Console.WriteLine("Process: {0} ID: {1}", theprocess.ProcessName, theprocess.Id);

                    }

                }
            }
            //input.Unload();
            Console.WriteLine("Ticked at {0:HH:mm:ss.fff}", DateTime.Now);
            Console.WriteLine("Called back with state = " + state + ctr++);
        }
        #endregion

        static void Main(string[] args)
        {

            
            _method = args[0].ToString();
            if (args.Count() == 1)
            {
                _minute = 0;
            } 
            else
            {
                _minute = int.Parse(args[1].ToString());
            }
           

            //this will convert hours to minutes.
            _minute = _minute * 60;

            if (_method == string.Empty)
            {
                Console.WriteLine("No argument found");
                return;
            }

            //if (_minute <= 0)
            //{
            //    Console.WriteLine("Cannot set a zero minute");
            //    return;
            //}

            switch (_method)
            {
                case "-buff":
                    AutoBuff();
                    break;
                case "-farm":
                    Farming();
                    break;
                case "-help":
                    GetHelp();
                    break;
                default:
                    Console.WriteLine("wrong argument, type -help for more information ");
                    break;
            }
           // Console.WriteLine("Ticked at {0:HH:mm:ss.fff}", DateTime.UtcNow.ToLocalTime());
        }

        private static void GetHelp()
        {
            Console.WriteLine("Displaying Help information");
            Console.WriteLine("Format argument : AzlBOt.exe -method NoofHour");
            Console.WriteLine("method [-farm] [-buff] hour/minute");
            Console.WriteLine("For minutes use decimal .50 for 30minutes");
        }

        internal static void AutoBuff()
        {
            Console.WriteLine("Starting timer.");
            Input input = new Input();
            var starttime = DateTime.UtcNow;
            Console.WriteLine("Execution starts @ " + starttime.ToLocalTime());
            //Timer timer = new Timer(callback, "Run buff", TimeSpan.FromSeconds(320), TimeSpan.FromSeconds(320));

            input.KeyboardFilterMode = KeyboardFilterMode.All;
            input.Load();

            var collection = new List<string>();
            EnumDelegate filter = delegate (IntPtr hWnd, int lParam)
            {
                StringBuilder strbTitle = new StringBuilder(255);
                int nLength = GetWindowText(hWnd, strbTitle, strbTitle.Capacity + 1);
                string strTitle = strbTitle.ToString();

                if (IsWindowVisible(hWnd) && string.IsNullOrEmpty(strTitle) == false)
                {
                    collection.Add(strTitle);
                }
                return true;
            };

            if (EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero))
            {
                foreach (var item in collection)
                {

                    if (item == "RF Online") //"RF Online"
                    {
                        Console.WriteLine("Do some push ups and coffee. This will take a while.");
                        IntPtr RFOnlineHandle = FindWindow(null, item);
                        if (RFOnlineHandle != IntPtr.Zero)
                        {
                            int ctr = 0;
                            //input.MoveMouseBy(730, 342,true); //x,y
                            //Console.WriteLine("x: " + System.Windows.Forms.Cursor.Position.X + " y: " + System.Windows.Forms.Cursor.Position.Y);
                            //input.SendLeftClick();
                            while (DateTime.UtcNow.ToLocalTime() - starttime.ToLocalTime() < TimeSpan.FromMinutes(_minute)) //14400000 ms
                            {
                                SetForegroundWindow(RFOnlineHandle);

                                input.KeyPressDelay = 20;
                                input.SendKey(Keys.Nine, KeyState.Down);
                                Thread.Sleep(1);
                                input.SendKey(Keys.Nine, KeyState.Up);                            
                                //Console.WriteLine(ctr);
                                Thread.Sleep(10000);
                              

                                ctr++;
                            }


                        }
                        else
                        {
                            Console.WriteLine("Rf not running");
                        }
                        // Console.WriteLine("Process: {0} ID: {1}", theprocess.ProcessName, theprocess.Id);

                    }

                }
                //}
                input.Unload();
                Thread.Sleep(2000);
                Console.WriteLine("Ticked at {0:HH:mm:ss.fff}", DateTime.UtcNow.ToLocalTime());
            }
        }
        internal static void Farming()
        {
            Console.WriteLine("Starting timer.");
            Input input = new Input();
            var starttime = DateTime.UtcNow;
            //Timer timer = new Timer(callback, "Run buff", TimeSpan.FromSeconds(320), TimeSpan.FromSeconds(320));

            input.KeyboardFilterMode = KeyboardFilterMode.All;
            input.Load();

            var collection = new List<string>();
            EnumDelegate filter = delegate (IntPtr hWnd, int lParam)
            {
                StringBuilder strbTitle = new StringBuilder(255);
                int nLength = GetWindowText(hWnd, strbTitle, strbTitle.Capacity + 1);
                string strTitle = strbTitle.ToString();

                if (IsWindowVisible(hWnd) && string.IsNullOrEmpty(strTitle) == false)
                {
                    collection.Add(strTitle);
                }
                return true;
            };

            if (EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero))
            {
                foreach (var item in collection)
                {

                    if (item == "RF Online") //"RF Online"
                    {
                        Console.WriteLine("Do some push ups and coffee. This will take a while.");
                        IntPtr RFOnlineHandle = FindWindow(null, item);
                        if (RFOnlineHandle != IntPtr.Zero)
                        {
                            int ctr = 0;
                            while (DateTime.UtcNow.ToLocalTime() - starttime.ToLocalTime() < TimeSpan.FromMinutes(_minute))
                            {
                                SetForegroundWindow(RFOnlineHandle);

                                input.KeyPressDelay = 20;
                                
                                input.SendKey(Keys.Space, KeyState.Down);
                                Thread.Sleep(1);
                                input.SendKey(Keys.Space, KeyState.Up);
                                Thread.Sleep(1); // addtional only for looting
                                input.SendKey(Keys.Space, KeyState.Down);
                                Thread.Sleep(1);
                                input.SendKey(Keys.Space, KeyState.Up);
                                Thread.Sleep(1);
                                input.SendKey(Keys.Space, KeyState.Down);
                                Thread.Sleep(1);
                                input.SendKey(Keys.Space, KeyState.Up);
                                Thread.Sleep(1); // addtional only for looting
                                input.SendKey(Keys.Space, KeyState.Down);
                                Thread.Sleep(1);
                                input.SendKey(Keys.Space, KeyState.Up);
                                Thread.Sleep(1);
                                input.SendKey(Keys.Space, KeyState.Down);
                                Thread.Sleep(1);
                                input.SendKey(Keys.Space, KeyState.Up);
                                Thread.Sleep(1); // addtional only for looting
                                input.SendKey(Keys.Space, KeyState.Down);
                                Thread.Sleep(1);
                                input.SendKey(Keys.Space, KeyState.Up);
                                Thread.Sleep(1);
                                input.SendKey(Keys.Space, KeyState.Down);
                                Thread.Sleep(1);
                                input.SendKey(Keys.Space, KeyState.Up);
                                Thread.Sleep(1); // addtional only for looting
                                input.SendKey(Keys.Space, KeyState.Down);
                                Thread.Sleep(1); 
                                input.SendKey(Keys.Space, KeyState.Up);
                                Thread.Sleep(1);
                                input.SendKey(Keys.Space, KeyState.Down);
                                Thread.Sleep(1);
                                input.SendKey(Keys.Space, KeyState.Up);
                                Thread.Sleep(1); // addtional only for looting
                                input.SendKey(Keys.Space, KeyState.Down);
                                Thread.Sleep(1);
                                input.SendKey(Keys.Space, KeyState.Up);
                                Console.WriteLine(ctr);
                                Thread.Sleep(1000);

                                ctr++;
                            }


                        }
                        else
                        {
                            Console.WriteLine("Rf not running");
                        }
                        // Console.WriteLine("Process: {0} ID: {1}", theprocess.ProcessName, theprocess.Id);

                    }

                }
                //}
                input.Unload();
                Thread.Sleep(20000);
                Console.WriteLine("Ticked at {0:HH:mm:ss.fff}", DateTime.Now);
            }
        }
        //Console.Write(pts.ToString());

    }
}

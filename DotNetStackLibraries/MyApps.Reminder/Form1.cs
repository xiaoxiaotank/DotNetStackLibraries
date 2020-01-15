using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyApps.Reminder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Define the priority of the application (0x3FF = The higher priority)
            SetProcessShutdownParameters(0x3FF, SHUTDOWN_NORETRY);
        }

        /// <summary>
        /// when closing the session
        /// </summary>
        public const int WM_QUERYENDSESSION = 0x0011;
        /// <summary>
        /// when shutting down the machine
        /// </summary>
        public const int WM_ENDSESSION = 0x0016;
        public const uint SHUTDOWN_NORETRY = 0x00000001;

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ShutdownBlockReasonCreate(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string reason);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ShutdownBlockReasonDestroy(IntPtr hWnd);
        [DllImport("kernel32.dll")]
        static extern bool SetProcessShutdownParameters(uint dwLevel, uint dwFlags);


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_QUERYENDSESSION || m.Msg == WM_ENDSESSION)
            {
                // Prevent windows shutdown
                ShutdownBlockReasonCreate(Handle, "打卡了吗？给你10秒钟时间!");
                Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(10));
                    BeginInvoke((Action)(() =>
                    {
                        // This method must be called on the same thread as the one that have create the Handle, so use BeginInvoke
                        ShutdownBlockReasonCreate(Handle, "走吧!");

                        // Allow Windows to shutdown
                        ShutdownBlockReasonDestroy(Handle);
                    }));
                });

                return;
            }

            base.WndProc(ref m);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //switch (e.CloseReason)
            //{
            //    case CloseReason.ApplicationExitCall:
            //        break;
            //    case CloseReason.FormOwnerClosing:
            //        break;
            //    case CloseReason.MdiFormClosing:
            //        break;
            //    case CloseReason.None:
            //        break;
            //    case CloseReason.TaskManagerClosing:
            //        break;
            //    // 注销或关机会触发此事件
            //    case CloseReason.UserClosing:
            //        break;
            //    case CloseReason.WindowsShutDown:
            //        break;
            //    default:
            //        break;
            //}

            MessageBox.Show("不允许关闭！");
            e.Cancel = true;
            base.OnFormClosing(e);
        }
    }
}

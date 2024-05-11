using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ToastLikeNotify
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        readonly Mutex mutex = new Mutex(true, "__ToastLikeNotify_SingleInstance_Mutex__");
        readonly uint ipcmsg = NativeMethods.RegisterWindowMessage("__ToastLikeNotify_ShowToast__");

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!mutex.WaitOne(TimeSpan.Zero, true))
            {
                NativeMethods.PostMessage(NativeMethods.HWND_BROADCAST, ipcmsg, )
                Shutdown();
            }
        }
    }

    // pInvoke stuff
    internal class NativeMethods
    {
        public const int HWND_BROADCAST = 0xffff;

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool PostMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        ///     Defines a new window message that is guaranteed to be unique throughout the system. The message value can be used
        ///     when sending or posting messages.
        ///     <para>
        ///     Go to https://msdn.microsoft.com/en-us/library/windows/desktop/ms644947%28v=vs.85%29.aspx for more
        ///     information.
        ///     </para>
        /// </summary>
        /// <param name="msg">C++ ( lpString [in]. Type: LPCTSTR )<br /> The message to be registered.</param>
        /// <returns>
        ///     C++ ( Type: UINT )<br /> If the message is successfully registered, the return value is a message identifier in the
        ///     range 0xC000 through 0xFFFF. If the function fails, the return value is zero.<br /><br /> To get extended error
        ///     information, call GetLastError.
        /// </returns>
        /// <remarks>
        ///     The <see cref="RegisterWindowMessage" /> function is typically used to register messages for communicating between
        ///     two cooperating applications. If two different applications register the same message string, the applications
        ///     return the same message value.The message remains registered until the session ends. Only use
        ///     <see cref="RegisterWindowMessage" /> when more than one application must process the same message.For sending
        ///     private messages within a window class, an application can use any integer in the range WM_USER through 0x7FFF.
        ///     <br />(Messages in this range are private to a window class, not to an application.For example, predefined control
        ///     classes such as BUTTON, EDIT, LISTBOX, and COMBOBOX may use values in this range.)
        /// </remarks>
        /// <example>
        ///     <code><![CDATA[
        ///  //provide a private internal message id
        ///  private UInt32 queryCancelAutoPlay = 0;
        ///  
        ///  [DllImport("user32.dll", SetLastError=true, CharSet=CharSet.Auto)]
        /// static extern uint RegisterWindowMessage(string lpString);
        ///  
        ///  /* only needed if your application is using a dialog box and needs to respond to a "QueryCancelAutoPlay" message, it cannot simply return TRUE or FALSE.
        ///      [DllImport("user32.dll")]
        ///      static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        /// */
        ///
        /// protected override void WndProc(ref Message m)
        /// {
        ///      //calling the base first is important, otherwise the values you set later will be lost
        ///      base.WndProc(ref m);
        ///      
        ///      //if the QueryCancelAutoPlay message id has not been registered...
        ///      if (queryCancelAutoPlay == 0)
        ///      queryCancelAutoPlay = RegisterWindowMessage("QueryCancelAutoPlay");
        ///      
        ///      //if the window message id equals the QueryCancelAutoPlay message id
        ///      if ((UInt32)m.Msg == queryCancelAutoPlay)
        ///      {
        ///      /* only needed if your application is using a dialog box and needs to
        ///      * respond to a "QueryCancelAutoPlay" message, it cannot simply return TRUE or FALSE.
        ///      SetWindowLong(this.Handle, 0, 1);
        ///      */
        ///      m.Result = (IntPtr)1;
        ///      }
        /// }
        ///  ]]></code>
        /// </example>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint RegisterWindowMessage(string lpString);
    }
}

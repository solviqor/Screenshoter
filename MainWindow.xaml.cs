using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Screenshoter.Utilities;
using Clipboard = System.Windows.Clipboard;

namespace Screenshoter;

public partial class MainWindow : Window
{
    private System.Windows.Forms.NotifyIcon _notifyIcon;
    private bool _isExit;
    
        [DllImport("user32.dll")]
        //method which set a hot key
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
 
        //method which remove a hot key
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
 
        private const int HOTKEY_ID = 0;
        private bool isComplete = true;
 
        //Modifiers:
        private const uint MOD_NONE = 0x0000; //(none)
        private const uint MOD_ALT = 0x0001; //ALT
        private const uint MOD_CONTROL = 0x0002; //CTRL
        private const uint MOD_SHIFT = 0x0004; //SHIFT
        private const uint MOD_WIN = 0x0008; //WINDOWS
        private const uint MOD_Q = (int)Keys.Q; //Q
        //CAPS LOCK:
        private const uint VK_CAPITAL = 0x14;
 
        private IntPtr _windowHandle;
        private HwndSource _source;
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
 
            _windowHandle = new WindowInteropHelper(this).Handle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);
 
            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_CONTROL, MOD_Q); //CTRL + Q
        }
 
        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            //Get virtual key code and transform it to default key code
                            int vkey = (((int)lParam >> 16) & 0xFFFF);
                            //check if virtual key code equals to hot key
                            if (vkey == MOD_Q)
                            {
                                if (isComplete)
                                {
                                    isComplete = false;
                                    var screenShot = new ScreenCaptureUtility().CaptureScreen(out isComplete);
                                    BalloonNotify.CompletedMsg("Success", "Screenshot save in clipboard");
                                    Clipboard.SetDataObject(screenShot);
                                }
                            }
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }
 
        protected override void OnClosed(EventArgs e)
        {
            _source.RemoveHook(HwndHook);
            UnregisterHotKey(_windowHandle, HOTKEY_ID);
            base.OnClosed(e);
        }
    public MainWindow()
    {
        var startupPath = new Uri(@"Icon/Logo.ico", UriKind.Relative);
        Closing += MainWindow_Closing;
        _notifyIcon = new System.Windows.Forms.NotifyIcon();
        _notifyIcon.DoubleClick += (s, args) => ShowMainWindow();
        _notifyIcon.Icon = new Icon(startupPath.ToString());
        _notifyIcon.Visible = true;
    
        CreateContextMenu();
    }
    
    //Tray icon menu
    private void CreateContextMenu()
    {
        _notifyIcon.ContextMenuStrip =
            new System.Windows.Forms.ContextMenuStrip();
        _notifyIcon.ContextMenuStrip.Items.Add("MainWindow...").Click += (s, e) => ShowMainWindow();
        _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();
    }
    
    private void ExitApplication()
    {
        _isExit = true;
        Close();
        _notifyIcon.Dispose();
        _notifyIcon = null;
    }
    
    //show tray icon menu
    private void ShowMainWindow()
    {
        if (IsVisible)
        {
            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }
            Activate();
        }
        else
        {
            Show();
        }
    }
    
    private void MainWindow_Closing(object sender, CancelEventArgs e)
    {
        if (!_isExit)
        {
            e.Cancel = true;
            Hide(); // A hidden window can be shown again, a closed one not
        }
    }
}
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Screenshoter.Utilities;

public static class BalloonNotify
{
    public static void CompletedMsg(string title, string msg, NotifyIcon notifyIcon)
    {
        notifyIcon.BalloonTipTitle = title;
        notifyIcon.BalloonTipText = msg;
        notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
        notifyIcon.Visible = true;
        notifyIcon.ShowBalloonTip(100);
    }

}
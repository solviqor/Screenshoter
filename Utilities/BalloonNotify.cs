using System;
using System.Drawing;
using System.Windows.Forms;

namespace Screenshoter.Utilities;

public static class BalloonNotify
{
    public static void CompletedMsg(string title, string msg)
    {
        var notifyIcon1 = new NotifyIcon();
        notifyIcon1.Icon = SystemIcons.Exclamation;
        notifyIcon1.BalloonTipTitle = title;
        notifyIcon1.BalloonTipText = msg;
        notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
        notifyIcon1.Visible = true;
        notifyIcon1.ShowBalloonTip(1000);

    }

}
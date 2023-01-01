using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Screenshoter.Utilities;

public class ScreenCaptureUtility
{
    private Rectangle canvasBounds = Screen.GetBounds(Point.Empty);

    public ScreenCaptureUtility()
    {
        SetCanvas();
    }
    
    public void SetCanvas()
    {
        using (ScreenshotWindow screenshotWindow = new ScreenshotWindow())
        {
            if (screenshotWindow.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.canvasBounds = screenshotWindow.GetRectangle();
            }
        }
    }

    public Bitmap CaptureScreen(out bool isDone)
    {
        using (Bitmap bmp = new Bitmap(canvasBounds.Width, canvasBounds.Height))
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(new Point
                    (canvasBounds.Left, canvasBounds.Top), Point.Empty, canvasBounds.Size);
            }
            isDone = true;
            return new Bitmap(SetBorder(bmp, Color.CornflowerBlue, 2));

        }
    }

    public Image SetBorder(Image img, Color color, float width)
    {
        // Create a copy of the image and graphics context
        Image? dstImg = img.Clone() as Image;
        Graphics g = Graphics.FromImage(dstImg);
            
        // Create the pen
        Pen pBorder = new Pen(color, width)
        {
            Alignment = PenAlignment.Center
        };
        
        g.DrawRectangle(pBorder, 0, 0, dstImg.Width - 1, dstImg.Height - 1);
        // Clean up
        pBorder.Dispose();
        g.Save();
        g.Dispose();

        // Return
        return dstImg;
    }
}
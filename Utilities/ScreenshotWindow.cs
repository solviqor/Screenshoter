using System;
using System.Windows.Forms;
using System.Drawing;
namespace Screenshoter.Utilities;

public partial class ScreenshotWindow : Form
{
    Point startPos;      // mouse-down position
    Point currentPos;    // current mouse position
    bool drawing;
    private bool isOneInstance = true;
    
    //color outlines and fills

    private static readonly Color borderColor = Color.CornflowerBlue;
    Pen pen = new Pen(borderColor,4);
    Brush brush = Brushes.CornflowerBlue;
    

    public ScreenshotWindow()
    {
        this.WindowState = FormWindowState.Maximized;
        this.BackColor = Color.Black;
        this.FormBorderStyle = FormBorderStyle.None;
        this.Cursor = Cursors.Cross;
        this.Opacity = 0.55;
        this.MouseDown += MouseDownEvent;
        this.MouseMove += MouseMoveEvent;
        this.MouseUp += MouseUpEvent;
        this.Paint += PaintEvent;
        this.KeyDown += KeyDownEvent;
        this.DoubleBuffered = true;
        
    }
    private void KeyDownEvent(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
    private void MouseDownEvent(object sender, MouseEventArgs e)
    {
        drawing = true;
        currentPos = startPos = e.Location;
    }

    private void PaintEvent(object sender, PaintEventArgs e)
    {
        if (drawing)
        {
          e.Graphics.DrawRectangle(pen, GetRectangle());
          e.Graphics.FillRectangle(brush, GetRectangle());
        }
    }
    
    private void MouseMoveEvent(object sender, MouseEventArgs e)
    {
        currentPos = e.Location;
        if (drawing) this.Invalidate();
    }

    private void MouseUpEvent(object sender, MouseEventArgs e)
    {
        this.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.Close();
    }
    
    
    
    public Rectangle GetRectangle()
    {
        return new Rectangle(
            Math.Min(startPos.X, currentPos.X),
            Math.Min(startPos.Y, currentPos.Y),
            Math.Abs(startPos.X - currentPos.X),
            Math.Abs(startPos.Y - currentPos.Y));
    }
}
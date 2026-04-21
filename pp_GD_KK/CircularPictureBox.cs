using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class CircularPictureBox : PictureBox
{
    public float BorderSize { get; set; } = 2f;
    public Color BorderColor { get; set; } = Color.RoyalBlue;

    protected override void OnPaint(PaintEventArgs pe)
    {
        base.OnPaint(pe);
        var graphics = pe.Graphics;

        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        var rectContour = RectangleF.Inflate(this.ClientRectangle, -0.5f, -0.5f);

        using (var path = new GraphicsPath())
        {
            path.AddEllipse(rectContour);

            this.Region = new Region(path);

            if (BorderSize > 0)
            {
                using (var pen = new Pen(BorderColor, BorderSize))
                {
                    pen.Alignment = PenAlignment.Inset;
                    graphics.DrawEllipse(pen, rectContour);
                }
            }
        }
    }
}
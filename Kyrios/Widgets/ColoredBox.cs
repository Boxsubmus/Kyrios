using SkiaSharp;

namespace Kyrios.Widgets;

public class ColoredBox : Widget
{
    private SKColor m_color;

    public ColoredBox(Widget? parent, SKColor color, int width, int height) : base(parent)
    {
        m_color = color;
        Resize(width, height);
    }

    public override void OnPaint(SKCanvas canvas)
    {
        base.OnPaint(canvas);

        using var paint = new SKPaint
        {
            Color = m_color,
            IsAntialias = true
        };
        canvas.DrawRoundRect(new SKRoundRect(new SKRect(0, 0, Width, Height), 8f), paint);
    }
}
using Kyrios.Widgets;
using SkiaSharp;

namespace Sandbox;

internal class TestWindow : MainWindow
{
    int mouseX;
    int mouseY;

    private SKTypeface m_defaultTypeface;

    private int index;

    public TestWindow(int i, string title = "") : base(1280, 720)
    {
        index = i;

        m_defaultTypeface = SKTypeface.FromFamilyName("Segoe UI");

        AddChild(new Checkbox(), 8, 8);
    }

    int frame = 0;

    public override void OnPaint(SKCanvas canvas)
    {
        base.OnPaint(canvas);

        canvas.Clear(new SKColor(40, 40, 40));

        // Update();

        // Console.WriteLine(frame);

        Console.WriteLine($"paint: {index}");

        frame++;

        using var paint = new SKPaint
        {
            Color = SKColors.White,
            IsAntialias = true
        };

        using var skFont = new SKFont
        {
            Edging = SKFontEdging.Antialias,
            Subpixel = true,
            Hinting = SKFontHinting.Normal,
            Typeface = m_defaultTypeface,
            Size = 13
        };

        // canvas.DrawText("Scene Graph", new SKPoint(10, 30), SKTextAlign.Left, skFont, paint);
        canvas.DrawText($"{mouseX}, {mouseY}", new SKPoint(mouseX, mouseY), skFont, paint);
    }

    public override void OnMouseMove(int x, int y)
    {
        base.OnMouseMove(x, y);

        mouseX = x;
        mouseY = y;

        Update();
    }
}
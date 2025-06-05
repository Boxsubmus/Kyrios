using Kyrios.Widgets;
using SkiaSharp;

namespace Sandbox;

internal class TestWindow : MainWindow
{
    int mouseX;
    int mouseY;


    private int index;

    public TestWindow(Widget? parent = null) : base(parent)
    {
        AddChild(
            new Checkbox
            {
                Label = "Selection"
            },
            8, 8);
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
            Typeface = Cache.DefaultTypeFace,
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
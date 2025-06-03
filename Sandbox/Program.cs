using Kyrios;
using Kyrios.Widgets;
using SkiaSharp;

namespace Sandbox;

internal class TestWindow : MainWindow
{
    int mouseX;
    int mouseY;

    public TestWindow(string title = "") : base(1280, 720)
    {
        var redBox = new ColoredBox(this, SKColors.Red, 100, 100);
        var greenBox = new ColoredBox(this, SKColors.Green, 150, 80);
        // var blueBox = new ColoredBox(this, SKColors.Blue, 60, 160);

        AddChild(redBox, 50, 50);
        AddChild(greenBox, 50, 120);
        // AddChild(blueBox, 400, 300);
    }

    public override void OnPaint(SKCanvas canvas)
    {
        base.OnPaint(canvas);

        using var paint = new SKPaint
        {
            Color = SKColors.White,
            IsAntialias = true
        };
        canvas.Clear(new SKColor(20, 20, 20));
        // canvas.DrawText($"{mouseX}, {mouseY}", new SKPoint(mouseX, mouseY), paint);
    }

    public override void OnMouseMove(int x, int y)
    {
        base.OnMouseMove(x, y);

        mouseX = x; mouseY = y;
    }
}

public static class Program
{
    public static void Main(string[] args)
    {
        using var app = new Application();
        
        var mainWindow = new TestWindow();
        mainWindow.Show();

        app.Run(mainWindow);
    }
}
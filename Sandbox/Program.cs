using Kyrios;
using Kyrios.Widgets;
using SkiaSharp;

namespace Sandbox;

internal class TestWindow : MainWindow
{
    public TestWindow(Widget? parent, string title) : base(parent, title)
    {
        var redBox = new ColoredBox(this, SKColors.Red, 100, 100);
        var greenBox = new ColoredBox(this, SKColors.Green, 150, 80);
        var blueBox = new ColoredBox(this, SKColors.Blue, 60, 160);

        AddChild(redBox, 50, 50);
        AddChild(greenBox, 200, 120);
        AddChild(blueBox, 400, 300);
    }

    public override void OnPaint(SKCanvas canvas)
    {
        base.OnPaint(canvas);

        canvas.Clear(new SKColor(20, 20, 20));
    }
}

public static class Program
{
    public static void Main(string[] args)
    {
        using var app = new Application();
        
        var mainWindow = new TestWindow(null, "Test");
        mainWindow.Show();

        app.Run(mainWindow);
    }
}
using Kyrios;
using Kyrios.Widgets;

namespace Sandbox;

public static class Program
{
    public static void Main(string[] args)
    {
        using var app = new Application();

        var mainWindow = new TestWindow();
        mainWindow.SetTitle("Sandbox");
        mainWindow.Resize(300, 200);
        mainWindow.Show();

        var button = new PushButton(mainWindow);
        button.SetRect(40, 40, 200, 30);
        button.Label = "Push Me!";

        app.Run();
    }
}
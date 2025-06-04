using Kyrios;

namespace Sandbox;

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
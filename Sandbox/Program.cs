using Kyrios;
using Kyrios.Widgets;

namespace Sandbox;

public static class Program
{
    public static void Main(string[] args)
    {
        using var app = new Application();

        var windows = new List<MainWindow>();
        for (int i = 0; i < 6; i++)
        {
            var mainWindow = new TestWindow(i);
            mainWindow.Show();

            windows.Add(mainWindow);
        }

        app.Run(windows.ToArray());
    }
}
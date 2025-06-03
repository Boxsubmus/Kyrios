using GLFW;
using Window = Kyrios.Widgets.Window;

namespace Kyrios;

public class Application : IDisposable
{
    private readonly List<Window> m_windows = [];

    public Application()
    {
        Glfw.Init();

        Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
        Glfw.WindowHint(Hint.ContextVersionMajor, 3);
        Glfw.WindowHint(Hint.ContextVersionMinor, 3);
        Glfw.WindowHint(Hint.Doublebuffer, true);
        Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
    }

    public void Run(params Window[] mainWindows)
    {
        m_windows.AddRange(mainWindows);

        foreach (var win in m_windows)
            win.Initialize();

        while (m_windows.Count > 0)
        {
            Glfw.PollEvents();

            // Use a copy of the list to avoid modifying while iterating
            foreach (var win in m_windows.ToArray())
            {
                if (win.ShouldClose())
                {
                    win.Shutdown();
                    m_windows.Remove(win);
                }
                else
                {
                    win.UpdateAndRender();
                }
            }
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        Glfw.Terminate();
    }
}
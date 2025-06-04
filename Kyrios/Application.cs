using SDL2;
using Kyrios.Widgets;
using Kyrios.Platform.Skia;

namespace Kyrios;

internal static class WindowRegistry
{
    private static readonly Dictionary<uint, SkiaWindow> windows = new();

    public static void Register(SkiaWindow window)
    {
        windows[window.WindowID] = window;
    }

    public static SkiaWindow? Get(uint id) =>
        windows.TryGetValue(id, out var win) ? win : null;
}

public class Application : IDisposable
{
    private readonly List<Widget> m_topLevelWidgets = [];

    public Application()
    {
        SDL.SDL_Init(SDL.SDL_INIT_VIDEO);
    }

    public void Run(params Widget[] widgets)
    {
        foreach (var w in widgets)
        {
            if (w.Parent != null) continue;
            
            m_topLevelWidgets.Add(w);
            w.InitializeIfTopLevel();
        }

        while (m_topLevelWidgets.Count > 0)
        {
            pumpEvents();

            foreach (var w in m_topLevelWidgets.ToArray())
            {
                if (w.ShouldClose())
                {
                    w.Dispose();
                    m_topLevelWidgets.Remove(w);
                }
                else
                {
                    w.UpdateAndRender();
                    // Glfw.WaitEvents(); // <- This tells Glfw to wait until the user does something to actually update
                }
            }

            // SDL.SDL_Delay(16); // ~60fps
        }
    }

    public void Dispose()
    {
        foreach (var w in m_topLevelWidgets)
        {
            w.Dispose();
        }

        SDL.SDL_Quit();
        GC.SuppressFinalize(this);
    }

    private void pumpEvents()
    {
        while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
        {
            uint id = e.type switch
            {
                SDL.SDL_EventType.SDL_MOUSEMOTION => e.motion.windowID,
                SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN or SDL.SDL_EventType.SDL_MOUSEBUTTONUP => e.button.windowID,
                SDL.SDL_EventType.SDL_WINDOWEVENT => e.window.windowID,
                _ => 0
            };

            if (WindowRegistry.Get(id) is { } win)
            {
                win.ParentWidget.HandleEvent(e);
            }
        }
    }
}
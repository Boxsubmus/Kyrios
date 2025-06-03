using GLFW;
using Kyrios.Widgets;

namespace Kyrios;

public class Application : IDisposable
{
    private readonly List<Widget> m_topLevelWidgets = [];

    public Application()
    {
        Glfw.Init();

        Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
        Glfw.WindowHint(Hint.ContextVersionMajor, 3);
        Glfw.WindowHint(Hint.ContextVersionMinor, 3);
        Glfw.WindowHint(Hint.Doublebuffer, true);
        Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
    }

    public void Run(params Widget[] widgets)
    {
        foreach (var w in widgets)
        {
            if (w.Parent == null)
            {
                m_topLevelWidgets.Add(w);
                w.InitializeIfTopLevel();
            }
        }

        while (m_topLevelWidgets.Count > 0)
        {
            Glfw.PollEvents();

            foreach (var w in m_topLevelWidgets.ToArray())
            {
                if (w.ShouldClose())
                {
                    w.Shutdown();
                    m_topLevelWidgets.Remove(w);
                }
                else
                {
                    w.UpdateAndRender();
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
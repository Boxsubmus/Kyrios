using GLFW;
using SkiaSharp;

namespace Kyrios.Widgets;

public class Widget
{
    public Widget? Parent { get; private set; }
    public readonly List<Widget> m_children = new();

    public int X = 0;
    public int Y = 0;
    public int Width;
    public int Height;

    protected bool IsHovered { get; private set; } = false;
    private Widget? m_lastHovered = null;

    // If top-level, owns a native window
    public bool IsTopLevel => Parent == null;
    private GLFW.Window glfwWindow;
    private GRContext grContext;
    private SKSurface surface;

    private const uint GL_RGBA8 = 0x8058;

    public Widget(Widget? parent = null)
    {
        Parent = parent;
    }

    public void InitializeIfTopLevel()
    {
        if (!IsTopLevel) return;

        glfwWindow = Glfw.CreateWindow(Width, Height, GetType().Name, GLFW.Monitor.None, GLFW.Window.None);
        Glfw.MakeContextCurrent(glfwWindow);
        Glfw.SwapInterval(1);

        var glInterface = GRGlInterface.Create();
        grContext = GRContext.CreateGl(glInterface);
    }

    public void UpdateAndRender()
    {
        if (!IsTopLevel) return;

        Glfw.MakeContextCurrent(glfwWindow);
        Glfw.GetFramebufferSize(glfwWindow, out int fbWidth, out int fbHeight);

        Glfw.GetCursorPosition(glfwWindow, out double mx, out double my);

        if (fbWidth != Width || fbHeight != Height)
            Resize(fbWidth, fbHeight);

        var fbInfo = new GRGlFramebufferInfo(0, GL_RGBA8);
        var renderTarget = new GRBackendRenderTarget(fbWidth, fbHeight, 0, 8, fbInfo);

        surface?.Dispose();
        surface = SKSurface.Create(grContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);

        var canvas = surface.Canvas;
        canvas.Clear(SKColors.White);

        Paint(canvas);

        canvas.Flush();
        surface.Flush();

        var newHovered = findHoveredWidget((int)mx, (int)my);

        if (m_lastHovered != newHovered)
        {
            m_lastHovered?.handleMouseLeave();
            newHovered?.handleMouseEnter();
        }

        newHovered?.handleMouseMove((int)mx, (int)my);
        m_lastHovered = newHovered;

        Glfw.SwapBuffers(glfwWindow);
    }

    public void Shutdown()
    {
        if (!IsTopLevel) return;

        surface?.Dispose();
        grContext?.Dispose();
        Glfw.DestroyWindow(glfwWindow);
    }

    public void Show()
    {
    }

    public void AddChild(Widget child, int x = 0, int y = 0)
    {
        child.Parent = this;
        child.X = x;
        child.Y = y;
        m_children.Add(child);
    }

    public void RemoveChild(Widget child)
    {
        child.Parent = null;
        m_children.Remove(child);
    }

    public void SetPosition(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public void Resize(int width, int height)
    {
        Width = width;
        Height = height;

        OnResize(width, height);
    }

    public void Paint(SKCanvas canvas)
    {
        canvas.Save();
        canvas.Translate(X, Y);

        OnPaint(canvas);

        foreach (var child in m_children)
        {
            child.Paint(canvas);
        }

        canvas.Restore();
    }

    public bool ShouldClose()
    {
        return IsTopLevel && Glfw.WindowShouldClose(glfwWindow);
    }

    #region Events

    public virtual void OnPaint(SKCanvas canvas)
    {
    }

    public virtual void OnResize(int width, int height)
    {
    }

    public virtual void OnMouseEnter() { }
    public virtual void OnMouseLeave() { }
    public virtual void OnMouseMove(int x, int y) { }

    #endregion

    #region Private Methods

    private Widget? findHoveredWidget(int x, int y)
    {
        if (x < X || y < Y || x > X + Width || y > Y + Height)
            return null;

        foreach (var child in m_children.AsReadOnly().Reverse()) // top to bottom
        {
            var localX = x - child.X;
            var localY = y - child.Y;
            var hit = child.findHoveredWidget(x, y);
            if (hit != null)
                return hit;
        }

        return this;
    }

    private void handleMouseEnter()
    {
        if (!IsHovered)
        {
            IsHovered = true;
            OnMouseEnter();
        }
    }

    private void handleMouseLeave()
    {
        if (IsHovered)
        {
            IsHovered = false;
            OnMouseLeave();
        }
    }

    private void handleMouseMove(int x, int y)
    {
        OnMouseMove(x - X, y - Y);
    }

    #endregion
}
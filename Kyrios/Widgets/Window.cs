using GLFW;
using SkiaSharp;

namespace Kyrios.Widgets;

public class Window : Widget
{
    public GLFW.Window Handle { get; private set; }
    public GRContext GRContext { get; private set; }

    protected SKSurface surface;
    private const uint GL_RGBA8 = 0x8058;

    public string Title { get; }

    public Window(Widget? parent, int width, int height, string title) : base(parent)
    {
        Width = width;
        Height = height;
        Title = title;
    }

    public void Initialize()
    {
        Handle = Glfw.CreateWindow(Width, Height, Title, GLFW.Monitor.None, GLFW.Window.None);
        Glfw.MakeContextCurrent(Handle);
        Glfw.SwapInterval(1);

        var glInterface = GRGlInterface.Create();
        GRContext = GRContext.CreateGl(glInterface);

        OnInitialize();
    }

    public void UpdateAndRender()
    {
        Glfw.MakeContextCurrent(Handle);
        Glfw.GetFramebufferSize(Handle, out int fbWidth, out int fbHeight);

        if (fbWidth != Width || fbHeight != Height)
            Resize(fbWidth, fbHeight);

        var fbInfo = new GRGlFramebufferInfo(0, GL_RGBA8);
        var renderTarget = new GRBackendRenderTarget(fbWidth, fbHeight, 0, 8, fbInfo);

        surface?.Dispose();
        surface = SKSurface.Create(GRContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);

        var canvas = surface.Canvas;
        canvas.Clear(SKColors.White);

        Paint(canvas);

        surface.Canvas.Flush();
        surface.Flush();

        Glfw.SwapBuffers(Handle);
    }

    public bool ShouldClose() => Glfw.WindowShouldClose(Handle);

    public void Shutdown()
    {
        OnDestroy();
        surface?.Dispose();
        GRContext.Dispose();
        Glfw.DestroyWindow(Handle);
    }

    protected virtual void OnInitialize()
    {
    }

    protected virtual void OnDestroy()
    {
    }
}
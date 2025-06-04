using GLFW;
using SkiaSharp;

namespace Kyrios.Platform.Skia;

internal class SkiaWindow
{
    private const uint GL_RGBA8 = 0x8058;

    public readonly Window GLFWWindow;
    public readonly GRContext GRContext;
    public SKSurface Surface { get; private set; }

    public SkiaWindow(int width, int height, string title)
    {
        GLFWWindow = Glfw.CreateWindow(width, height, title, GLFW.Monitor.None, Window.None);
        Glfw.MakeContextCurrent(GLFWWindow);
        Glfw.SwapInterval(1);

        GRContext = SkiaHelper.GenerateSkiaContext(GLFWWindow);

        CreateFrameBuffer(width, height);
    }

    public void CreateFrameBuffer(int w, int h)
    {
        var fbInfo = new GRGlFramebufferInfo(0, GL_RGBA8);
        using var renderTarget = new GRBackendRenderTarget(w, h, 0, 8, fbInfo);

        Surface?.Dispose();
        Surface = SKSurface.Create(GRContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
    }

    public void Dispose()
    {
        Surface?.Dispose();
        GRContext?.Dispose();

        Glfw.DestroyWindow(GLFWWindow);
    }
}
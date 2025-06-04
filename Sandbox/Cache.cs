using SkiaSharp;

namespace Sandbox;

public static class Cache
{
    public static SKTypeface DefaultTypeFace { get; private set; }

    public static void Init()
    {
        DefaultTypeFace = SKTypeface.FromFamilyName("Segoe UI");
    }
}
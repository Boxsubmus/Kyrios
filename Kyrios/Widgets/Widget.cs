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

    public Widget(Widget? parent)
    {
        Parent = parent;
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

    public virtual void OnPaint(SKCanvas canvas)
    {
    }

    public virtual void OnResize(int width, int height)
    {
    }
}
using System.Collections;

public class Sprite : GameObject
{
    public Color Color = Color.WHITE;

    public virtual void Render() { }
}
public class Polygon : Sprite
{
    public Vector2 Size = new(1);
    public Vector2[] Vertices = Array.Empty<Vector2>();
    public int[] Indices = Array.Empty<int>();

    public Vector2 GetTransformedVertex(Vector2 vertex, Vector2 size, float rotation, Vector2 position)
    {
        return RotateDirection(vertex * size, DEG2RAD * rotation) * new Vector2(1, -1) + position;
    }
}
public class LineSprite : Polygon
{
    public float Thickness = 1;

    public override void Render()
    {
        var position = ScreenPosition;
        var rotation = Rotation;

        for (var i = 0; i < Indices.Length; i += 2)
        {
            var vertex = GetTransformedVertex(Vertices[Indices[i]], Size, rotation, position);
            var vertex1 = GetTransformedVertex(Vertices[Indices[i + 1]], Size, rotation, position);

            DrawLineEx(vertex, vertex1, Thickness, Color);
        }
        base.Render();
    }
}
public class TextSprite : Sprite
{
    public Font Font = GetFontDefault();
    public Vector2 Origin;
    public float Spacing = 2;
    public int Size = 12;
    public string Text = "";

    public override void Render()
    {
        var position = ScreenPosition;

        DrawTextPro(Font, Text, ScreenPosition, Origin, Rotation, Size, Spacing, Color);
        base.Render();
    }
}
public class CircleSprite : Sprite
{
    public float Radius = 1;

    public override void Render()
    {
        DrawCircleV(ScreenPosition, Radius, Color);
        base.Render();
    }
}
public class RectangleSprite : Sprite
{
    public Vector2 Origin = new(0.5f);
    public Vector2 Size = new(1);
    public Rectangle Rectangle => new()
    {
        width = Size.X,
        height = Size.Y,
        x = Position.X,
        y = Position.Y
    };

    public override void Render()
    {
        DrawRectanglePro(
            Rectangle,
            Origin,
            Rotation,
            Color
        );
        base.Render();
    }
}
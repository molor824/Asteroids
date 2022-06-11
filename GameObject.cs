using System.Collections.ObjectModel;

public class GameObject
{
    public string Name = "";
    public bool Enabled = true;
    public Vector2 Position
    {
        get
        {
            var position = LocalPosition;
            var parent = Parent;

            while (parent != null)
            {
                position = RotateDirection(position, parent.LocalRotation * DEG2RAD);
                position += parent.LocalPosition;
                parent = parent.Parent;
            }

            return position;
        }
        set
        {
            var parent = Parent;

            while (parent != null)
            {
                value -= parent.LocalPosition;
                value = RotateDirection(value, -parent.LocalRotation * DEG2RAD);
                parent = parent.Parent;
            }

            LocalPosition = value;
        }
    }
    public float Rotation
    {
        get
        {
            var rotation = LocalRotation;
            var parent = Parent;

            while (parent != null)
            {
                rotation += parent.LocalRotation;
                parent = parent.Parent;
            }

            return rotation;
        }
        set
        {
            var parent = Parent;

            while (parent != null)
            {
                value -= parent.LocalRotation;
                parent = parent.Parent;
            }

            LocalRotation = value;
        }
    }
    public Vector2 LocalPosition;
    public float LocalRotation;
    public Game Game = null!;
    public GameObject? Parent
    {
        get => _parent;
        set
        {
            if (_parent == value) { return; }

            if (_parent != null) { _parent!._children.Remove(this); }
            if (value != null) { value._children.Add(this); }

            _parent = value;
        }
    }
    public Vector2 ScreenPosition
    {
        get => Position * new Vector2(1, -1) + ScreenCenter;
        set { Position = (value - ScreenCenter) * new Vector2(1, -1); }
    }
    public Vector2 Up => RotateDirection(new(0, 1), Rotation * DEG2RAD);
    public Vector2 Forward => RotateDirection(new(1, 0), Rotation * DEG2RAD);
    public Vector2 ScreenSize => new(GetScreenWidth(), GetScreenHeight());
    public Vector2 ScreenCenter => ScreenSize / 2;
    public ReadOnlyCollection<GameObject> Children => _children.AsReadOnly();

    GameObject? _parent;
    List<GameObject> _children = new();

    public T? GetChild<T>() where T : GameObject
    {
        foreach (var child in _children)
        {
            if (child is T t) { return t; }
        }

        return null;
    }
    public Vector2 RotateDirection(Vector2 direction, float radian)
    {
        var s = MathF.Sin(radian);
        var c = MathF.Cos(radian);

        return new(
            direction.X * c - direction.Y * s,
            direction.X * s + direction.Y * c
        );
    }
    public static void Print(params object[] objects)
    {
        foreach (var obj in objects)
        {
            Console.Write(obj + " ");
        }

        Console.WriteLine();
    }
    public virtual void Start() { }
    public virtual void Update(float delta) { }
    public virtual void Close() { }
}
using System.Collections.ObjectModel;

enum GameObjectState
{
    Added,
    Removed
}
public class Game
{
    public ReadOnlyCollection<GameObject> Objects => _objects.AsReadOnly();
    public int ObjectCount => _objects.Count;

    List<GameObject> _objects = new(0x100);
    List<(GameObject, GameObjectState)> _newObjects = new();
    Renderer _renderer;

    public Game()
    {
        _renderer = new(_objects);
    }

    public void RemoveAllObjects()
    {
        _newObjects.Clear();

        foreach (var obj in _objects)
        {
            _newObjects.Add((obj, GameObjectState.Removed));
        }
    }
    public T? GetObject<T>() where T : GameObject
    {
        foreach (var obj in _objects)
        {
            if (obj is T t) { return t; }
        }
        return null;
    }
    void _AddObject(GameObject obj, bool addDirectly)
    {
        var children = obj.Children;

        for (var i = 0; i < children.Count; i++)
        {
            AddObject(children[i]);
        }

        if (addDirectly) { _objects.Add(obj); }
        else { _newObjects.Add((obj, GameObjectState.Added)); }
    }
    public void AddObjectDirectly(GameObject obj)
    {
        obj.Game = this;
        _AddObject(obj, true);
    }
    public void AddObjectsDirectly(params GameObject[] objs)
    {
        foreach (var obj in objs) { AddObjectDirectly(obj); }
    }
    public void AddObject(GameObject obj) { _AddObject(obj, false); }
    public void AddObjects(params GameObject[] objs)
    {
        foreach (var obj in objs) { _AddObject(obj, false); }
    }
    public void RemoveObjects(params GameObject[] objs)
    {
        foreach (var obj in objs) { RemoveObject(obj); }
    }
    public void RemoveObject(GameObject obj)
    {
        foreach (var child in obj.Children)
        {
            RemoveObject(child);
        }

        _newObjects.Add((obj, GameObjectState.Removed));
    }
    void SyncObjects()
    {
        for (var i = 0; i < _newObjects.Count; i++)
        {
            var (obj, state) = _newObjects[i];

            if (state == GameObjectState.Added)
            {
                _objects.Add(obj);
                obj.Game = this;

                if (obj.Enabled) { obj.Start(); }

                // Console.WriteLine(
                //     $"Object added\nHash: {obj.GetHashCode()}\nType: {obj.GetType()}\n"
                // );
            }
            else
            {
                if (obj.Enabled) { obj.Close(); }
                _objects.Remove(obj);

                obj.Game = null!;
                obj.Parent = null;

                // Console.WriteLine(
                //     $"Object removed\nHash: {obj.GetHashCode()}\nType: {obj.GetType()}\n"
                // );
            }
        }

        _newObjects.Clear();
    }
    void Start()
    {
        foreach (var obj in _objects) { if (obj.Enabled) { obj.Start(); } }
    }
    void Update(float delta)
    {
        foreach (var obj in _objects) { if (obj.Enabled) { obj.Update(delta); } }
    }
    void Close()
    {
        foreach (var obj in _objects) { if (obj.Enabled) { obj.Close(); } }
    }
    public void Run(string name, int width, int height)
    {
        InitWindow(width, height, name);
        SetTargetFPS(60);

        AddObjectsDirectly(Program.InitialObjects);

        Start();

        while (!WindowShouldClose())
        {
            var delta = GetFrameTime();

            SyncObjects();
            Update(delta);

            BeginDrawing();
            _renderer.Render();
            EndDrawing();
        }

        SyncObjects();
        foreach (var obj in _objects)
        {
            if (obj.Enabled) { obj.Close(); }
        }

        CloseWindow();
    }
}
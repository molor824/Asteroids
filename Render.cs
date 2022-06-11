public class Renderer
{
    List<GameObject> _objects;

    public Renderer(List<GameObject> objects)
    {
        _objects = objects;
    }
    public void Render()
    {
        ClearBackground(Color.BLACK);

        foreach (var obj in _objects)
        {
            if (obj is Sprite sprite && sprite.Enabled)
            {
                sprite.Render();
            }
        }
    }
}
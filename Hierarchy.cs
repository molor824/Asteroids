public class Hierarchy : TextSprite // only used in debug mode
{
    public override void Start()
    {
        ScreenPosition = new();

        base.Start();
    }
    public override void Update(float delta)
    {
        var str = "";

        foreach (var obj in Game.Objects)
        {
            if (obj.Parent != null) continue;
            str += ObjectHierarchy(obj);
        }

        Text = str;

        base.Update(delta);
    }
    string ObjectHierarchy(GameObject obj, int level = 0)
    {
        var str = "";

        for (var i = 0; i < level; i++) { str += "> "; }

        str += obj.GetType();
        str += '\n';

        foreach (var child in obj.Children)
        {
            str += ObjectHierarchy(child, level + 1);
        }

        return str;
    }
}
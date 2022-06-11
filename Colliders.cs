using System.Collections.ObjectModel;

public class CircleCollider : GameObject
{
    public ReadOnlyCollection<CircleCollider> CollidedObjects => _collidedObjects.AsReadOnly();
    public bool IsCollided => _collidedObjects.Count != 0;
    public float Radius = 1;
    public bool Trigger { get; private set; }
    public event Action<CircleCollider>? OnCollisionEnter;
    public event Action<CircleCollider>? OnCollisionExit;

    List<CircleCollider> _collidedObjects = new();

    public bool Intersect(CircleCollider collider)
    {
        float dist = Vector2.DistanceSquared(Position, collider.Position);
        float totalRadius = Radius + collider.Radius;

        return dist <= totalRadius * totalRadius;
    }
    public override void Update(float delta)
    {
        foreach (var obj in Game.Objects)
        {
            if (obj != this && obj is CircleCollider collider)
            {
                if (Intersect(collider))
                {
                    if (!_collidedObjects.Contains(collider))
                    {
                        _collidedObjects.Add(collider);
                        OnCollisionEnter?.Invoke(collider);
                    }
                }
                // if remove() returns true, it means it was removed
                // invoke oncollisionexit
                else if (_collidedObjects.Remove(collider) && OnCollisionExit != null)
                {
                    OnCollisionExit.Invoke(collider);
                }
            }
        }

        base.Update(delta);
    }
}
public class RigidBody : GameObject
{
    public Vector2 LinearVelocity;
    public float AngularVelocity;

    public override void Update(float delta)
    {
        LocalPosition += LinearVelocity * delta;
        LocalRotation += AngularVelocity * delta;

        base.Update(delta);
    }
}
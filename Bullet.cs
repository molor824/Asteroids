public class Bullet : RigidBody
{
    public override void Start()
    {
        Game.AddObjects(
            new CircleSprite()
            {
                Radius = 1,
                Parent = this
            },
            new CircleCollider()
            {
                Radius = 1,
                Parent = this
            }
        );

        base.Start();
    }
    public override void Update(float delta)
    {
        var position = Position;
        var screenCenter = ScreenCenter;

        if (
            MathF.Abs(position.X) > screenCenter.X
            || MathF.Abs(position.Y) > screenCenter.Y
        ) { Game.RemoveObject(this); }

        base.Update(delta);
    }
}
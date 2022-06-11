public class PlayerMovement : RigidBody
{
    static readonly float PlayerSize = 10;
    static readonly float BulletSpeed = 500;
    static readonly float RotateSpeed = 270;
    static readonly float MaxSpeed = 200;
    static readonly float Acceleration = 150;
    LineSprite _sprite = new()
    {
        Vertices = new Vector2[]
        {
            new(1, 0),
            new(-1),
            new(-0.5f, 0),
            new(-1, 1)
        },
        Indices = new int[] { 0, 1, 1, 2, 2, 3, 3, 0 },
        Size = new Vector2(PlayerSize),
        Color = new Color(255, 255, 255, 255 / 2),
        Thickness = 2
    };
    LineSprite _fire = new()
    {
        Vertices = new Vector2[]
        {
            new(),
            new(-0.2f, 0.4f),
            new(-0.2f, -0.4f),
            new()
        },
        Indices = new int[] { 0, 1, 0, 2, 1, 3, 2, 3 },
        Position = new Vector2(-8, 0),
        Size = new Vector2(PlayerSize),
        Color = new Color(255, 255 / 2, 0, 255 / 2),
        Enabled = false,
        Thickness = 2
    };

    public override void Start()
    {
        Game.AddObjects(
            _sprite, _fire,
            new CircleCollider()
            {
                Radius = new Vector2(PlayerSize).Length(),
                Parent = this
            }
        );
        _sprite.Parent = this;
        _fire.Parent = this;

        base.Start();
    }
    public override void Update(float delta)
    {
        var rotateDirection = 0f;
        var screenCenter = ScreenCenter;
        var velocity = LinearVelocity;
        var position = Position;
        var forward = Forward;
        var keyUp = IsKeyDown(KeyboardKey.KEY_UP);

        if (IsKeyDown(KeyboardKey.KEY_RIGHT)) { rotateDirection--; }
        if (IsKeyDown(KeyboardKey.KEY_LEFT)) { rotateDirection++; }

        if (keyUp) { velocity += forward * Acceleration * delta; }
        if (IsKeyPressed(KeyboardKey.KEY_SPACE)) Game.AddObject(new Bullet()
        {
            Position = position,
            LinearVelocity = forward * BulletSpeed
        });

        if (velocity.LengthSquared() > MaxSpeed * MaxSpeed)
        {
            velocity = Vector2.Normalize(velocity) * MaxSpeed;
        }

        if (MathF.Abs(position.X) > screenCenter.X)
        {
            position.X -= MathF.Sign(position.X) * screenCenter.X * 2;
        }
        if (MathF.Abs(position.Y) > screenCenter.Y)
        {
            position.Y -= MathF.Sign(position.Y) * screenCenter.Y * 2;
        }

        AngularVelocity = rotateDirection * RotateSpeed;
        LinearVelocity = velocity;
        Position = position;
        _fire.Enabled = keyUp;
        _fire.Vertices[3] = new(MathF.Sin((float)GetTime() * 50) * 0.1f - 1, 0);

        base.Update(delta);
    }
}
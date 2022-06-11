public class Rock : RigidBody
{
    public static readonly float Speed = 40;
    static readonly float[] RockSizes = { 40, 20, 10 };
    ScoreText _score = null!;
    public int RockType
    {
        get => _rockType;
        set
        {
            _lineSprite.Vertices = Rocks[value].Vertices;
            _lineSprite.Indices = Rocks[value].Indices;
            _rockType = value;
        }
    }
    public int RockSize
    {
        get => _rockSize;
        set
        {
            _lineSprite.Size = new(RockSizes[value]);
            Collider.Radius = _lineSprite.Size.Length();
            _rockSize = value;
        }
    }
    public CircleCollider Collider;
    LineSprite _lineSprite = new()
    {
        Thickness = 2,
        Color = new Color(255, 255, 255, 255 / 2)
    };
    int _rockSize = 0;
    int _rockType = 0;

    void BreakRock(CircleCollider collider)
    {
        if (collider.Parent is not Bullet bullet) { return; }

        _score.Score++;
        Game.RemoveObject(bullet);

        var rng = Random.Shared;

        if (_rockSize == RockSizes.Length - 1)
        {
            Game.RemoveObjects(
                this,
                Collider,
                _lineSprite
            );
            return;
        }

        RockSize++;

        var direction = rng.Next(360);

        LinearVelocity = RotateDirection(new(Speed, 0), direction * DEG2RAD);

        direction += rng.Next(90, 271);

        Game.AddObject(new Rock()
        {
            RockSize = _rockSize,
            RockType = rng.Next(0, Rocks.Length),
            Position = Position,
            LinearVelocity = RotateDirection(new(Speed, 0), direction * DEG2RAD),
        });
    }
    public Rock()
    {
        _lineSprite.Parent = this;
        Collider = new() { Parent = this };

        RockSize = _rockSize;
        RockType = _rockType;
    }
    public override void Start()
    {
        _score = Game.GetObject<ScoreText>()!;

        Collider.OnCollisionEnter += BreakRock;

        Game.AddObjects(_lineSprite, Collider);

        base.Start();
    }
    public override void Update(float delta)
    {
        var position = Position;
        var screenCenter = ScreenCenter;

        if (MathF.Abs(position.X) > screenCenter.X)
        {
            position.X -= MathF.Sign(position.X) * screenCenter.X * 2;
        }
        if (MathF.Abs(position.Y) > screenCenter.Y)
        {
            position.Y -= MathF.Sign(position.Y) * screenCenter.Y * 2;
        }

        Position = position;

        base.Update(delta);
    }
    public static readonly Polygon[] Rocks = {
        new()
        {
            Vertices = new Vector2[]
            {
                new(-0.41f, 0.92f),
                new(0.43f, 0.98f),
                new(0.412f, 0.394f),
                new(0.98f, 0.51f),
                new(0.99f, -0.34f),
                new(0.41f, -0.9f),
                new(-0.54f, -1.12f),
                new(-0.41f, -0.43f),
                new(-1f, -0.41f),
                new(-0.78f, 0.44f)
            },
            Indices = new int[] { 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 0 }
        },
        new()
        {
            Vertices = new Vector2[]
            {
                new(-0.414f, 0.912f),
                new(0.52f, 0.829f),
                new(0.99f, 0.64f),
                new(0.82f, -0.53f),
                new(0.43f, -0.92f),
                new(-0.445f, -0.992f),
                new(-0.324f, -0.53f),
                new(-0.75f, -0.96f),
                new(-0.942f, -0.585f),
                new(-0.412f, -0.323f),
                new(-0.895f, -0.374f),
                new(-0.76f, 0.532f),
                new(-0.43f, 0.44f)
            },
            Indices = new int[]
            {
                0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 0
            }
        },
        new()
        {
            Vertices = new Vector2[]
            {
                new(-0.43f, 0.92f),
                new(0.1f, 0.64f),
                new(0.32f, 0.9f),
                new(0.98f, 0.46f),
                new(0.52f, -0.12f),
                new(0.96f, -0.38f),
                new(0.45f, -1),
                new(0.02f, -0.61f),
                new(-0.445f, -0.992f),
                new(-0.87f, -0.51f),
                new(-0.35f, 0.1f),
                new(-1, 0.4f)
            },
            Indices = new int[]
            {
                0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 0
            }
        }
    };
}
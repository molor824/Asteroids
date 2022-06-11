public class RockSpawner : GameObject
{
    CircleCollider _playerCollider = null!;

    void SpawnRocks()
    {
        var rng = System.Random.Shared;
        var minRange = 100;
        var maxRange = 1000;

        for (var i = 0; i < 8; i++)
        {
            var rock = new Rock()
            {
                Position = RotateDirection(
                    new(rng.Next(minRange, maxRange), 0), rng.Next(360) * DEG2RAD
                ),
                LinearVelocity = RotateDirection(new(Rock.Speed, 0), rng.Next(360) * DEG2RAD),
                RockType = rng.Next(0, Rock.Rocks.Length)
            };

            Game.AddObject(rock);
        }
    }
    public override void Start()
    {
        _playerCollider = Game.GetObject<PlayerMovement>()!.GetChild<CircleCollider>()!;

        SpawnRocks();

        base.Start();
    }
    public override void Update(float delta)
    {
        foreach (var obj in Game.Objects)
        {
            if (obj is Rock) { return; }
        }

        var player = (PlayerMovement)_playerCollider.Parent!;

        player.LocalPosition = new Vector2();
        player.LinearVelocity = new Vector2();
        player.LocalRotation = 0;
        player.AngularVelocity = 0;

        SpawnRocks();

        base.Update(delta);
    }
}
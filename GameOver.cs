public class GameOver : TextSprite
{
    bool _restarted = true;
    int _highScore;
    string _filePath = "high_score";

    void GameOverCollision(CircleCollider collider)
    {
        if (collider.Parent is not Rock) return;

        var score = Game.GetObject<ScoreText>()!.Score;

        foreach (var obj in Game.Objects)
        {
            if (obj == this) continue;

            Game.RemoveObject(obj);
        }

        Text = $"Game Over!\nScore: {score}\n";

        if (_highScore >= score) { Text += $"High Score: {_highScore}"; }
        else
        {
            _highScore = score;
            Text += "New High Score!";
        }
    }
    public override void Start()
    {
        Origin = new(0.5f);
        Size = 30;

        if (File.Exists(_filePath) && int.TryParse(File.ReadAllText(_filePath), out var score))
        {
            _highScore = score;
        }

        base.Start();
    }
    public override void Update(float delta)
    {
        if (_restarted)
        {
            _restarted = false;

            var collider = Game.GetObject<PlayerMovement>()!.GetChild<CircleCollider>()!;
            collider.OnCollisionEnter += GameOverCollision;
        }
        if (Text.Length == 0) return;
        if (IsKeyDown(KeyboardKey.KEY_ENTER))
        {
            var initialObjs = Program.InitialObjects;
            foreach (var obj in initialObjs)
            {
                if (obj is not GameOver) { Game.AddObject(obj); }
            }

            Text = "";
            _restarted = true;
            // we need to wait until next update, so that we can 
        }
        if (IsKeyDown(KeyboardKey.KEY_ESCAPE))
        {
            CloseWindow();
        }

        base.Update(delta);
    }
    public override void Close()
    {
        File.WriteAllText(_filePath, _highScore.ToString());

        base.Close();
    }
}
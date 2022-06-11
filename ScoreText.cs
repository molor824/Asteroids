public class ScoreText : TextSprite
{
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            Text = $"Score: {value}";
        }
    }
    int _score;

    public override void Start()
    {
        Score = _score;
        Size = 20;
        ScreenPosition = new(10);

        base.Start();
    }
}
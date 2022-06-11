global using Raylib_cs;
global using static Raylib_cs.Raylib;
global using System.Numerics;

public class Program
{
    public static GameObject[] InitialObjects => new GameObject[]
    {
        new PlayerMovement(),
        new RockSpawner(),
        new GameOver(),
        new ScoreText(),
        // new Hierarchy() // was used for debugging
    };
    static void Main()
    {
        var game = new Game();
        game.Run("Asteroids", 800, 600);
    }
}
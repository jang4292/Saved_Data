
public class GameState
{
    public enum Game
    {
        BaseBall,
        Escape
    }

    public enum Level
    {
        Easy,
        Nomal,
        Hard
    }

    public enum Direction
    {
        Right,
        Left
    }

    public enum Playing
    {
        Pause,
        Playing
    }

    public static Game gameState = Game.BaseBall;
    public static Level levelState = Level.Easy;
    public static Direction directionState = Direction.Left;
    public static Playing playingState = Playing.Pause;
}

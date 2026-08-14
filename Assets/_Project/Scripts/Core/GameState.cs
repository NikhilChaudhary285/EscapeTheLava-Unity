namespace EscapeTheLava.Core
{
    /// <summary>
    /// Represents the overall flow of a single round.
    /// GameManager is the only script allowed to change this value.
    /// </summary>
    public enum GameState
    {
        Ready,   // Board is set up, waiting for the round to start
        Playing, // Timer running, input active
        Won,     // All diamonds collected before timer expired
        Lost     // Timer hit 0 OR lives hit 0
    }
}
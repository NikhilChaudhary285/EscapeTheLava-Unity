namespace EscapeTheLava.Grid
{
    public enum TileType
    {
        Island, // safe, no effect
        Diamond, // collectible
        Lava    // danger
    }

    public enum TileState
    {
        Active,   // can still be interacted with
        Consumed  // already collected/hit — ignores further input
    }
}
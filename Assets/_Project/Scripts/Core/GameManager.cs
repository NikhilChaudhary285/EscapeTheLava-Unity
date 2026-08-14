using UnityEngine;
using EscapeTheLava.Core;
using EscapeTheLava.Grid;
using EscapeTheLava.Systems;

namespace EscapeTheLava.Core
{
    /// <summary>
    /// Single source of truth for the round's state.
    /// Other systems (Timer, Lives, Score, Grid) raise events;
    /// GameManager listens and decides Win/Lose — nothing else does.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public GameState CurrentState { get; private set; } = GameState.Ready;

        [SerializeField] private ScoreController scoreController;
        [SerializeField] private LivesController livesController;
        [SerializeField] private GridManager gridManager;

        private void Awake()
        {
            // Simple singleton — fine for a project this size, no need for DI here.
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            scoreController.Initialize(gridManager.TotalDiamonds);
            ScoreController.OnAllDiamondsCollected += WinRound;
            LivesController.OnLivesDepleted += LoseRound;

            StartRound();
        }

        public void StartRound()
        {
            CurrentState = GameState.Playing;
            Debug.Log("Round started. State = Playing");
        }

        public void WinRound()
        {
            if (CurrentState != GameState.Playing) return; // prevents double-trigger
            CurrentState = GameState.Won;
            Debug.Log("Round WON. State = Won");
        }

        public void LoseRound()
        {
            if (CurrentState != GameState.Playing) return; // prevents double-trigger
            CurrentState = GameState.Lost;
            Debug.Log("Round LOST. State = Lost");
        }

        public bool IsInputAllowed()
        {
            return CurrentState == GameState.Playing;
        }

        public void HandleTileTapped(Tile tile)
        {
            if (!IsInputAllowed()) return;               // blocks input after Win/Lose
            if (tile.State != TileState.Active) return;   // blocks double-fire on same tile

            tile.Consume(); // mark this tile as used — it can never fire again

            switch (tile.Type)
            {
                case TileType.Diamond:
                    scoreController.CollectDiamond();
                    break;
                case TileType.Lava:
                    livesController.LoseLife();
                    break;
                case TileType.Island:
                    // intentionally no effect
                    break;
            }
        }

        private void OnDestroy()
        {
            ScoreController.OnAllDiamondsCollected -= WinRound;
            LivesController.OnLivesDepleted -= LoseRound;
        }
    }
}
using System;
using UnityEngine;

namespace EscapeTheLava.Grid
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Tile : MonoBehaviour
    {
        public TileType Type { get; private set; }
        public TileState State { get; private set; } = TileState.Active;
        public int Row { get; private set; }
        public int Col { get; private set; }

        // GridManager subscribes to this to react to taps (collect / damage / ignore)
        public static event Action<Tile> OnTileTapped;

        private SpriteRenderer _renderer;

        // Placeholder colors until real art is in — swapped out during the polish pass
        private static readonly Color IslandColor = new Color(0.29f, 0.75f, 0.35f);  // green
        private static readonly Color DiamondColor = new Color(0.25f, 0.55f, 0.95f); // blue
        private static readonly Color LavaColor = new Color(0.85f, 0.2f, 0.15f);     // red

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        public void Initialize(TileType type, int row, int col)
        {
            Type = type;
            Row = row;
            Col = col;
            State = TileState.Active;
            _renderer.color = GetColorForType(type);
            gameObject.name = $"Tile_{type}_{row}_{col}";
        }

        private static Color GetColorForType(TileType type)
        {
            return type switch
            {
                TileType.Island => IslandColor,
                TileType.Diamond => DiamondColor,
                TileType.Lava => LavaColor,
                _ => Color.white
            };
        }

        public void Consume()
        {
            State = TileState.Consumed;
        }

        // Works for both mouse clicks and touch taps in the Editor/on-device.
        private void OnMouseDown()
        {
            OnTileTapped?.Invoke(this);
        }
    }
}
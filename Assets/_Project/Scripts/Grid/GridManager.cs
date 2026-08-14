using EscapeTheLava.Core;
using System.Collections.Generic;
using UnityEngine;

namespace EscapeTheLava.Grid
{
    public class GridManager : MonoBehaviour
    {
        [Header("Grid Size — per spec, do not change")]
        [SerializeField] private int rows = 16;
        [SerializeField] private int cols = 8;

        [Header("Layout")]
        [SerializeField] private float tileSpacing = 1.0f;
        [SerializeField] private Tile tilePrefab;

        [Header("Tile Distribution (must sum sensibly, Diamonds > 0)")]
        [Range(0f, 1f)][SerializeField] private float diamondChance = 0.25f;
        [Range(0f, 1f)][SerializeField] private float lavaChance = 0.35f;
        // remaining chance becomes Island

        public Tile[,] Tiles { get; private set; }
        public int TotalDiamonds { get; private set; }

        private void Awake()
        {
            BuildGrid();
        }

        private void OnEnable()
        {
            Tile.OnTileTapped += HandleTileTapped;
        }

        private void BuildGrid()
        {
            Tiles = new Tile[rows, cols];
            TotalDiamonds = 0;

            // Center the grid around this object's position
            float offsetX = (cols - 1) * tileSpacing * 0.5f;
            float offsetY = (rows - 1) * tileSpacing * 0.5f;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Vector3 pos = new Vector3(
                        c * tileSpacing - offsetX,
                        r * tileSpacing - offsetY,
                        0f) + transform.position;

                    Tile tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                    TileType type = RollTileType();
                    tile.Initialize(type, r, c);

                    if (type == TileType.Diamond)
                        TotalDiamonds++;

                    Tiles[r, c] = tile;
                }
            }

            // Safety net: spec requires diamonds to exist to be collectible.
            // If bad luck rolls zero diamonds, force one so the round is always winnable.
            if (TotalDiamonds == 0)
            {
                Tile fallback = Tiles[0, 0];
                fallback.Initialize(TileType.Diamond, 0, 0);
                TotalDiamonds = 1;
            }

            Debug.Log($"Grid built: {rows}x{cols} = {rows * cols} tiles, {TotalDiamonds} diamonds.");
        }

        private TileType RollTileType()
        {
            float roll = Random.value;
            if (roll < diamondChance) return TileType.Diamond;
            if (roll < diamondChance + lavaChance) return TileType.Lava;
            return TileType.Island;
        }

        private void HandleTileTapped(Tile tile, Vector2 screenPosition)
        {
            GameManager.Instance.HandleTileTapped(tile, screenPosition);
        }

        private void OnDisable()
        {
            Tile.OnTileTapped -= HandleTileTapped;
        }
    }
}
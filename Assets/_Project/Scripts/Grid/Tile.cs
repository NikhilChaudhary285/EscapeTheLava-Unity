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
        public static event Action<Tile, Vector2> OnTileTapped;

        private SpriteRenderer _renderer;

        // Placeholder colors until real art is in — swapped out during the polish pass
        private static readonly Color IslandColor = new Color(0.29f, 0.75f, 0.35f);  // green
        private static readonly Color DiamondColor = new Color(0.25f, 0.55f, 0.95f); // blue
        private static readonly Color LavaColor = new Color(0.85f, 0.2f, 0.15f);     // red

        // Idle Animation Tuning
        [Header("Idle Animation Tuning")]
        [SerializeField] private float diamondFloatAmplitude = 0.08f;
        [SerializeField] private float diamondFloatSpeed = 2.5f;
        [SerializeField] private float lavaGlowSpeed = 1.8f;
        [SerializeField] private float lavaGlowMinBrightness = 0.75f;

        private Vector3 _baseLocalPosition;
        private Color _baseColor;
        private float _idleTimeOffset; // staggers tiles so they don't all pulse in perfect sync

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

            _baseLocalPosition = transform.localPosition;
            _baseColor = _renderer.color;
            _idleTimeOffset = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
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
            transform.localPosition = _baseLocalPosition; // snap back to rest, idle anim now stops
            _renderer.color = _baseColor;                 // reset to base color, no leftover glow tint
        }

        // Works for both mouse clicks and touch taps in the Editor/on-device.
        private void OnMouseDown()
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            OnTileTapped?.Invoke(this, screenPos);
        }

        private void Update()
        {
            if (State != TileState.Active) return; // consumed tiles freeze — no idle anim on used tiles

            switch (Type)
            {
                case TileType.Diamond:
                    AnimateDiamondFloat();
                    break;
                case TileType.Lava:
                    AnimateLavaGlow();
                    break;
                    // Island intentionally has no idle animation — stays visually calm/static.
            }
        }

        private void AnimateDiamondFloat()
        {
            float yOffset = Mathf.Sin(Time.time * diamondFloatSpeed + _idleTimeOffset) * diamondFloatAmplitude;
            transform.localPosition = _baseLocalPosition + new Vector3(0f, yOffset, 0f);
        }

        private void AnimateLavaGlow()
        {
            float t = Mathf.PingPong(Time.time * lavaGlowSpeed + _idleTimeOffset, 1f);
            float brightness = Mathf.Lerp(lavaGlowMinBrightness, 1f, t);
            _renderer.color = new Color(
                _baseColor.r * brightness,
                _baseColor.g * brightness,
                _baseColor.b * brightness,
                _baseColor.a);
        }
    }
}
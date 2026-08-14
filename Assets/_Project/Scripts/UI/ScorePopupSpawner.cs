using UnityEngine;

namespace EscapeTheLava.UI
{
    public class ScorePopupSpawner : MonoBehaviour
    {
        [SerializeField] private ScorePopup popupPrefab;
        [SerializeField] private Canvas parentCanvas;

        private static readonly Color DiamondColor = new Color(0.35f, 0.85f, 0.45f); // green, positive
        private static readonly Color LavaColor = new Color(0.95f, 0.3f, 0.25f);      // red, negative

        public void SpawnDiamondPopup(Vector2 screenPosition)
        {
            Spawn("+1", DiamondColor, screenPosition);
        }

        public void SpawnLavaPopup(Vector2 screenPosition)
        {
            Spawn("-1", LavaColor, screenPosition);
        }

        private void Spawn(string message, Color color, Vector2 screenPosition)
        {
            ScorePopup popup = Instantiate(popupPrefab, parentCanvas.transform);
            popup.Play(message, color, screenPosition);
        }
    }
}
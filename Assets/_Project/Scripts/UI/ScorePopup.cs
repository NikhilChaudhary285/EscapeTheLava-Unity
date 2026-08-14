using System.Collections;
using TMPro;
using UnityEngine;

namespace EscapeTheLava.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class ScorePopup : MonoBehaviour
    {
        [Header("Animation Tuning")]
        [SerializeField] private float floatDistance = 80f;
        [SerializeField] private float duration = 0.8f;
        [SerializeField] private AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

        private TMP_Text _text;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _rectTransform = GetComponent<RectTransform>();
        }

        /// <summary>
        /// Configures and plays the popup, then destroys itself when done.
        /// screenPosition: the tap position in screen space (from the click event).
        /// </summary>
        public void Play(string message, Color color, Vector2 screenPosition)
        {
            _text.text = message;
            _text.color = color;
            _rectTransform.position = screenPosition;

            StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            Vector3 startPos = _rectTransform.position;
            Vector3 endPos = startPos + new Vector3(0f, floatDistance, 0f);
            Color startColor = _text.color;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                _rectTransform.position = Vector3.LerpUnclamped(startPos, endPos, moveCurve.Evaluate(t));

                Color c = startColor;
                c.a = fadeCurve.Evaluate(t);
                _text.color = c;

                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
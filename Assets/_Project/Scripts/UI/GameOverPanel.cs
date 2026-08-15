using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using EscapeTheLava.Core;
using EscapeTheLava.Systems;

namespace EscapeTheLava.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private TMP_Text finalScoreText;
        [SerializeField] private ScoreController scoreController;

        [Header("Entrance Animation Tuning")]
        [SerializeField] private float animationDuration = 0.5f;
        [SerializeField]
        private AnimationCurve scaleCurve = new AnimationCurve(
            new Keyframe(0f, 0.6f),
            new Keyframe(0.7f, 1.08f),
            new Keyframe(1f, 1f)
        ); // slight overshoot ("pop") then settle — reads as punchy, not robotic
        [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private RectTransform _rectTransform;
        private Coroutine _activeAnimation;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            // GameObject stays ACTIVE the whole time now — Awake/OnEnable are
            // therefore guaranteed to run at scene start, so the subscription
            // below is never missed.
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
            Hide();
        }

        private void OnEnable()
        {
            GameManager.OnGameEnded += HandleGameEnded;
        }

        private void OnDisable()
        {
            GameManager.OnGameEnded -= HandleGameEnded;
        }

        private void HandleGameEnded(GameState finalState)
        {
            resultText.text = finalState == GameState.Won ? "YOU WIN!" : "GAME OVER";
            finalScoreText.text = $"Score: {scoreController.Score}";
            Show();
        }

        private void Show()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            if (_activeAnimation != null) StopCoroutine(_activeAnimation);
            _activeAnimation = StartCoroutine(PlayEntranceAnimation());
        }

        private IEnumerator PlayEntranceAnimation()
        {
            float elapsed = 0f;
            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / animationDuration);

                _rectTransform.localScale = Vector3.one * scaleCurve.Evaluate(t);
                _canvasGroup.alpha = fadeCurve.Evaluate(t);

                yield return null;
            }

            _rectTransform.localScale = Vector3.one;
            _canvasGroup.alpha = 1f;
        }

        private void Hide()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _rectTransform.localScale = Vector3.one * 0.6f; // matches scaleCurve's starting value
        }

        // Hooked up to RestartButton's OnClick in the Inspector
        public void OnRestartPressed()
        {
            Scene current = SceneManager.GetActiveScene();
            SceneManager.LoadScene(current.name);
        }
    }
}
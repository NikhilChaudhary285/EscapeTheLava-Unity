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

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            // GameObject stays ACTIVE the whole time now — Awake/OnEnable are
            // therefore guaranteed to run at scene start, so the subscription
            // below is never missed.
            _canvasGroup = GetComponent<CanvasGroup>();
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
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        private void Hide()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        // Hooked up to RestartButton's OnClick in the Inspector
        public void OnRestartPressed()
        {
            Scene current = SceneManager.GetActiveScene();
            SceneManager.LoadScene(current.name);
        }
    }
}
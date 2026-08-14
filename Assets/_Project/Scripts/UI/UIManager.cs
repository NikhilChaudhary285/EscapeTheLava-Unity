using System.Collections.Generic;
using TMPro;
using UnityEngine;
using EscapeTheLava.Systems;

namespace EscapeTheLava.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Text")]
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text scoreText;

        [Header("Lives")]
        [SerializeField] private Transform livesContainer;
        [SerializeField] private GameObject heartPrefab;
        [SerializeField] private int startingLives = 5;

        private readonly List<GameObject> _hearts = new List<GameObject>();

        private void Awake()
        {
            // Build the 5 heart icons once, up front.
            for (int i = 0; i < startingLives; i++)
            {
                GameObject heart = Instantiate(heartPrefab, livesContainer);
                _hearts.Add(heart);
            }
        }

        private void OnEnable()
        {
            TimerController.OnTimeChanged += HandleTimeChanged;
            ScoreController.OnScoreChanged += HandleScoreChanged;
            LivesController.OnLivesChanged += HandleLivesChanged;
        }

        private void OnDisable()
        {
            TimerController.OnTimeChanged -= HandleTimeChanged;
            ScoreController.OnScoreChanged -= HandleScoreChanged;
            LivesController.OnLivesChanged -= HandleLivesChanged;
        }

        private void HandleTimeChanged(float timeRemaining)
        {
            timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
        }

        private void HandleScoreChanged(int score)
        {
            scoreText.text = $"Score: {score}";
        }

        private void HandleLivesChanged(int livesRemaining)
        {
            // Toggle heart visibility from the end inward — simple, correct, no negative-index risk.
            for (int i = 0; i < _hearts.Count; i++)
            {
                _hearts[i].SetActive(i < livesRemaining);
            }
        }
    }
}
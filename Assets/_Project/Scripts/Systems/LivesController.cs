using System;
using UnityEngine;

namespace EscapeTheLava.Systems
{
    public class LivesController : MonoBehaviour
    {
        [SerializeField] private int startingLives = 5;

        public int CurrentLives { get; private set; }

        public static event Action<int> OnLivesChanged; // new lives value
        public static event Action OnLivesDepleted;      // triggers Lose check

        private void Awake()
        {
            CurrentLives = startingLives;
        }

        public void LoseLife()
        {
            if (CurrentLives <= 0) return; // never go negative — closes a risk-table item

            CurrentLives = Mathf.Max(0, CurrentLives - 1);
            OnLivesChanged?.Invoke(CurrentLives);

            if (CurrentLives == 0)
            {
                OnLivesDepleted?.Invoke();
            }
        }
    }
}
using System;
using UnityEngine;

namespace EscapeTheLava.Systems
{
    public class TimerController : MonoBehaviour
    {
        [SerializeField] private float startingTime = 30f;

        public float TimeRemaining { get; private set; }
        public bool IsRunning { get; private set; }

        public static event Action<float> OnTimeChanged; // new time remaining
        public static event Action OnTimerExpired;         // triggers Lose check

        private void Awake()
        {
            TimeRemaining = startingTime;
        }

        public void StartTimer()
        {
            IsRunning = true;
        }

        public void StopTimer()
        {
            IsRunning = false;
        }

        private void Update()
        {
            if (!IsRunning) return;

            TimeRemaining -= Time.deltaTime;

            if (TimeRemaining <= 0f)
            {
                TimeRemaining = 0f; // never negative — closes a risk-table item
                IsRunning = false;
                OnTimeChanged?.Invoke(TimeRemaining);
                OnTimerExpired?.Invoke();
                return;
            }

            OnTimeChanged?.Invoke(TimeRemaining);
        }
    }
}
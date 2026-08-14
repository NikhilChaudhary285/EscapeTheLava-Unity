using System;
using UnityEngine;

namespace EscapeTheLava.Systems
{
    public class ScoreController : MonoBehaviour
    {
        public int Score { get; private set; }
        public int DiamondsCollected { get; private set; }
        public int TotalDiamonds { get; private set; }

        public static event Action<int> OnScoreChanged;       // new score value
        public static event Action OnAllDiamondsCollected;    // triggers Win check

        public void Initialize(int totalDiamonds)
        {
            TotalDiamonds = totalDiamonds;
            Score = 0;
            DiamondsCollected = 0;
        }

        public void CollectDiamond()
        {
            Score += 1;
            DiamondsCollected += 1;
            OnScoreChanged?.Invoke(Score);

            if (DiamondsCollected >= TotalDiamonds)
            {
                OnAllDiamondsCollected?.Invoke();
            }
        }
    }
}
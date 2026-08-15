using UnityEngine;

namespace EscapeTheLava.VFX
{
    public class VfxManager : MonoBehaviour
    {
        [SerializeField] private ParticleSystem lavaSplashPrefab;

        public void PlayLavaSplash(Vector3 worldPosition)
        {
            ParticleSystem instance = Instantiate(lavaSplashPrefab, worldPosition, lavaSplashPrefab.transform.rotation);
            instance.Play();

            // Self-destruct once the burst finishes — no manual pooling needed
            // for an effect this cheap and infrequent (max 5 lava taps per round).
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }
}
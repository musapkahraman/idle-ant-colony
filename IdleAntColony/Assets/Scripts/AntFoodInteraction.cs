using System.Collections;
using UnityEngine;

public class AntFoodInteraction : MonoBehaviour
{
    private static bool _isAlreadyPlaying;
    [SerializeField] private AudioClip chewingSound;
    [SerializeField] private ParticleSystem particles;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Chew(Transform targetPiece, float lossScale, float chewingInterval)
    {
        targetPiece.localScale -= lossScale * Vector3.one;
        PlayChewingSound(chewingInterval);
        var targetMaterials = targetPiece.GetComponent<Renderer>().materials;
        if (targetMaterials.Length > 0)
        {
            var targetColor = targetMaterials[targetMaterials.Length - 1].color;
            var settings = particles.main;
            settings.startColor = new ParticleSystem.MinMaxGradient(targetColor);
            particles.Play();
        }
    }

    private void PlayChewingSound(float chewingInterval)
    {
        if (_isAlreadyPlaying) return;
        float speed = chewingSound.length / chewingInterval;
        _audioSource.pitch = speed;
        _audioSource.PlayOneShot(chewingSound);
        StartCoroutine(SoundPlayTimeWaitingCoroutine(chewingInterval));
    }

    private static IEnumerator SoundPlayTimeWaitingCoroutine(float length)
    {
        _isAlreadyPlaying = true;
        yield return new WaitForSeconds(length);
        _isAlreadyPlaying = false;
    }
}
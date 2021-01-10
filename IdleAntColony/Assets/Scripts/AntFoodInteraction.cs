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
        var settings = particles.main;
        settings.duration = chewingSound.length;
    }

    public void Chew(Transform targetPiece, float lossScale)
    {
        targetPiece.localScale -= lossScale * Vector3.one;
        PlayChewingSound();
        var targetMaterials = targetPiece.GetComponent<Renderer>().materials;
        if (targetMaterials.Length > 0)
        {
            var targetColor = targetMaterials[targetMaterials.Length - 1].color;
            var settings = particles.main;
            settings.startColor = new ParticleSystem.MinMaxGradient(targetColor);
            particles.Play();
        }
    }

    private void PlayChewingSound()
    {
        if (_isAlreadyPlaying) return;
        _audioSource.PlayOneShot(chewingSound);
        StartCoroutine(SoundPlayTimeWaitingCoroutine(chewingSound.length));
    }

    private static IEnumerator SoundPlayTimeWaitingCoroutine(float length)
    {
        _isAlreadyPlaying = true;
        yield return new WaitForSeconds(length);
        _isAlreadyPlaying = false;
    }
}
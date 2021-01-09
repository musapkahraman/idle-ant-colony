using System.Collections;
using UnityEngine;

public class AntFoodInteraction : MonoBehaviour
{
    [SerializeField] private AudioClip[] chewingSounds;
    [SerializeField] private ParticleSystem particles;
    private AudioSource _audioSource;
    private static bool _isAlreadyPlaying;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
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
        if (chewingSounds.Length == 0 || _isAlreadyPlaying) return;
        var soundClip = chewingSounds[Random.Range(0, chewingSounds.Length)];
        _audioSource.PlayOneShot(soundClip);
        StartCoroutine(SoundPlayTimeWaitingCoroutine(soundClip.length));
    }

    private static IEnumerator SoundPlayTimeWaitingCoroutine(float length)
    {
        _isAlreadyPlaying = true;
        yield return new WaitForSeconds(length);
        _isAlreadyPlaying = false;
    }
}
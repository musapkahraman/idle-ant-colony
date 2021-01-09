using System.Collections;
using UnityEngine;

public class AntSounds : MonoBehaviour
{
    [SerializeField] private AudioClip[] chewingSounds;
    private AudioSource _audioSource;
    private static bool _isAlreadyPlaying;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayChewingSound()
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
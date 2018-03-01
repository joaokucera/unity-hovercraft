using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioSource _audioSource;

    [SerializeField]
    private AudioClip _hitClip;
    [SerializeField]
    private AudioClip _playerShotClip;
    [SerializeField]
    private AudioClip _enemyShotClip;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public static void StopBackgroundMusic()
    {
        _audioSource.Stop();
    }

    public void PlayHitClip()
    {
        _audioSource.PlayOneShot(_hitClip);
    }

    public void PlayPlayerShotClip()
    {
        _audioSource.PlayOneShot(_playerShotClip);
    }

    public void PlayEnemyShotClip()
    {
        _audioSource.PlayOneShot(_enemyShotClip);
    }
}
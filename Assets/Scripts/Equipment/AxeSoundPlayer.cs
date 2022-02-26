using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AxeSoundPlayer : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField]
    private AudioClip[] _hitSounds;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayHitSound()
    {
        if (_hitSounds.Length != 0)
        {
            _audioSource.PlayOneShot(_hitSounds[Random.Range(0, _hitSounds.Length - 1)]);
        }
    }
}

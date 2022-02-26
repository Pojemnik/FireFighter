using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundSource : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerMovement _playerMovement;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip _walkingSound;
    [SerializeField]
    private AudioClip _landingSound;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _playerMovement.Landed += OnPlayerLanding;
        _playerMovement.WalkingStateChanged += OnPlayerWalkStateChange;
        _audioSource.clip = _walkingSound;
        _audioSource.loop = true;
    }

    private void OnPlayerWalkStateChange(object sender, bool state)
    {
        if(state)
        {
            _audioSource.Play();
        }
        else
        {
            _audioSource.Stop();
        }
    }

    private void OnPlayerLanding(object sender, System.EventArgs args)
    {
        _audioSource.PlayOneShot(_landingSound);
    }
}

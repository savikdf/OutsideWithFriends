using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAudioClips { 
    Jump,
};

public class PlayerAudioController : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip jumpNoise;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPlayerClip(PlayerAudioClips clipToPlay)
    {
        switch (clipToPlay)
        {
            case PlayerAudioClips.Jump:
                // audio was broken without this
                audioSource.pitch = Random.Range(0.3f, 2.0f);

                Play(jumpNoise);
                break;
        }
    }

    private void Play(AudioClip clip)
    {
        if(!audioSource.isPlaying)
            audioSource.PlayOneShot(clip);
    }

}

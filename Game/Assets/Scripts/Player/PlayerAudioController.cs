using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAudioClips { 
    Jump,
};

public class PlayerAudioController : MonoBehaviour
{
    private AudioSource audioSource;
    private float originalPitch;

    public AudioClip jumpNoise;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        originalPitch = audioSource.pitch;
    }

    public void PlayPlayerClip(PlayerAudioClips clipToPlay)
    {
        switch (clipToPlay)
        {
            case PlayerAudioClips.Jump:
                Play(jumpNoise, true);
                break;
        }
    }

    private void Play(AudioClip clip, bool randomPitch)
    {
        if (!audioSource.isPlaying) {
            audioSource.pitch = randomPitch ? Random.Range(0.05f, 3f) : originalPitch;            
            audioSource.PlayOneShot(clip);
        }
    }

}

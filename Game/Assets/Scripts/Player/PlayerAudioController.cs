using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAudioClips { 
    Jump,
    Fire,
};

public class PlayerAudioController : MonoBehaviour
{
    private AudioSource audioSource;
    private float originalPitch;
    public AudioClip jumpNoise;
    public AudioClip fireNoise;
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
                Play(jumpNoise, true, 0.2f, 2.5f);
                break;
             case PlayerAudioClips.Fire:
                Play(fireNoise, true, 0.2f, 2.0f);
                break;
        }
    }

    private void Play(AudioClip clip, bool randomPitch, float from, float to)
    {
        if (!audioSource.isPlaying) {
            audioSource.pitch = randomPitch ? Random.Range(from, to) : originalPitch;            
            audioSource.PlayOneShot(clip);
        }
    }

}

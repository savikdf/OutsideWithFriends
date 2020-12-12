using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EnvironmentClips
{
    JumpPad,
}

public class AudioManager : MonoBehaviour, IManager
{
    public static AudioManager singleton;
    public AudioClip jumpPadAudioClip;    

    public void PlayOneShotClipAtPoint(EnvironmentClips clipType, Vector3 pos)
    {
        AudioClip clipToUse = null;
        switch (clipType)
        {
            case EnvironmentClips.JumpPad:
                clipToUse = jumpPadAudioClip;
                break;
        }

        AudioSource.PlayClipAtPoint(clipToUse, pos, 2f);      
    }

    void Awake() {
        singleton = this;
    }

    public bool Initialize()
    {       
        return true;
    }

    public IEnumerator Routine()
    {
        yield return null;
    }
}

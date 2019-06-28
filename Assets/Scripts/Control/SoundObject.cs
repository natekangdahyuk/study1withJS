using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    public AudioSource source { get; private set; }
    public SoundManager.SoundType type = SoundManager.SoundType.None;


    private void Awake()
    {
        source = gameObject.GetComponent<AudioSource>();
        if (type == SoundManager.SoundType.None)
        {
            if (source == null)
                source = gameObject.AddComponent<AudioSource>();
        }
        //else
        //{
        //    if( type == SoundManager.SoundType.BGM )
        //        source.volume = GameOption.BGMVolume;
        //    else if( type == SoundManager.SoundType.InGameBGM )
        //        source.volume = GameOption.BGMVolume;
        //    else if( type == SoundManager.SoundType.voice )
        //        source.volume = GameOption.VoiceVoluem;
        //    else if( type == SoundManager.SoundType.Effect )
        //        source.volume = GameOption.EffectVoluem;
        //}
    }

    private void Start()
    {
        if (type != SoundManager.SoundType.None)
        {
            if (type == SoundManager.SoundType.BGM)
                source.volume = GameOption.BGMVolume;
            else if (type == SoundManager.SoundType.InGameBGM)
                source.volume = GameOption.BGMVolume;
            else if (type == SoundManager.SoundType.voice)
                source.volume = GameOption.VoiceVoluem;
            else if (type == SoundManager.SoundType.Effect)
                source.volume = GameOption.EffectVoluem;
        }
    }

    public void SetClip(AudioClip clip)
    {
        source.clip = clip;
    }

    public void Play()
    {
        source.Play();
    }

    public void SetVolume(float value)
    {
        source.volume = value;
    }

    public float GetVolume()
    {
        return source.volume;
    }

    public bool IsPlay()
    {
        return source.isPlaying;
    }
}

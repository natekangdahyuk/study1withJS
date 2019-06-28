using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SoundManager : MonoSinglton<SoundManager>
{
    public enum SoundType
    {
        None = -1,
        voice = 0,
        BGM,
        InGameBGM,
        Effect,
        Max,
    }

    //SoundManager.I.Play(SoundManager.SoundType.Effect, "snd_ui_common_button" , GameOption.EffectVoluem );
    //SoundManager.I.Play(SoundManager.SoundType.Effect, "snd_ui_common_selectcard" , GameOption.EffectVoluem );
    //SoundManager.I.Play(SoundManager.SoundType.Effect, "snd_ui_common_page" , GameOption.EffectVoluem );

    private SoundComponent[] dicSound = new SoundComponent[(int)SoundType.Max];


    public override void Awake()
    {
        base.Awake();
        for (int i = 0; i < dicSound.Length; i++)
        {
            GameObject go = new GameObject();
            go.transform.SetParent(this.transform);
            go.name = ((SoundType)i).ToString();
            dicSound[i] = go.AddComponent<SoundComponent>();
        }
    }

    public override void ClearAll()
    {
        for (int i = 0; i < dicSound.Length; i++)
        {
            dicSound[i].ClearAll();
        }
    }

    public void Add(SoundType type, string name, SoundObject item)
    {
        dicSound[(int)type].Add(name, item);
    }

    public void Clear(SoundType type)
    {
        dicSound[(int)type].ClearAll();
    }

    public void Stop(SoundType type)
    {
        dicSound[(int)type].Stop();
    }

    public void Stop(SoundType type, string name)
    {
        dicSound[(int)type].Stop(name);
    }

    void Play(SoundType type, string name)
    {
        Play(type, name, 1f, false);
    }
    public SoundObject Play(SoundType type, string name, float volume)
    {
        return Play(type, name, volume, false);
    }

    public bool IsPlay(SoundType type, string name)
    {
        return dicSound[(int)type].IsPlay(name);
    }

    void Play(SoundType type, string name, bool loop)
    {
        Play(type, name, 1f, loop);
    }
    public SoundObject Play(SoundType type, string name, float volume, bool loop)
    {
        return dicSound[(int)type].Play(name, volume, loop);
    }


    public static SoundObject New(SoundType type, SoundManager sndMgr, string soundPath)
    {
        return sndMgr.dicSound[(int)type].New(soundPath);
    }


}

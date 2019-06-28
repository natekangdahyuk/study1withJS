using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundComponent : MonoBehaviour
{
    private Dictionary<string, SoundObject> dicSound = new Dictionary<string, SoundObject>();

    public int Count { get { return dicSound != null ? dicSound.Count : 0; } }


    public void ClearAll()
    {
        foreach (KeyValuePair<string, SoundObject> value in dicSound)
        {
            GameObject.DestroyImmediate(value.Value.gameObject);
        }

        dicSound.Clear();
    }

    public void Add(string name, SoundObject item)
    {
        dicSound.Add(name, item);
    }
    public bool Remove(string name)
    {
        return dicSound.Remove(name);
    }

    public SoundObject TryGetValue(string name)
    {
        SoundObject sndObj = null;
        if (dicSound.TryGetValue(name, out sndObj))
        {
            return sndObj;
        }

        return null;
    }
    public bool ContainsKey(string name) { return dicSound.ContainsKey(name); }

    void Play(string name)
    {
        Play(name, 1f, false);
    }
    public void Play(string name, float volume)
    {
        Play(name, volume, false);
    }
    void Play(string name, bool loop)
    {
        Play(name, 1f, loop);
    }

    public bool IsPlay(string name)
    {
        SoundObject sndObj = TryGetValue(name);
        if (sndObj == null)
        {
            sndObj = New(name);

            if (sndObj == null)
            {
                Debug.LogErrorFormat("[SoundManager] Not exist sound : {0}", name);
                return false;
            }

        }
        return sndObj.IsPlay();
    }


    public SoundObject Play(string name, float volume, bool loop)
    {
        SoundObject sndObj = TryGetValue(name);
        if (sndObj == null)
        {
            sndObj = New(name);

            if (sndObj == null)
            {
                Debug.LogErrorFormat("[SoundManager] Not exist sound : {0}", name);
                return null;
            }

        }

        sndObj.source.volume = volume;
        sndObj.source.loop = loop;
        sndObj.source.Play();

        return sndObj;
    }

    public void Stop()
    {
        foreach (KeyValuePair<string, SoundObject> kvp in dicSound)
            kvp.Value.source.Stop();
    }
    public void Stop(string name)
    {
        SoundObject sndObj = TryGetValue(name);
        if (sndObj != null)
            sndObj.source.Stop();
    }
    public SoundObject New(string soundPath)
    {
        if (TryGetValue(soundPath))
            return null;

        AudioClip clip = ResourceManager.LoadSound(soundPath);
        if (clip == null)
            return null;

        GameObject go = new GameObject(clip.name);
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        go.layer = gameObject.layer;

        SoundObject sndObj = go.AddComponent<SoundObject>();
        sndObj.SetClip(clip);

        Add(soundPath, sndObj);
        return sndObj;
    }
}
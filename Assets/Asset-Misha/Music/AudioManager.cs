using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;
    public bool loop;

    [HideInInspector] public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public List<Sound> sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (var sound in sounds)
        {
            if (sound.clip == null)
            {
                Debug.LogWarning("Sound clip is missing for: " + sound.name);
                continue;
            }

            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;

            Debug.Log("Added AudioSource for: " + sound.name);
        }
    }

    public void Play(string name)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound != null)
        {
            if (sound.source == null)
            {
                Debug.LogError("AudioSource is missing for: " + name);
                return;
            }
            sound.source.Play();
            Debug.Log("Playing sound: " + name);
        }
        else
        {
            Debug.LogWarning("Sound not found: " + name);
        }
    }

    public void Stop(string name)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound != null)
        {
            if (sound.source == null)
            {
                Debug.LogError("AudioSource is missing for: " + name);
                return;
            }
            sound.source.Stop();
            Debug.Log("Stopped sound: " + name);
        }
        else
        {
            Debug.LogWarning("Sound not found: " + name);
        }
    }
}

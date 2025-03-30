using System.Collections;
using UnityEngine;

public class ScreamerManager : MonoBehaviour
{
    public static ScreamerManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayScreamer(AudioClip clip, Vector3 position, float volume = 1f, float duration = 3f)
    {
        if (clip == null)
        {
            //Debug.LogError("Screamer clip is NULL!");
            return;
        }

        //Debug.Log("Запуск скримера! Звук: " + clip.name);

        GameObject soundObject = new GameObject("ScreamerSound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.spatialBlend = 1f; // 3D-звук
        soundObject.transform.position = position;

        audioSource.Play();
        Destroy(soundObject, duration);
    }
}

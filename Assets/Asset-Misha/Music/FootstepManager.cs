using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    public AudioSource audioSource;
    public List<FootstepSurface> surfaces;
    public float stepInterval = 0.3f; // Шаги чаще
    public float minSpeedThreshold = 0.01f; // Минимальная скорость для срабатывания шага
    public float stepResetDelay = 0.2f; // Время перед сбросом stepTimer

    private float stepTimer = 0f;
    private Vector3 lastPosition;
    private float lastSpeed = 0f; // Последняя известная скорость
    private float timeSinceLastMove = 0f;

    private void Update()
    {
        float speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        speed = Mathf.Max(speed, lastSpeed * 0.9f); // Фильтрация резких изменений скорости
        lastPosition = transform.position;

        if (speed > minSpeedThreshold)
        {
            stepTimer += Time.deltaTime;
            timeSinceLastMove = 0f; // Обнуляем таймер простоя

            if (stepTimer >= stepInterval)
            {
                PlayFootstep();
                stepTimer = 0f;
            }
        }
        else
        {
            timeSinceLastMove += Time.deltaTime;
            if (timeSinceLastMove > stepResetDelay)
            {
                stepTimer = 0f; // Сбрасываем таймер шагов только после задержки
            }
        }

        lastSpeed = speed;
    }

    private void PlayFootstep()
    {
        //Debug.Log("PlayFootstep called!");
        string surfaceTag = GetSurfaceTag();
        FootstepSurface surface = surfaces.Find(s => s.surfaceTag == surfaceTag);

        if (surface != null && surface.footstepSounds.Length > 0)
        {
            AudioClip clip = surface.footstepSounds[Random.Range(0, surface.footstepSounds.Length)];
            //Debug.Log("Playing sound for surface: " + surfaceTag);

            if (audioSource == null)
            {
                //Debug.LogError("AudioSource is NULL!");
                return;
            }

            if (clip == null)
            {
                //Debug.LogError("Footstep sound clip is NULL!");
                return;
            }

            // Рандомный Pitch от 0.8 до 1.2
            audioSource.pitch = Random.Range(0.9f, 1.1f);

            audioSource.PlayOneShot(clip);
            //Debug.Log("Playing sound: " + clip.name + " with pitch: " + audioSource.pitch);
        }
        else
        {
            //Debug.LogWarning("No footstep sounds found for surface: " + surfaceTag);
        }
    }

    private string GetSurfaceTag()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 2f))
        {
            //Debug.Log("Raycast hit: " + hit.collider.name + " with tag: " + hit.collider.tag);
            return hit.collider.tag;
        }

        //Debug.Log("Raycast did not hit anything!");
        return "Default";
    }
}

using UnityEngine;

public class StoryTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip storySound; // Звук, который нужно проиграть
    [SerializeField] private float volume = 1f;    // Громкость (от 0 до 1)
    
    private AudioSource audioSource;
    private bool hasTriggered = false; // Чтобы звук не играл несколько раз

    private void Start()
    {
        if (storySound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // Защита от повторного срабатывания
            
            // Проигрываем звук (если он есть)
            if (storySound != null && audioSource != null)
            {
                audioSource.PlayOneShot(storySound, volume);
            }

            // Уничтожаем объект после проигрыша звука
            Destroy(gameObject, storySound != null ? storySound.length : 0f);
        }
    }
}
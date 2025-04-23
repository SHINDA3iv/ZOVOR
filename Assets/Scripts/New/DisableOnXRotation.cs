using UnityEngine;

public class DisableOnXRotation : MonoBehaviour
{
    [SerializeField] private float targetXRotation = 90f; // Угол, при котором нужно отключиться
    [SerializeField] private float threshold = 5f; // Порог точности (чтобы не проверять на точное совпадение)
    [SerializeField] private GameObject obj; // Порог точности (чтобы не проверять на точное совпадение)
    [SerializeField] private AudioClip soundClip;         // Звук, который проиграется
    [SerializeField] private float volume = 1f;   

    private AudioSource audioSource;
    private void Start()
    {
        // Добавляем AudioSource, если его нет
        if (!TryGetComponent<AudioSource>(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    private void Update()
    {
        // Получаем текущий угол вращения по оси X
        float currentXRotation = transform.rotation.eulerAngles.x;
        Debug.Log(Mathf.Abs(currentXRotation - targetXRotation));
        // Проверяем, достигнут ли целевой угол (с учетом порога)
        if (Mathf.Abs(currentXRotation - targetXRotation) < threshold) 
        {
            obj.SetActive(false);
            audioSource.PlayOneShot(soundClip, volume);
        }
        else 
            obj.SetActive(true);
    }
}
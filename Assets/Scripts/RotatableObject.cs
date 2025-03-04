using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RotatableObject : MonoBehaviour
{
    [Header("Настройки поворота")]
    [Tooltip("Минимальный угол изменения поворота (в градусах), чтобы считать объект повернутым.")]
    [SerializeField] private float rotationThreshold = 90f;

    private bool _isRotated = false;
    private bool _rotationEventTriggered = false;
    private Quaternion _initialRotation;
    private HandleXRGrabInteractable grabInteractable;

    /// <summary>
    /// Событие, вызываемое при повороте объекта.
    /// </summary>
    public event System.Action OnObjectRotatedEvent;

    /// <summary>
    /// Свойство для получения состояния поворота объекта.
    /// </summary>
    public bool IsRotated
    {
        get { return _isRotated; }
        private set { _isRotated = value; }
    }

    private void Start()
    {
        _initialRotation = transform.rotation;
        grabInteractable = GetComponent<HandleXRGrabInteractable>();
    }

    public void Enable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    public void Disable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
    }

    public void OnGrab()
    {
        _initialRotation = transform.rotation;
        StartCoroutine(TrackRotation());
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        StopCoroutine(TrackRotation());
    }

    private IEnumerator TrackRotation()
    {
        while (true)
        {
            if (Quaternion.Angle(transform.rotation, _initialRotation) >= rotationThreshold && !_rotationEventTriggered)
            {
                OnObjectRotated();
            }

            yield return null;
        }
    }

    private void OnObjectRotated()
    {
        IsRotated = true;
        _rotationEventTriggered = true;

        Debug.Log($"Объект повернут! Новый поворот: {transform.rotation.eulerAngles}");
        grabInteractable.enabled = false;

        OnObjectRotatedEvent?.Invoke();
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
    }
}
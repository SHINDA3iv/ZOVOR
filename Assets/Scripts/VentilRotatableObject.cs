using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VentilRotatableObject : MonoBehaviour
{
    [Header("Настройки поворота")]
    [Tooltip("Максимальный угол поворота вентиля (в градусах).")]
    [SerializeField] private float maxRotationAngle = 80f;

    private Quaternion _initialRotation;
    private XRGrabInteractable grabInteractable;
    private Transform _handTransform;
    private float _currentRotationAngle = 0f; // Текущий угол поворота
    private bool _isFullyRotated = false;

    public event System.Action OnObjectRotatedEvent;

    private void Start()
    {
        _initialRotation = transform.rotation;
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        _handTransform = args.interactorObject.transform;
        StartCoroutine(TrackHandPosition());
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        StopAllCoroutines();
        _isFullyRotated = false;
    }

    private IEnumerator TrackHandPosition()
    {
        Vector3 centerOfValve = transform.position;
        Vector3 initialHandDirection = (_handTransform.position - centerOfValve).normalized;

        while (true)
        {
            if (_handTransform == null)
                yield break;

            Vector3 currentHandDirection = (_handTransform.position - centerOfValve).normalized;

            // Вычисляем изменение угла между начальным и текущим направлением
            float angleDelta = Vector3.SignedAngle(initialHandDirection, currentHandDirection, transform.forward);

            // Обновляем текущий угол поворота
            _currentRotationAngle += angleDelta;
            _currentRotationAngle = Mathf.Clamp(_currentRotationAngle, 0, maxRotationAngle);

            // Применяем поворот к объекту
            transform.rotation = _initialRotation * Quaternion.Euler(0, _currentRotationAngle, 0);

            // Проверяем, достигнут ли максимальный угол поворота
            if (_currentRotationAngle >= maxRotationAngle && !_isFullyRotated)
            {
                _isFullyRotated = true;
                OnObjectRotated();
            }

            // Обновляем начальное направление для следующего кадра
            initialHandDirection = currentHandDirection;

            yield return null;
        }
    }

    private void OnObjectRotated()
    {
        Debug.Log($"Вентиль полностью повернут! Угол: {_currentRotationAngle}");
        OnObjectRotatedEvent?.Invoke();
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
    }
}
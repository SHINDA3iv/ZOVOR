using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VentilRotatableObject : MonoBehaviour
{
    [Header("��������� ��������")]
    [Tooltip("������������ ���� �������� ������� (� ��������).")]
    [SerializeField] private float maxRotationAngle = 80f;

    private Quaternion _initialRotation;
    private XRGrabInteractable grabInteractable;
    private Transform _handTransform;
    private float _currentRotationAngle = 0f; // ������� ���� ��������
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

            // ��������� ��������� ���� ����� ��������� � ������� ������������
            float angleDelta = Vector3.SignedAngle(initialHandDirection, currentHandDirection, transform.forward);

            // ��������� ������� ���� ��������
            _currentRotationAngle += angleDelta;
            _currentRotationAngle = Mathf.Clamp(_currentRotationAngle, 0, maxRotationAngle);

            // ��������� ������� � �������
            transform.rotation = _initialRotation * Quaternion.Euler(_currentRotationAngle, 0, 0);

            // ���������, ��������� �� ������������ ���� ��������
            if (_currentRotationAngle >= maxRotationAngle && !_isFullyRotated)
            {
                _isFullyRotated = true;
                OnObjectRotated();
            }

            // ��������� ��������� ����������� ��� ���������� �����
            initialHandDirection = currentHandDirection;

            yield return null;
        }
    }

    private void OnObjectRotated()
    {
        Debug.Log($"������� ��������� ��������! ����: {_currentRotationAngle}");
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
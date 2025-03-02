using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RotatableObject : MonoBehaviour
{
    [Header("��������� ��������")]
    [Tooltip("����������� ���� ��������� �������� (� ��������), ����� ������� ������ ����������.")]
    [SerializeField] private float rotationThreshold = 90f;

    private bool _isRotated = false;
    private bool _rotationEventTriggered = false;
    private Quaternion _initialRotation;
    private XRGrabInteractable grabInteractable;

    /// <summary>
    /// �������, ���������� ��� �������� �������.
    /// </summary>
    public event System.Action OnObjectRotatedEvent;

    /// <summary>
    /// �������� ��� ��������� ��������� �������� �������.
    /// </summary>
    public bool IsRotated
    {
        get { return _isRotated; }
        private set { _isRotated = value; }
    }

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
        StartCoroutine(TrackRotation());
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        StopAllCoroutines();
    }

    private IEnumerator TrackRotation()
    {
        while (true)
        {
            if (Quaternion.Angle(transform.rotation, _initialRotation) > rotationThreshold && !_rotationEventTriggered)
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

        Debug.Log($"������ ��������! ����� �������: {transform.rotation.eulerAngles}");

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
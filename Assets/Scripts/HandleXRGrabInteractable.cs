using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandleXRGrabInteractable : XRGrabInteractable
{
    private Rigidbody _rigidbody;
    private bool _wasKinematic;

    private bool isInSocket = false;
    private Transform currentSocket = null;

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody>();

        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody is missing on the grabbable object. Please add one.");
        }
        else
        {
            _wasKinematic = _rigidbody.isKinematic;
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        attachTransform = null;

        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            FreezeRotation();
            ResolveCollisions();
        }

        StartCoroutine(CancelGrabWhenHandMoves(args.interactorObject.transform.parent));
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        if (_rigidbody != null)
        {
            ResolveCollisions();
            _rigidbody.isKinematic = _wasKinematic;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            UnfreezeRotation();
        }
    }

    public void PlaceInSocket(Transform socket)
    {
        if (isInSocket) return;

        transform.position = socket.position;
        transform.rotation = socket.rotation;

        isInSocket = true;
        currentSocket = socket;

        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true;
        }
    }

    public void RemoveFromSocket()
    {
        if (!isInSocket) return;

        isInSocket = false;
        currentSocket = null;

        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = _wasKinematic;
        }
    }

    private IEnumerator CancelGrabWhenHandMoves(Transform handTransform)
    {
        while (true)
        {
            Vector3 distance = this.transform.position - handTransform.position;

            if (distance.magnitude > 0.3f)
            {
                float speed = 5f;
                Vector3 targetPosition = Vector3.MoveTowards(this.transform.position, handTransform.position, speed * Time.deltaTime);

                if (!IsPositionValid(targetPosition))
                {
                    ResolveCollisions();
                    break;
                }

                this.transform.position = targetPosition;
            }

            yield return null;
        }
    }

    private bool IsPositionValid(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapBox(position, transform.localScale / 2, transform.rotation);

        foreach (var collider in colliders)
        {
            if (collider.gameObject != this.gameObject && !collider.isTrigger)
            {
                return false;
            }
        }

        return true;
    }

    private void ResolveCollisions()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation);

        foreach (var collider in colliders)
        {
            if (collider.gameObject != this.gameObject && !collider.isTrigger)
            {
                if (Physics.ComputePenetration(
                    GetComponent<Collider>(), transform.position, transform.rotation,
                    collider, collider.transform.position, collider.transform.rotation,
                    out Vector3 direction, out float distance))
                {
                    transform.position += direction * distance;
                }
            }
        }
    }

    private void FreezeRotation()
    {
        if (_rigidbody != null)
        {
            _rigidbody.freezeRotation = true;
        }
    }

    private void UnfreezeRotation()
    {
        if (_rigidbody != null)
        {
            _rigidbody.freezeRotation = false;
        }
    }
}
using UnityEngine;

public class VRCollisionPrevention : MonoBehaviour
{
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private LayerMask collisionLayer;

    private void Start()
    {
        // Validate assignments
        Debug.Assert(head != null, "Head Transform is not assigned.");
        Debug.Assert(leftHand != null, "Left Hand Transform is not assigned.");
        Debug.Assert(rightHand != null, "Right Hand Transform is not assigned.");
        Debug.Assert(collisionLayer != 0, "Collision Layer is not set.");
    }

    private void Update()
    {
        if (head != null) CheckAndPreventCollision(head);
        if (leftHand != null) CheckAndPreventCollision(leftHand);
        if (rightHand != null) CheckAndPreventCollision(rightHand);
    }

    private void CheckAndPreventCollision(GameObject target)
    {
        if (target == null) return;

        if (Physics.CheckSphere(target.transform.position, 0.1f, collisionLayer))
        {
            Vector3 lastSafePosition = GetLastSafePosition(target.transform.position);
            target.transform.position = lastSafePosition;
        }
    }

    private Vector3 GetLastSafePosition(Vector3 currentPosition)
    {
        RaycastHit hit;
        if (Physics.Raycast(currentPosition, -Vector3.forward, out hit, 1f, collisionLayer))
        {
            return hit.point + hit.normal * 0.1f; // Offset by normal to avoid clipping
        }
        return currentPosition; // Return original position if no safe position found
    }
}
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private float minHeight = -5f;

    [SerializeField] private Transform respawnPosition;

    [SerializeField] private GameObject parentObject;

    private Vector3 initialPosition;
    private Rigidbody rb;

    void Start()
    {
        initialPosition = transform.position;

        if (respawnPosition.position == Vector3.zero)
        {
            respawnPosition.position = initialPosition;
        }

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (transform.position.y < minHeight)
        {
            RespawnObject();
        }
    }

    private void RespawnObject()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        transform.position = respawnPosition.position;

        if (parentObject != null)
        {
            transform.SetParent(parentObject.transform);
        }
    }
}
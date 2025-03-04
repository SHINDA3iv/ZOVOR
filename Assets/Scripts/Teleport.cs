using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private float minHeight = -5f;

    [SerializeField] private Transform respawnPosition;

    [SerializeField] private GameObject parentObject;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;

        if (respawnPosition.position == Vector3.zero)
        {
            respawnPosition.position = initialPosition;
        }
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
        transform.position = respawnPosition.position;

        if (parentObject != null)
        {
            transform.SetParent(parentObject.transform);
        }
    }
}
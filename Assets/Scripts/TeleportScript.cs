using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    public Transform teleportTarget;

    void Update()
    {
        CheckIfOutOfBounds();

    }

    void TeleportToTarget()
    {
        if (teleportTarget != null)
        {
            transform.position = teleportTarget.position;
        }
    }

    void CheckIfOutOfBounds()
    {
        if (transform.position.y < -1)
        {
            TeleportToTarget();
        }
    }
}

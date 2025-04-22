using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorusScript : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;

    // Update is called once per frame
    void Update()
    {
        if(cameraTransform != null)
        {
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                cameraTransform.rotation.eulerAngles.y,
                transform.rotation.eulerAngles.z
            );
            transform.position = new Vector3(
                cameraTransform.position.x,
                transform.position.y,
                cameraTransform.position.z
            );
        }
    }
}

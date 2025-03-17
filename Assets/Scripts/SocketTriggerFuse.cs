using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketTriggerFuse : MonoBehaviour
{
    public FusePanelLogic fusePanelLogic;
    private bool isOccupied = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (isOccupied) return;

        if (other.CompareTag("Fuse"))
        {
            isOccupied = true;
            SnapToSocket(other.transform);

            XRGrabInteractable grabInteractable = other.GetComponent<XRGrabInteractable>();
            if (grabInteractable != null)
            {
                grabInteractable.enabled = false;
            }

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            XRSocketInteractor socketInteractor = GetComponent<XRSocketInteractor>();
            if (socketInteractor != null)
            {
                socketInteractor.enabled = false;
            }

            fusePanelLogic?.OnFusePlaced(other.gameObject, gameObject);
        }
    }

    private void SnapToSocket(Transform fuseTransform)
    {
        fuseTransform.position = transform.position;
        fuseTransform.rotation = transform.rotation;
    }
}
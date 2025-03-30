using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class FusePanelLogic : MonoBehaviour
{
    [Header("Door Components")]
    public HingeJoint door1Hinge;
    public HandleXRGrabInteractable door1Handle;
    [SerializeField] private GameObject doorRigidbody;

    private int placedFusesCount = 0;
    private bool isDoorOpened = false;

    public void OnFusePlaced(GameObject fuse, GameObject socket)
    {
        if (isDoorOpened) return; // Дверь уже открыта

        placedFusesCount++;
        Debug.Log($"Вставлен предохранитель. Текущее количество: {placedFusesCount}");

        if (placedFusesCount == 4)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        Destroy(doorRigidbody);
        if (isDoorOpened) return;

        door1Handle.enabled = true;

        Rigidbody door1Rb = door1Hinge.GetComponent<Rigidbody>();

        if (door1Rb != null )
        {
            door1Rb.isKinematic = false;

            JointLimits limits = door1Hinge.limits;
            limits.min = -90;
            limits.max = 90;
            door1Hinge.limits = limits;

        }

        foreach (Transform child in transform)
        {
            SocketTriggerFuse socket = child.GetComponent<SocketTriggerFuse>();
            if (socket != null)
            {
                XRSocketInteractor interactor = socket.GetComponent<XRSocketInteractor>();
                if (interactor != null) interactor.enabled = false;
            }
        }
        isDoorOpened = true;
        Debug.Log("Дверь открыта!");
    }
}
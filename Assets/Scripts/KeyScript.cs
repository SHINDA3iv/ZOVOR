using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KeyScript : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private RotatableObject rotatableObject;
    private new Rigidbody rigidbody;

    [SerializeField] private HingeJoint door1Hinge;
    [SerializeField] private HingeJoint door2Hinge;
    [SerializeField] private HandleXRGrabInteractable door1Handle;
    [SerializeField] private HandleXRGrabInteractable door2Handle;
    [SerializeField] private Light light;

    private Rigidbody door1Rigidbody;
    private Rigidbody door2Rigidbody;

    public GameObject socket;

    private bool isInserted = false;

    public AudioClip insertSound;
    private AudioSource audioSource;

    private ConfigurableJoint joint;

    private Quaternion initialControllerRotation;
    private Quaternion initialObjectRotation;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rotatableObject = GetComponent<RotatableObject>();
        rigidbody = GetComponent<Rigidbody>();

        door1Rigidbody = door1Hinge.GetComponent<Rigidbody>();
        door2Rigidbody = door2Hinge.GetComponent<Rigidbody>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!isInserted)
        {
            transform.rotation = socket.transform.rotation;
            StartCoroutine(CheckKeyMovement());
        }
        else
        {
            initialControllerRotation = args.interactorObject.transform.rotation;
            initialObjectRotation = transform.rotation;

            StartCoroutine(ApplyRotation(args.interactorObject.transform));
        }
    }

    private IEnumerator ApplyRotation(Transform controllerTransform)
    {
        while (grabInteractable.isSelected)
        {
            Quaternion deltaRotation = controllerTransform.rotation * Quaternion.Inverse(initialControllerRotation);

            float angleX = deltaRotation.eulerAngles.x;

            if (angleX > 180) angleX -= 360;

            angleX = Mathf.Clamp(angleX, 0f, 90f);

            transform.rotation = initialObjectRotation * Quaternion.Euler(0, angleX, 0);

            yield return null;
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        StopAllCoroutines();

        if (!isInserted && Vector3.Distance(transform.position, socket.transform.position) < 0.1f)
        {
            InsertKey();
        }
    }

    private IEnumerator CheckKeyMovement()
    {
        Vector3 initialPosition = transform.position;

        while (true)
        {
            if (Vector3.Distance(initialPosition, transform.position) > 0.01f)
            {
                break;
            }

            yield return null;
        }

        if (!isInserted && Vector3.Distance(transform.position, socket.transform.position) < 0.1f)
        {
            InsertKey();
        }
    }

    private void InsertKey()
    {
        isInserted = true;

        if (insertSound != null)
        {
            audioSource.PlayOneShot(insertSound);
        }

        transform.position = socket.transform.position;
        transform.rotation = socket.transform.rotation;

        int keyLayer = LayerMask.NameToLayer("Key");
        door2Rigidbody.excludeLayers = keyLayer;

        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;

        rigidbody.constraints = RigidbodyConstraints.FreezePosition
                      | RigidbodyConstraints.FreezeRotationY
                      | RigidbodyConstraints.FreezeRotationZ;

        grabInteractable.trackPosition = false;
        grabInteractable.trackRotation = false;

        light.gameObject.SetActive(false);

        CreateAndConfigureJoint();
        rotatableObject.Enable();
        rotatableObject.OnGrab();
        rotatableObject.OnObjectRotatedEvent += DoorOpened;
    }

    private void CreateAndConfigureJoint()
    {
        joint = gameObject.AddComponent<ConfigurableJoint>();

        joint.connectedBody = socket.GetComponent<Rigidbody>();
        if (joint.connectedBody == null)
        {
            Debug.LogError("Socket does not have a Rigidbody component. Please add one.");
            return;
        }

        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        joint.angularXMotion = ConfigurableJointMotion.Limited;
        joint.angularYMotion = ConfigurableJointMotion.Locked;
        joint.angularZMotion = ConfigurableJointMotion.Locked;

        SoftJointLimit limit = new SoftJointLimit();
        limit.limit = 90f;
        joint.lowAngularXLimit = limit;
        joint.highAngularXLimit = limit;
        joint.axis = Vector3.right;
        joint.secondaryAxis = Vector3.up;
    }

    private void DoorOpened()
    {
        socket.SetActive(false);

        door1Handle.enabled = true;
        door2Handle.enabled = true;

        door1Rigidbody.isKinematic = false;
        door2Rigidbody.isKinematic = false;

        joint.angularXMotion = ConfigurableJointMotion.Locked;
        GetComponent<Collider>().enabled = false;

        rotatableObject.Disable();
        rotatableObject.OnObjectRotatedEvent -= DoorOpened;
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        grabInteractable.selectExited.RemoveListener(OnSelectExited);

        rotatableObject.Disable();
        rotatableObject.OnObjectRotatedEvent -= DoorOpened;
    }
}

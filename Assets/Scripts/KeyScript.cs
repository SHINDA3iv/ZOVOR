using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KeyScript : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private RotatableObject rotatableObject;

    [SerializeField] private HingeJoint door1Hinge;
    [SerializeField] private HingeJoint door2Hinge;

    private Rigidbody door1Rigidbody;
    private Rigidbody door2Rigidbody;

    public Transform socket;

    private bool isInserted = false;

    public AudioClip insertSound;
    private AudioSource audioSource;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rotatableObject = GetComponent<RotatableObject>();

        door1Rigidbody = door1Hinge.GetComponent<Rigidbody>();
        door2Rigidbody = door2Hinge.GetComponent<Rigidbody>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        StartCoroutine(CheckKeyMovement());
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        StopAllCoroutines();

        if (!isInserted && Vector3.Distance(transform.position, socket.position) < 0.1f)
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

        if (!isInserted && Vector3.Distance(transform.position, socket.position) < 0.1f)
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

        transform.position = socket.position;
        transform.rotation = socket.rotation;

        CreateAndConfigureJoint();
        rotatableObject.OnObjectRotatedEvent += DoorOpened;
    }

    private void CreateAndConfigureJoint()
    {
        //FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();

        //fixedJoint.connectedBody = door1Rigidbody;

        ConfigurableJoint joint = gameObject.AddComponent<ConfigurableJoint>();

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
    }

    private void DoorOpened()
    {
        JointLimits door1Limits = door1Hinge.limits;
        door1Limits.min = -90;
        door1Limits.max = 90;
        door1Hinge.limits = door1Limits; 

        JointLimits door2Limits = door2Hinge.limits;
        door2Limits.min = -90;
        door2Limits.max = 90;
        door2Hinge.limits = door2Limits; 

        door1Rigidbody.isKinematic = false;
        door2Rigidbody.isKinematic = false;

        rotatableObject.OnObjectRotatedEvent -= DoorOpened;
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        grabInteractable.selectExited.RemoveListener(OnSelectExited);

        rotatableObject.OnObjectRotatedEvent -= DoorOpened;
    }
}

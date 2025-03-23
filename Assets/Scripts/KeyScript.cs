using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KeyScript : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private new Rigidbody rigidbody;

    [SerializeField] private HingeJoint door1Hinge;
    [SerializeField] private HingeJoint door2Hinge;
    [SerializeField] private HandleXRGrabInteractable door1Handle;
    [SerializeField] private HandleXRGrabInteractable door2Handle;
    [SerializeField] private Light keyHoleLight;
    [SerializeField] private Light keyLight;

    private Rigidbody door1Rigidbody;
    private Rigidbody door2Rigidbody;

    public GameObject socket;

    private bool isInserted = false;

    public AudioClip insertSound;
    private AudioSource audioSource;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rigidbody = GetComponent<Rigidbody>();

        door1Rigidbody = door1Hinge.GetComponent<Rigidbody>();
        door2Rigidbody = door2Hinge.GetComponent<Rigidbody>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);

        keyHoleLight.enabled = false;
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!isInserted)
        {
            keyHoleLight.enabled = true;
            keyLight.enabled = false;
            transform.rotation = socket.transform.rotation;
            //StartCoroutine(CheckKeyMovement());
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        //StopAllCoroutines();

        if (isInserted)
            return;

        keyHoleLight.enabled = false;
        keyLight.enabled = true;

        if (Vector3.Distance(transform.position, socket.transform.position) < 0.1f)
            InsertKey();
    }

    //private IEnumerator CheckKeyMovement()
    //{
    //    while (true)
    //    {
    //        if (Vector3.Distance(transform.position, socket.transform.position) < 0.1f)
    //        {
    //            Debug.Log("Break");
    //            break;
    //        }

    //        yield return null;
    //    }

    //    if (!isInserted && Vector3.Distance(transform.position, socket.transform.position) < 0.1f)
    //    {
    //        InsertKey();
    //    }
    //}

    private void InsertKey()
    {
        keyHoleLight.enabled = false;
        keyLight.enabled = false;
        isInserted = true;

        if (insertSound != null)
        {
            audioSource.PlayOneShot(insertSound);
        }

        transform.position = socket.transform.position;
        transform.rotation = socket.transform.rotation;

        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;

        grabInteractable.trackPosition = false;
        grabInteractable.trackRotation = false;

        DoorOpened();
    }


    private void DoorOpened()
    {
        socket.SetActive(false);

        door1Handle.enabled = true;
        door2Handle.enabled = true;

        door1Rigidbody.isKinematic = false;
        door2Rigidbody.isKinematic = false;

        GetComponent<Collider>().enabled = false;
        Debug.Log("Дверь открыта!");
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        grabInteractable.selectExited.RemoveListener(OnSelectExited);
    }
}

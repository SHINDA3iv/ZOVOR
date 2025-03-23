using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandleXRGrabInteractable : XRGrabInteractable
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        StartCoroutine(CancelGrabWhenHandMove(args.interactorObject.transform.parent));
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {

        base.OnSelectExited(args);
    }
    private IEnumerator CancelGrabWhenHandMove(Transform handTransform)
    {
        while (true)
        {
            Vector3 distance = this.transform.position - handTransform.position;

            if (distance.magnitude > 0.3f)
            {
                this.enabled = false;
                this.enabled = true;
                yield break;
            }
            yield return null;
        }
    }
}
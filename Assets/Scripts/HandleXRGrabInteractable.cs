using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandleXRGrabInteractable : XRGrabInteractable
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        StartCoroutine(CancleGrabWhenHandMove(args.interactorObject.transform.parent));
    }

    private IEnumerator CancleGrabWhenHandMove(Transform handTransform)
    {
        while (true)
        {
            Vector3 distance = this.transform.position - handTransform.position;

            if (distance.magnitude > 0.7f)
            {
                this.enabled = false;
                this.enabled = true;
                yield break;
            }
            yield return null;
        }
    }
}

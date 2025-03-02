using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandleXRGrabInteractable : XRGrabInteractable
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectedEnterd(args);

        StartCoroutine(CancleGrabWhenHandMove(args.interactorObject.transform.position));
    }

    private IEnumerator CancleGrabWhenHandMove(Transform handTransform)
    {
        while (true)
        {
            Vector3 distance = this.trasform.position - handTransform.position;

            if (distance.magnitude < 0)
            {
                this.enabled = false;
                this.enabled = true;
                yield break;
            }
            yield return null;
        }
    }
}

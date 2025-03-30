using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Content.Interaction;

public class KnobController : MonoBehaviour
{
    public XRKnob xrKnob;
    public ParticleSystem particleSystem; 
    public GameObject objectToDelete; 

    private void OnEnable()
    {
        xrKnob.onValueChange.AddListener(OnValueChanged);
    }

    private void OnDisable()
    {
        xrKnob.onValueChange.RemoveListener(OnValueChanged);
    }

    private void OnValueChanged(float newValue)
    {
        if (newValue > 2f)
        {
            xrKnob.enabled = false;
            if (particleSystem != null)
            {
                particleSystem.Stop(); 
                Destroy(particleSystem.gameObject); 
            }
            if (objectToDelete != null)
            {
                Destroy(objectToDelete);
            }
        }
    }
}
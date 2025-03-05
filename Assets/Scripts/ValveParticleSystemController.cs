using UnityEngine;

public class ValveParticleSystemController : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public Collider valveCollider;

    private RotatableObject rotatableObject;

    void Start()
    {
        rotatableObject = GetComponent<RotatableObject>();

        rotatableObject.OnObjectRotatedEvent += RotatedObject;
    }

    void RotatedObject()
    {
        particleSystem.Stop();

        if (valveCollider != null)
        {
            valveCollider.enabled = false;
        }
        else
        {
            Debug.LogWarning("Коллайдер не найден! Убедитесь, что он назначен в инспекторе или существует на объекте.");
        }
    }
}
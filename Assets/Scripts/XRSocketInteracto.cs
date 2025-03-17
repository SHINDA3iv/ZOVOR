using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MyXRSocketInteractor : XRSocketInteractor
{
    public bool IsAvailable { get; private set; } = true;

    public void SetAvailability(bool isAvailable)
    {
        IsAvailable = isAvailable;
        UpdateVisualFeedback(); // Обновляем визуальную обратную связь (например, подсветку)
    }

    private void UpdateVisualFeedback()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = IsAvailable ? Color.green : Color.red;
        }
    }
}
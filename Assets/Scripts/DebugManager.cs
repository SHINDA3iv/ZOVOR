using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugManager : MonoBehaviour
{
    public static DebugManager debugManager;
    public TextMeshProUGUI debugText;

    private List<string> messages = new List<string>();

    private void Awake()
    {
        debugManager = this;
    }

    public void DisplayMessage(string message, float duration = 3.0f)
    {
        messages.Add(message);
        UpdateDebugText();
        StartCoroutine(RemoveMessageAfterDelay(message, duration));
    }

    private void UpdateDebugText()
    {
        debugText.text = string.Join("\n", messages.ToArray());
    }

    private IEnumerator RemoveMessageAfterDelay(string message, float delay)
    {
        yield return new WaitForSeconds(delay);
        messages.Remove(message);
        UpdateDebugText();
    }
}

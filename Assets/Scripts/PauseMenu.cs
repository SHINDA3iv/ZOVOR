using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuCanvas; // ��� ������ � ScrollView
    [SerializeField] bool isVisible = false;

    [SerializeField] private InputActionProperty buttonAction;

    private void OnEnable()
    {
        // �������� �� ������� ������� ������
        buttonAction.action.performed += OnButtonPressed;
        buttonAction.action.Enable();
    }

    private void OnDisable()
    {
        // ������� �� �������
        buttonAction.action.performed -= OnButtonPressed;
        buttonAction.action.Disable();
    }

    private void OnButtonPressed(InputAction.CallbackContext context)
    {
        Debug.Log("������ ������ �� �����������!");
        menuCanvas.SetActive(!isVisible);
        isVisible = !isVisible;
    }
}

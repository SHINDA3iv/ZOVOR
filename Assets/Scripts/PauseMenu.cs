using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuCanvas; // Сам канвас с ScrollView
    [SerializeField] bool isVisible = false;

    [SerializeField] private InputActionProperty buttonAction;

    public Transform playerCamera;

    private void OnEnable()
    {
        // Подписка на событие нажатия кнопки
        buttonAction.action.performed += OnButtonPressed;
        buttonAction.action.Enable();
    }

    private void OnDisable()
    {
        // Отписка от события
        buttonAction.action.performed -= OnButtonPressed;
        buttonAction.action.Disable();
    }

    private void OnButtonPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Нажата кнопка на контроллере!");
        menuCanvas.SetActive(!isVisible);
        isVisible = !isVisible;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR; // Для работы с XR
using UnityEngine.XR.Interaction.Toolkit; // Для работы с XR Interaction Toolkit

public class VRManager : MonoBehaviour
{
    public XRController leftHandController;  // Ссылка на левый контроллер
    public XRController rightHandController; // Ссылка на правый контроллер
    public bool enableRayInteractors = true; // Включать ли лучи

    void Start()
    {
        // Проверяем, запущен ли билд на шлеме
        if (!IsRunningOnVRDevice())
        {
            Debug.Log("Running in non-VR mode. Enabling emulator and configuring settings...");

            // 1. Включить эмулятор VR
            EnableVREmulator();

            // 2. Отключить форс граб
            DisableForceGrab();

            // 3. Включить лучи у XR Origin
            EnableRayInteractors();
        }
        else
        {
            Debug.Log("Running on a VR device. No emulator or additional settings needed.");
        }
    }

    /// <summary>
    /// Проверяет, запущен ли проект на реальном VR-устройстве.
    /// </summary>
    /// <returns>True, если запущен на VR-устройстве; false, если нет.</returns>
    private bool IsRunningOnVRDevice()
    {
        // Проверяем, активно ли XR устройство
        return XRSettings.isDeviceActive;
    }

    /// <summary>
    /// Включает эмулятор VR (если используется OpenXR или другой плагин).
    /// </summary>
    private void EnableVREmulator()
    {
        Debug.LogWarning("XR is not active. Attempting to enable VR emulator...");
        XRSettings.LoadDeviceByName("OpenXR"); // Загрузка устройства OpenXR
        XRSettings.enabled = true; // Включение XR
        Debug.Log("VR emulator enabled.");
    }

    /// <summary>
    /// Отключает форс граб (если это применимо).
    /// </summary>
    private void DisableForceGrab()
    {
        // Здесь нужно добавить логику для отключения форс граба
        // Например, если это связано с каким-то компонентом, найдите его и измените параметры
        Debug.Log("Force grab disabled (if applicable).");
    }

    /// <summary>
    /// Включает лучи у XR Origin.
    /// </summary>
    private void EnableRayInteractors()
    {
        if (leftHandController != null && rightHandController != null)
        {
            // Находим компоненты Ray Interactor на контроллерах
            var leftRayInteractor = leftHandController.GetComponent<XRInteractorLineVisual>();
            var rightRayInteractor = rightHandController.GetComponent<XRInteractorLineVisual>();

            if (leftRayInteractor != null && rightRayInteractor != null)
            {
                leftRayInteractor.enabled = enableRayInteractors;
                rightRayInteractor.enabled = enableRayInteractors;

                Debug.Log("Ray interactors enabled on both controllers.");
            }
            else
            {
                Debug.LogWarning("Ray interactors not found on controllers.");
            }
        }
        else
        {
            Debug.LogError("Left or Right Hand Controller is not assigned in the Inspector.");
        }
    }
}
using UltimateXR.Avatar;
using UltimateXR.Core;
using UltimateXR.Devices;
using UltimateXR.Locomotion;
using UnityEngine;

public class CustomSmoothLoco: MonoBehaviour
{
    [SerializeField]
    private CharacterController Player;
    [SerializeField]
    private UxrAvatar Avatar;

    
    
    [Header("General parameters")]
    [SerializeField] private float _metersPerSecondNormal = 2.0f;
    [SerializeField] private float _metersPerSecondSprint = 4.0f;
    [SerializeField] private float _rotationDegreesPerSecondNormal = 120.0f;
    [SerializeField] private float _rotationDegreesPerSecondSprint = 120.0f;
    [Header("Input parameters")]
    [SerializeField] private UxrHandSide _sprintButtonHand = UxrHandSide.Left;
    [SerializeField] private UxrInputButtons _sprintButton = UxrInputButtons.Joystick;
    [SerializeField] private UxrWalkDirection _walkDirection = UxrWalkDirection.AvatarForward;

    private Vector3 _moveSpeed;


    private void Update()
    {
        Locomotion();
        Player.SimpleMove(_moveSpeed);
    }


    private void Locomotion()
    {
        if (Avatar)
        {
            // Get input

            Vector2 joystickLeft = Vector2.zero;
            Vector2 joystickRight = Vector2.zero;

            if (Avatar.ControllerInput.SetupType == UxrControllerSetupType.Dual)
            {
                // Two controllers with joystick
                joystickLeft = Avatar.ControllerInput.GetInput2D(UxrHandSide.Left, UxrInput2D.Joystick);
                joystickRight = Avatar.ControllerInput.GetInput2D(UxrHandSide.Right, UxrInput2D.Joystick);
            }
            else if (Avatar.ControllerInput.SetupType == UxrControllerSetupType.Single)
            {
                // Single controller with 2 joysticks (gamepad?)
                joystickLeft = Avatar.ControllerInput.GetInput2D(UxrHandSide.Left, UxrInput2D.Joystick);
                joystickRight = Avatar.ControllerInput.GetInput2D(UxrHandSide.Left, UxrInput2D.Joystick2);
            }

            Vector3 offset = Vector3.zero;

            if (_walkDirection == UxrWalkDirection.ControllerForward)
            {
                Transform forwardTransform = Avatar.GetControllerInputForward(UxrHandSide.Left);

                if (forwardTransform != null)
                {
                    offset = Vector3.ProjectOnPlane(forwardTransform.forward, Vector3.up).normalized * joystickLeft.y +
                             Vector3.ProjectOnPlane(forwardTransform.right, Vector3.up).normalized * joystickLeft.x;
                }
            }
            else if (_walkDirection == UxrWalkDirection.AvatarForward)
            {
                offset = Avatar.transform.forward * joystickLeft.y + Avatar.transform.right * joystickLeft.x;
            }
            else if (_walkDirection == UxrWalkDirection.LookDirection)
            {
                offset = Vector3.ProjectOnPlane(Avatar.CameraComponent.transform.forward, Vector3.up).normalized * joystickLeft.y +
                         Vector3.ProjectOnPlane(Avatar.CameraComponent.transform.right, Vector3.up).normalized * joystickLeft.x;
            }

            if (offset.magnitude > 1.0f)
            {
                offset.Normalize();
            }

            // Compute translation speed for UpdateLocomotionPhysics()

            bool isSprinting = Avatar.ControllerInput.GetButtonsPress(_sprintButtonHand, _sprintButton);

            float speed = isSprinting ? _metersPerSecondSprint : _metersPerSecondNormal;
            _moveSpeed = offset * speed;

            // Rotation. We perform it here since it doesn't require any collision checks.

            if (!Mathf.Approximately(joystickRight.x, 0.0f))
            {
                float rotationSpeed = isSprinting ? _rotationDegreesPerSecondSprint : _rotationDegreesPerSecondNormal;
                UxrManager.Instance.RotateAvatar(Avatar, joystickRight.x * rotationSpeed * Time.deltaTime);
            }
        }
    }
}

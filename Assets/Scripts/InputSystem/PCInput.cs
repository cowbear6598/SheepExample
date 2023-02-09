using CameraSystem;
using UnityEngine;
using Zenject;

namespace InputSystem
{
    public class PCInput : IInput, IInitializable
    {
        [Inject] private readonly ICameraService cameraService;

        private PlayerInput playerInput;

        public void Initialize()
        {
            playerInput = new PlayerInput();
            playerInput.Enable();
        }

        public bool IsPress() => playerInput.Player.Press.WasPressedThisFrame();

        public Ray GetPressRay()
        {
            var mousePosition = playerInput.Player.PressPosition.ReadValue<Vector2>();

            return cameraService.GetRayFromScreenPoint(mousePosition);
        }
    }
}
using UnityEngine;

namespace InputSystem
{
    public interface IInput
    {
        public bool IsPress();
        public Ray GetPressRay();
    }
}
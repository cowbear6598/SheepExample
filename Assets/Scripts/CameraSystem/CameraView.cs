using UnityEngine;

namespace CameraSystem
{
    public interface ICameraService
    {
        public Ray GetRayFromScreenPoint(Vector2 mousePosition);
    }
    
    public class CameraView : MonoBehaviour, ICameraService
    {
        [SerializeField] private Camera cam;

        public Ray GetRayFromScreenPoint(Vector2 mousePosition) => cam.ScreenPointToRay(mousePosition);
    }
}
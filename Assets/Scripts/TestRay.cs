using UnityEngine;

namespace JBK.AstrocyteViewer
{
    public class TestRay : MonoBehaviour
    {
        private Transform _transform;
        private Controls.MouseActions _mouse;
        private Camera _mainCamera;
        
        private void Awake()
        {
            _mouse = new Controls().Mouse;
            _mouse.Enable();
            
            _mainCamera = Camera.main;
            _transform = transform;
        }

        private void Update()
        {
            var ray = _mainCamera.ScreenPointToRay(_mouse.MousePosition.ReadValue<Vector2>());
            _transform.position = ray.origin + ray.direction * 3;
        }
    }
}
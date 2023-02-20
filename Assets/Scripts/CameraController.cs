using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace JBK.AstrocyteViewer
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float rotationSensitivity = 0.2f;
        [SerializeField] private float panSensitivity = 0.2f;
        [SerializeField] private float zoomSensitivity = 0.2f;

        private Controls.CamMovementActions _camMovementActions;
        private float _cameraDistance;
        private bool _isDragging;
        private bool _isPanning;
        private Transform _transform;
        private Transform _pivotPoint;
        private EventSystem _eventSystem;

        private void Awake()
        {
            _transform = transform;
            _cameraDistance = _transform.localPosition.z;
            _pivotPoint = _transform.parent;
            _eventSystem = EventSystem.current;
            
            _camMovementActions = new Controls().CamMovement;
            _camMovementActions.Enable();
        }
        
        private void OnEnable()
        {
            _camMovementActions.MouseClick.started += OnStartMouseClick;
            _camMovementActions.MouseClick.canceled += OnEndMouseClick;
            _camMovementActions.PanActive.started += OnStartPan;
            _camMovementActions.PanActive.canceled += OnEndPan;
            _camMovementActions.MouseScroll.performed += Zoom;
        }
        
        private void OnDisable()
        {
            _camMovementActions.MouseClick.started -= OnStartMouseClick;
            _camMovementActions.MouseClick.canceled -= OnEndMouseClick;
            _camMovementActions.PanActive.started -= OnStartPan;
            _camMovementActions.PanActive.canceled -= OnEndPan;
            _camMovementActions.MouseScroll.performed -= Zoom;
        }

        private void OnStartMouseClick(InputAction.CallbackContext obj)
        {
            if (_eventSystem != null && _eventSystem.IsPointerOverGameObject()) return;  //don't drag if over UI
            _isDragging = true;
        }

        private void OnEndMouseClick(InputAction.CallbackContext obj) => _isDragging = false;

        private void OnStartPan(InputAction.CallbackContext obj) => _isPanning = true;

        private void OnEndPan(InputAction.CallbackContext obj) => _isPanning = false;

        private void Update()
        {
            if (!_isDragging) return;

            var mouseDelta = _camMovementActions.MouseDelta.ReadValue<Vector2>();

            if (!_isPanning)
                Rotate(mouseDelta);
            else
                Pan(mouseDelta);
        }

        private void Pan(Vector2 delta)
        {
            delta *= panSensitivity * -_cameraDistance;
            _pivotPoint.Translate( -delta.x, -delta.y, 0f, Space.Self);
        }

        private void Rotate(Vector2 delta)
        {
             delta *= rotationSensitivity;
            _pivotPoint.Rotate(-delta.y, delta.x, 0f, Space.Self);        
        }

        private void Zoom(InputAction.CallbackContext ctx)
        {
            _cameraDistance += ctx.ReadValue<float>() * zoomSensitivity;
            _cameraDistance = Mathf.Clamp(_cameraDistance, -100f, -0.1f);
            _transform.localPosition = new Vector3(0f, 0f, _cameraDistance);
        }
    }
}

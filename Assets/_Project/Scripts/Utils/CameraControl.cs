using Cinemachine;
using UnityEngine;
using CodeMonkey.Utils;

namespace TheSuperShell
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float zoomSpeed = 10f;

        private bool drag = false;
        private Vector2 lastMousePosition;
        private float targetSize = 20;
        private Transform follow;

        public void Connect(Transform transform) => follow = transform;
        public void Disconnect() => follow = null;

        private void Update()
        {
            HandleCameraMovement();

            HandleCameraZoom();

            if (follow != null)
                transform.position = follow.position;
        }

        private void HandleCameraZoom()
        {
            if (Input.mouseScrollDelta.y < 0)
                targetSize += 2;
            if (Input.mouseScrollDelta.y > 0)
                targetSize -= 2;

            targetSize = Mathf.Clamp(targetSize, 3, 50);
            
            virtualCamera.m_Lens.OrthographicSize = 
                Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, targetSize, Time.deltaTime * zoomSpeed);

        }

        private void HandleCameraMovement()
        {
            Vector3 moveDir = Vector3.zero;

            if (Input.GetMouseButtonDown(1))
            {
                drag = true;
                follow = null;
                lastMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
            if (Input.GetMouseButtonUp(1)) drag = false;

            if (drag)
            {
                moveDir = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition) - lastMousePosition;
                lastMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }

            transform.position -= moveDir * 2;
        }
    }
}
